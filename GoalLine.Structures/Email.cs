using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Structures
{
    public class Email
    {
        public int UniqueID { get; set; }
        public DateTime Date { get; set; }
        public int TemplateID { get; set; }
        public bool Read { get; set; }
        public List<int> Data { get; set; }
        public EmailFrom From { get; set; }
        public string FromOtherName { get; set; }
    }

    public class EmailTemplate
    {
        public int UniqueID { get; set; }
        public EmailType Type { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailTemplate(int UniqueID, EmailType Type, string Subject, string Body)
        {
            this.UniqueID = UniqueID;
            this.Type = Type;
            this.Subject = Subject;
            this.Body = Body;
        }
    }

    public class EmailViewable
    {
        public int UniqueID { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool Read { get; set; }
        public string From { get; set; }

    }
}
