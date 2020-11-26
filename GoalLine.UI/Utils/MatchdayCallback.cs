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
            TeamAdapter ta = new TeamAdapter();
            string result = "{0} {1} : {2} {3}";

            result = String.Format(result, ta.GetTeam(f.TeamIDs[0]).Name,
                f.Score[0],
                f.Score[1],
                ta.GetTeam(f.TeamIDs[1]).Name);

            UI.txtResults.Text = UI.txtResults.Text + result + "\n";

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
            UI.txtTime.Text = DisplayMatchTime();
            UI.txtEvents.Text = Commentary;
            UI.txtScore.Text = String.Format(" {0} : {1} ", MatchStatus.Score[0], MatchStatus.Score[1]);
            DoEvents();

            if (EventType == MatchEventType.Goal)
            {
                System.Threading.Thread.Sleep(50);
                UI.txtEvents.Text = "*** GOAL ***";
                DoEvents();
                System.Threading.Thread.Sleep(1500);
            }
            else if (EventType == MatchEventType.None)
            {
                System.Threading.Thread.Sleep(75);
            }
            else
            {
                System.Threading.Thread.Sleep(750);
            }

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
