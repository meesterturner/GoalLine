using System;

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

        public int Value
        {
            get
            {
                int retVal;
                retVal = (Agility + Speed + Stamina) / 3 * 10000;
                return retVal;
            }
        }

        public double Stars
        {
            get
            {
                return (double)EffectiveRating / 20;
            }
        }

        public int EffectiveRating
        {
            get
            {
                return (Agility + Attitude + Speed + Stamina) / 4;
            }
        }
    }
}
