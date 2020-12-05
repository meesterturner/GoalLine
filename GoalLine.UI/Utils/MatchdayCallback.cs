using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Matchday;
using GoalLine.Structures;
using GoalLine.Data;
using GoalLine.UI.GameScreens;
using System.Windows.Threading;
using System.Windows;
using System.Threading;

namespace GoalLine.UI.Utils
{
    class MatchdayCallback : IMatchCallback
    {
        private Data.Utils u = new Data.Utils();

        public MatchdayMain UI { get; set; }
        public string Commentary { get; set; }
        public MatchEventType EventType { get; set; }
        public MatchStatus MatchStatus { get; set; }
        private int[] PauseLengths = new int[] { 250, 750, 2000 };

        public MatchdayCallback(MatchdayMain SourceUI)
        {
            UI = SourceUI;
        }

        public void MatchdayComplete()
        {
            UI.MatchdayComplete = true;
        }

        public void MatchFinished(Fixture f, bool Interactive)
        {
            bool b = MatchFinished_InvokeThread(f);
        }

        private bool MatchFinished_InvokeThread(Fixture f)
        {
            return UI.Dispatcher.Invoke(() => MatchFinished_Worker(f));
        }

        private bool MatchFinished_Worker(Fixture f)
        {
            //TeamAdapter ta = new TeamAdapter();
            //string result = "{0} {1} : {2} {3}";

            //result = String.Format(result, ta.GetTeam(f.TeamIDs[0]).Name,
            //    f.Score[0],
            //    f.Score[1],
            //    ta.GetTeam(f.TeamIDs[1]).Name);

            //UI.txtResults.Text = UI.txtResults.Text + result + "\n";

            return true;
        }

        public void MatchStarting(Fixture f, bool Interactive)
        {
            if(UI == null)
            {
                throw new Exception("MatchdayCallback.UI has not been set");
            }

            if (Interactive)
            {
                bool b = MatchStarting_InvokeThread(f);
            }
        }

        private bool MatchStarting_InvokeThread(Fixture f)
        {
            return UI.Dispatcher.Invoke(() => MatchStarting_Worker(f));
        }

        private bool MatchStarting_Worker(Fixture f)
        {
            TeamAdapter ta = new TeamAdapter();
            UI.txtHome.Text = ta.GetTeam(f.TeamIDs[0]).Name;
            UI.txtAway.Text = ta.GetTeam(f.TeamIDs[1]).Name;

            return true;
        }

        public void UpdateUI()
        {
            bool b = UpdateUI_InvokeThread();
            int pause;

            switch(EventType)
            {
                case MatchEventType.Goal:
                    pause = PauseLengths[2];
                    break;

                case MatchEventType.None:
                    pause = PauseLengths[0];
                    break;

                default:
                    pause = PauseLengths[1];
                    break;

            }

            Thread.Sleep(pause);
        }

        private bool UpdateUI_InvokeThread()
        {
            return UI.Dispatcher.Invoke(() => UpdateUI_Worker());
        }

        private bool UpdateUI_Worker()
        {
            UI.txtTime.Text = DisplayMatchTime();
            UI.txtEvents.Text = Commentary;
            UI.txtHomeScore.Text = MatchStatus.Score[0].ToString();
            UI.txtAwayScore.Text = MatchStatus.Score[1].ToString();


            
            if(EventType == MatchEventType.KickOff || EventType == MatchEventType.HalfTime || EventType == MatchEventType.FullTime)
            {
                UI.pitPitch.CentreBall();
            }
            else
            {
                UI.pitPitch.Animate = true;
            }

            UI.pitPitch.AnimateMillisecs = PauseLengths[0] / 2;
            int destX = MatchStatus.BallX + 2;
            int destY = u.RandomInclusive(0, UI.pitPitch.SegmentCountY - 1);

            // TODO: Change the randomness when we go to have a BallY
            // TODO: No need to convert Ballx when we change match engine
            //UI.pitPitch.BallPosition = (destX, destY);

            if (EventType == MatchEventType.Goal)
            {
                UI.txtEvents.Text = "*** GOAL ***";
                destY = 1;
                destX = (MatchStatus.PossessionTeam == 0 ? UI.pitPitch.SegmentCountX - 1 : 0);
            }

            UI.pitPitch.BallPosition = (destX, destY);
            return true;
        }

        private string DisplayMatchTime()
        {
            return (MatchStatus.MatchTimeSeconds / 60).ToString("00") + ":" + (MatchStatus.MatchTimeSeconds % 60).ToString("00");
        }
    }
}
