using System;
using System.Collections.Generic;
using System.Linq;
using GoalLine.Structures;
using GoalLine.Resources;

namespace GoalLine.Data
{
    public class EmailAdapter
    {

        public void SendEmail(int ToManagerID, EmailType Type, string FromName, List<int> PlaceholderIDs)
        {
            SendEmailWorker(ToManagerID, Type, EmailFrom.Other, FromName, PlaceholderIDs);
        }

        public void SendEmail(int ToManagerID, EmailType Type, EmailFrom FromEntity, List<int> PlaceholderIDs)
        {
            SendEmailWorker(ToManagerID, Type, FromEntity, "", PlaceholderIDs);
        }

        public void SendEmail(int ToManagerID, EmailType Type, List<int> PlaceholderIDs)
        {
            SendEmailWorker(ToManagerID, Type, EmailFrom.BoardOfDirectors, "", PlaceholderIDs);
        }

        private void SendEmailWorker(int ToManagerID, EmailType Type, EmailFrom FromEntity, string FromName, List<int> PlaceholderIDs)
        {
            WorldAdapter wa = new WorldAdapter();

            Email e = new Email();
            e.From = FromEntity;
            e.FromOtherName = (FromEntity == EmailFrom.Other ? FromName : "");
            e.UniqueID = wa.GetNextEmailID();
            e.Date = wa.CurrentDate;
            e.Read = false;
            e.TemplateID = GetRandomEmailTemplateForType(Type).UniqueID;
            e.Data = PlaceholderIDs;

            if(!World.Emails.ContainsKey(ToManagerID))
            {
                World.Emails[ToManagerID] = new List<Email>();
            }

            World.Emails[ToManagerID].Add(e);
        }

        private EmailTemplate GetRandomEmailTemplateForType(EmailType Type)
        {
            // Get all templates by type
            List<EmailTemplate> templates = (from t in LangResources.CurLang.EmailTemplates
                                             where t.Type == Type
                                             select t).ToList();
            int count = templates.Count();

            // return a random one from the list, or just the one in the list if only one
            if(count == 0)
            {
                throw new Exception(String.Format("No emails available for type {0}", Type.ToString()));
            } else if(count == 1)
            {
                return templates[0];
            } else
            {
                Maths u = new Maths();
                int r = u.RandomInclusive(0, count - 1);
                return templates[r];
            }
        }

        public List<Email> GetEmails(int ManagerID)
        {
            return (from Email e in World.Emails[ManagerID]
                    orderby e.UniqueID descending
                    select e).ToList();
        }

        public Email GetEmail(int ManagerID, int EmailID)
        {
            Email retVal = null;

            if(World.Emails.ContainsKey(ManagerID))
            {
                retVal = (from e in World.Emails[ManagerID]
                          where e.UniqueID == EmailID
                          select e).FirstOrDefault();
            }

            return retVal;

        }


        public void MarkEmailAsRead(int ManagerID, int EmailID, bool IsRead)
        {
            if (World.Emails.ContainsKey(ManagerID))
            {
                Email e = GetEmail(ManagerID, EmailID);
                e.Read = IsRead;
            }
        }

        public EmailViewable ConvertEmailToViewable(Email e)
        {
            EmailViewable retVal = new EmailViewable();
            EmailTemplate t = GetEmailTemplate(e);
            
            retVal.UniqueID = e.UniqueID;
            switch(e.From)
            {
                case EmailFrom.BoardOfDirectors:
                    retVal.From = LangResources.CurLang.BoardOfDirectors;
                    break;

                default:
                    retVal.From = e.FromOtherName;
                    break;
            }
            retVal.Date = e.Date;
            retVal.Subject = t.Subject;
            retVal.Read = e.Read;
            retVal.Body = GetEmailBodyTranslated(e, t);

            return retVal;
        }

        private string GetEmailBodyTranslated(Email e, EmailTemplate t)
        {
            EmailTemplate template = GetEmailTemplate(e);
            string retVal = template.Body;

            ManagerAdapter ma = new ManagerAdapter();
            TeamAdapter ta = new TeamAdapter();

            switch (template.Type)
            {
                case EmailType.Welcome:
                    retVal = string.Format(retVal, 
                        ma.GetManager(e.Data[0]).DisplayName(PersonNameReturnType.FirstnameLastname),
                        ta.GetTeam(e.Data[1]).Name);
                    break;

                case EmailType.GoodMatch:
                case EmailType.BadMatch:
                    retVal = string.Format(retVal,
                        e.Data[0].ToString(),
                        e.Data[1].ToString(),
                        ta.GetTeam(e.Data[2]).Name);
                    break;

                default:
                    throw new Exception("Unknown email type");
            }


            return retVal;
        }

        public EmailTemplate GetEmailTemplate(Email e)
        {
            EmailTemplate t = (from templates in LangResources.CurLang.EmailTemplates
                               where templates.UniqueID == e.TemplateID
                               select templates).FirstOrDefault();
            return t;
        }
    }
}
