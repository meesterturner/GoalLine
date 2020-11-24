using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Structures
{
    public class Person
    {
        public bool UseInitial;
        public int UniqueID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CurrentTeam { get; set; } = -1;


        public string Name 
        { 
            get
            {
                return DisplayName(PersonNameReturnType.FirstnameLastname);
            } 
        }

        public string DisplayName(PersonNameReturnType returnType)
        {
            string retVal;

            switch(returnType)
            {
                case PersonNameReturnType.FirstnameLastname:
                    retVal = FirstName + " " + LastName;
                    break;

                case PersonNameReturnType.InitialLastname:
                    retVal = FirstName.Substring(0, 1) + ". " + LastName;
                    break;

                case PersonNameReturnType.InitialOptionalLastname:
                    retVal = (UseInitial ? FirstName.Substring(0, 1) + ". " : "") + LastName;
                    break;

                case PersonNameReturnType.LastnameFirstname:
                    retVal = LastName + ", " + FirstName;
                    break;

                case PersonNameReturnType.LastnameInitial:
                    retVal = LastName + ", " + FirstName.Substring(0, 1) + ".";
                    break;

                case PersonNameReturnType.LastnameInitialOptional:
                    retVal = LastName + (UseInitial ? ", " + FirstName.Substring(0, 1) + "." : "");
                    break;

                default:
                    throw new NotImplementedException();
            }

            return retVal;
        }
    }
}
