using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Structures
{
    public enum PlayerPosition // If this changes, update the languages (PositionsList)
    {
        None = 0,
        Goalkeeper = 1,
        Defender = 2,
        Midfielder = 3,
        Attacker = 4,
        Striker = 5
    }

    public enum PlayerPositionSide // If this changes, update the languages (SidesList)
    {
        Left = -10,
        Centre = 0,
        Right = 10
    }

    public enum PlayerSelectionStatus
    {
        None = 0,
        Starting = 1,
        Sub = 2
    }

    public enum PersonNameReturnType
    {
        FirstnameLastname,
        InitialLastname,
        InitialOptionalLastname,
        LastnameFirstname,
        LastnameInitial,
        LastnameInitialOptional
    }

    public enum EmailType
    {
        Welcome,
        GoodMatch,
        BadMatch
    }

    public enum EmailFrom
    {
        BoardOfDirectors = 0,
        Other = 255
    }

    public enum MatchEventType
    {
        None,
        Goal,
        GoalKick,
        Save,
        Miss,
        Foul,
        FreeKick,
        HalfTime,
        FullTime,
        KickOff,
        Dispossessed,
        Hoofed,
        Shot,
        BadSave,
        CornerAnnounce,
        CornerStart,
        CornerOpposition,
        Cross,
        OppositionGotThereFirst
    }

    public enum MatchSegment
    {
        None,
        FirstHalf,
        SecondHalf,
        ExtraTimeFirstHalf,
        ExtraTimeSecondHalf,
        Penalties
    }
}
