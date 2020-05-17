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
        public int UniqueID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CurrentTeam { get; set; }


        public string Name 
        { 
            get
            {
                return FirstName + " " + LastName;
            } 
        }
    }
}
