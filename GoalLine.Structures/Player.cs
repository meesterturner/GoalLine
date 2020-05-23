using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Structures
{
    public class Player : Person
    {
        public int Agility { get; set; }
        public int Attitude { get; set; }
        public int Speed { get; set; }
        public int Stamina { get; set; }

        public int Wages { get; set; }


        public PlayerPosition Position { get; set; }

        public int PreferenceLeft { get; set; }
        public int PreferenceMiddle { get; set; }
        public int PreferenceRight { get; set; }

        public PlayerPositionSide PreferredSide 
        {
            get 
            {
                if (PreferenceLeft > PreferenceMiddle && PreferenceLeft > PreferenceRight)
                {
                    return PlayerPositionSide.Left;
                } else if (PreferenceRight > PreferenceMiddle && PreferenceRight > PreferenceLeft)
                {
                    return PlayerPositionSide.Right;
                } else
                {
                    return PlayerPositionSide.Centre;
                }

                throw new ArgumentOutOfRangeException("Unknown preferred side");
             } 
        }

        public int PreferredSideInt
        {
            get
            {
                return (int)PreferredSide;
            }   
        }

        public int PositionInt
        {
            get
            {
                return (int)Position;
            }
        }

        public string PositionAndSideTextCode
        {
            get
            {
                if(Position == PlayerPosition.Goalkeeper || Position == PlayerPosition.Striker)
                {
                    return Position.ToString().Substring(0, 1);
                } else
                {
                    return Position.ToString().Substring(0, 1) + " " + PreferredSide.ToString().Substring(0, 1);
                }
                
            }
        }

        public int Value
        {
            get
            {
                int retVal;
                retVal = (Agility + Speed + Stamina) / 3 * 10000;
                return retVal;
            }
        }

        public int Stars
        {
            get
            {
                return  ((Agility + Attitude + Speed + Stamina) / 4) / 20;
            }
        }
    }
}
