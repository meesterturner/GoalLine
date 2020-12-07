using System;

namespace GoalLine.Structures
{
    public class Player : Person
    {
        public int Agility { get; set; }
        public int Attitude { get; set; }
        public int Speed { get; set; }
        public int Stamina { get; set; }
        public int Passing { get; set; }
        public int Marking { get; set; }
        public int Balance { get; set; }
        public int Tackling { get; set; }
        public int Shooting { get; set; }
        public int Handling { get; set; }
        public int Heading { get; set; }
        public int Influence { get; set; }

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
                return (double)OverallRating / 20;
            }
        }

        public int OverallRating
        {
            get
            {
                int standardSet = Agility + Attitude + Speed;
                switch (Position)
                {
                    case PlayerPosition.Goalkeeper:
                        return (Agility + Attitude + Handling + Balance + Passing) / 5;

                    case PlayerPosition.Defender:
                        return (standardSet + Stamina + Passing + Marking + Tackling + Heading) / 7;

                    case PlayerPosition.Midfielder:
                        return (standardSet + Stamina + Passing + Tackling + Heading) / 7;

                    case PlayerPosition.Attacker:
                        return (standardSet + Stamina + Passing + Tackling + Shooting) / 7;

                    case PlayerPosition.Striker:
                        return (standardSet + Stamina + Shooting) / 5;

                    default:
                        return (standardSet + Stamina) / 4;

                }
            }
        }
    }
}
