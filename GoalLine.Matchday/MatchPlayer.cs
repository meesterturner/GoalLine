using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Data;
using GoalLine.Structures;

namespace GoalLine.Matchday
{
    public class MatchPlayer
    {
        public Fixture Fixture { get; set; }
        public bool Interactive { get; set; }
        public IMatchCallback MatchCallback { get; set; }

        List<MatchEventCommentary> Comm;

        public MatchPlayer()
        {
            Comm = GetCommentaries();
        }


        public void StartMatch()
        {
            if(Fixture == null)
            {
                throw new ArgumentNullException("Fixture not set");
            }

            if(Interactive == true && MatchCallback == null)
            {
                throw new ArgumentNullException("MatchCallback not set");
            }

            // TEST CODE!!!
            MatchCallback.Segment = MatchSegment.FirstHalf;
            MatchCallback.EventType = MatchEventType.KickOff;
            for(int s = 0; s <= 45 * 60; s = s + 15)
            {
                MatchCallback.SegmentTimeSeconds = s;

                int RandomEvent = new Random().Next(0, 36); // 0-35, Thanks .net ;-) 

                if(RandomEvent <= 9)
                {
                    RaiseEvent((MatchEventType)RandomEvent);
                } else
                {
                    RaiseEvent(MatchEventType.None);
                }
            }
            
        }

        private void RaiseEvent(MatchEventType Ev)
        {
            if(Ev == MatchEventType.None)
            {
                MatchCallback.Commentary = "";
            } else
            {
                MatchCallback.Commentary = FindCommentary(Ev);
            }
            
            MatchCallback.EventType = Ev;
            MatchCallback.UpdateUI();
        }

        private string FindCommentary(MatchEventType Ev)
        {
            string retVal = "";

            List<MatchEventCommentary> PossComment = (from C in Comm
                                                      where C.EventType == Ev
                                                      select C).ToList();

            retVal = PossComment[0].RawText;
            return retVal;
        }

        private List<MatchEventCommentary> GetCommentaries()
        {
            List<MatchEventCommentary> retVal = new List<MatchEventCommentary>();

            retVal.Add(new MatchEventCommentary(MatchEventType.Foul, "His legs were hacked off right in front of the crowd", MatchSegment.None));
            retVal.Add(new MatchEventCommentary(MatchEventType.FreeKick, "He goes to take the free kick", MatchSegment.None));
            retVal.Add(new MatchEventCommentary(MatchEventType.FullTime, "And that's the end of the match!", MatchSegment.None));
            retVal.Add(new MatchEventCommentary(MatchEventType.Goal, "The keeper had no chance with that one!", MatchSegment.None));
            retVal.Add(new MatchEventCommentary(MatchEventType.GoalKick, "The keeper boots it down the pitch", MatchSegment.None));
            retVal.Add(new MatchEventCommentary(MatchEventType.HalfTime, "The end of a thrilling first half", MatchSegment.None));
            retVal.Add(new MatchEventCommentary(MatchEventType.KickOff, "This hotly anticipated game begins!", MatchSegment.None));
            retVal.Add(new MatchEventCommentary(MatchEventType.Miss, "Oooh, surely he will be kicking himself for that one!", MatchSegment.None));
            retVal.Add(new MatchEventCommentary(MatchEventType.Save, "Cracking reactions by the keeper there", MatchSegment.None));

            return retVal;
        }
    }
}
