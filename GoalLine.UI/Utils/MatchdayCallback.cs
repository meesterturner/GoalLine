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

namespace GoalLine.UI.Utils
{
    class MatchdayCallback : IMatchCallback
    {
        private Data.Utils u = new Data.Utils();

        public MatchdayMain UI { get; set; }
        public string Commentary { get; set; }
        public MatchEventType EventType { get; set; }
        public MatchStatus MatchStatus { get; set; }

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
            DoEvents();

            return true;
        }

        public void UpdateUI()
        {
            bool b = UpdateUI_InvokeThread();
        }

        private bool UpdateUI_InvokeThread()
        {
            return UI.Dispatcher.Invoke(() => UpdateUI_Worker());
        }

        private bool UpdateUI_Worker()
        {
            int PauseTime;
            if (EventType == MatchEventType.Goal)
            {
                PauseTime = 1500;
            }
            else if (EventType == MatchEventType.None)
            {
                PauseTime = 75;
            } 
            else
            {
                PauseTime = 750;
            }

            UI.txtTime.Text = DisplayMatchTime();
            UI.txtEvents.Text = Commentary;
            UI.txtHomeScore.Text = MatchStatus.Score[0].ToString();
            UI.txtAwayScore.Text = MatchStatus.Score[1].ToString();


            // TODO: Change the randomness when we go to have a BallY
            if(EventType == MatchEventType.KickOff || EventType == MatchEventType.HalfTime || EventType == MatchEventType.FullTime)
            {
                UI.pitPitch.Animate = false;
            }
            else
            {
                UI.pitPitch.Animate = true;
            }
            UI.pitPitch.AnimateMillisecs = PauseTime;
            UI.pitPitch.BallPosition = (MatchStatus.BallX, u.RandomInclusive((int)UI.pitPitch.MinY, (int)UI.pitPitch.MaxY));
            DoEvents();

            if (EventType == MatchEventType.Goal)
            {
                System.Threading.Thread.Sleep(50);
                UI.txtEvents.Text = "*** GOAL ***";
                DoEvents();
            }
            System.Threading.Thread.Sleep(PauseTime);

            DoEvents();
            return true;
        }

        private string DisplayMatchTime()
        {
            return (MatchStatus.MatchTimeSeconds / 60).ToString("00") + ":" + (MatchStatus.MatchTimeSeconds % 60).ToString("00");
        }

        private void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
        }
    }
}
