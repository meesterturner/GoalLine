using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Structures;
using System.Drawing;
using GoalLine.Resources;

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
                    return new ResultData(false, LangResources.CurLang.FormationExists);
                }
            }

            // Do we have the right number of points
            if(Points.Count != 11)
            {
                return new ResultData(false, LangResources.CurLang.FormationInvalid); ;
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

        public SuitablePlayerInfo SuitablePlayerPositions(Point2 p)
        {
            return SuitablePlayerPositions(p.X, p.Y);
        }

        public SuitablePlayerInfo SuitablePlayerPositions(int x, int y)
        {
            List<PlayerPosition> BestPos = new List<PlayerPosition>();
            List<PlayerPositionSide> BestSide = new List<PlayerPositionSide>();

            switch (y)
            {
                case 0:
                    BestPos.Add(PlayerPosition.Goalkeeper);
                    break;

                case 1:
                    BestPos.Add(PlayerPosition.Defender);
                    break;

                case 2:
                    BestPos.Add(PlayerPosition.Defender);
                    BestPos.Add(PlayerPosition.Midfielder);
                    break;

                case 3:
                    BestPos.Add(PlayerPosition.Midfielder);
                    BestPos.Add(PlayerPosition.Midfielder);
                    break;

                case 4:
                    BestPos.Add(PlayerPosition.Midfielder);
                    BestPos.Add(PlayerPosition.Attacker);
                    break;

                case 5:
                    BestPos.Add(PlayerPosition.Attacker);
                    break;

                case 6:
                    BestPos.Add(PlayerPosition.Striker);
                    break;

                default:
                    BestPos.Add(PlayerPosition.None);
                    break;

            }

            switch (x)
            {
                case 0:
                    BestSide.Add(PlayerPositionSide.Left);
                    break;

                case 1:
                    BestSide.Add(PlayerPositionSide.Centre);
                    BestSide.Add(PlayerPositionSide.Left);
                    break;

                case 2:
                    BestSide.Add(PlayerPositionSide.Centre);
                    break;

                case 3:
                    BestSide.Add(PlayerPositionSide.Centre);
                    BestSide.Add(PlayerPositionSide.Right);
                    break;

                case 4:
                    BestSide.Add(PlayerPositionSide.Right);
                    break;

            }

            SuitablePlayerInfo retVal = new SuitablePlayerInfo();
            retVal.Positions = BestPos;
            retVal.Sides = BestSide;

            return retVal;

        }
    }
}
