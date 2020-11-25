using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Structures;
using System.Drawing;

namespace GoalLine.Data
{
    public class FormationAdapter
    {
        public Formation GetFormation(int id)
        {
            return World.Formations[id];
        }

        public List<Formation> GetFormations()
        {
            return World.Formations;
        }

        public ResultData CheckFormation(string Name, List<Point2> Points)
        {
            // Check names
            foreach (Formation f in World.Formations)
            {
                if (f.Name.ToUpper() == Name.ToUpper())
                {
                    return new ResultData(false, "Formation with same name exists");
                }
            }

            // Do we have the right number of points
            if(Points.Count != 11)
            {
                return new ResultData(false, "Formation must have exactly 11 points"); ;
            }

            return new ResultData(true);
        }

        public bool AddFormation(string Name, List<Point2> Points, bool System)
        { 
            if(System) // Let's catch the programmer's errors! 
            {
                ResultData r = CheckFormation(Name, Points);
                if(!r.Success)
                {
                    throw new Exception(r.Description);
                }
            }

            Formation nf = new Formation();
            nf.Name = Name.Trim();
            nf.Points = Points;
            nf.System = System;
            nf.UniqueID = World.Formations.Count();

            World.Formations.Add(nf);

            return true;
        }
    }
}
