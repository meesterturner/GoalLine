using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GoalLine.Data;
using GoalLine.Structures;

namespace GoalLine.Matchday
{
    class MatchPlayer
    {
        Utils u = new Utils();
        private int HomeAdvantage = 7;
        private int EventCountThisSecond;

        public Fixture Fixture { get; set; }
        public bool Interactive { get; set; }
        public IMatchCallback MatchCallback { get; set; }

        List<MatchEventCommentary> CommentaryList;

        MatchStatus MatchStatus;

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

            if(Interactive)
            {
                CommentaryList = GetCommentaries();
            }

            
            MatchStatus = new MatchStatus();
            PopulatePlayerStatuses();
            MatchCallback.MatchStarting(Fixture);

            for (int h = 1; h <= 2; h++)
            {
                if(h == 1)
                {
                    MatchStatus.Segment = MatchSegment.FirstHalf;
                } else
                {
                    MatchStatus.Segment = MatchSegment.SecondHalf;
                }

                MatchStatus.BallX = 0;
                MatchStatus.PossessionTeam = h - 1; // TODO: Randomise for the start, but opposite team for second half.

                RaiseEvent(MatchEventType.KickOff);

                for (int s = 0; s <= Constants.MinsPerHalf * 60 - Constants.EventIntervalSecs; s += Constants.EventIntervalSecs)
                {
                    MatchStatus.TotalTimeUnits++;
                    MatchStatus.SegmentTimeSeconds = s;
                    MatchStatus.MatchTimeSeconds += Constants.EventIntervalSecs;

                    DetermineNextEvent();
                }

                if (h == 1)
                {
                    RaiseEvent(MatchEventType.HalfTime);
                }
                else
                {
                    RaiseEvent(MatchEventType.FullTime);
                }

            }

            Fixture.Score[0] = MatchStatus.Score[0];
            Fixture.Score[1] = MatchStatus.Score[1];
            MatchCallback.MatchFinished(Fixture);

        }

        private void RaiseEvent(MatchEventType Ev)
        {
            EventCountThisSecond++;

            if(!Interactive)
            {
                return; // If non-interactive, no point raising events or deciding on commentary for the UI
            }

            if(Ev == MatchEventType.None)
            {
                MatchCallback.Commentary = "";
            } else
            {
                MatchCallback.Commentary = FindCommentary(Ev);
            }

            MatchCallback.MatchStatus = MatchStatus;
            MatchCallback.EventType = Ev;
            MatchCallback.UpdateUI();
        }

        private string FindCommentary(MatchEventType Ev)
        {
            string retVal = "";

            List<MatchEventCommentary> PossComment = (from C in CommentaryList
                                                      where C.EventType == Ev && (C.Segment == MatchSegment.None || C.Segment == MatchStatus.Segment)
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
            retVal.Add(new MatchEventCommentary(MatchEventType.KickOff, "This hotly anticipated game begins!", MatchSegment.FirstHalf));
            retVal.Add(new MatchEventCommentary(MatchEventType.KickOff, "The second half gets under way!", MatchSegment.SecondHalf));
            retVal.Add(new MatchEventCommentary(MatchEventType.Miss, "Oooh, surely he will be kicking himself for that one!", MatchSegment.None));
            retVal.Add(new MatchEventCommentary(MatchEventType.Save, "Cracking reactions by the keeper there", MatchSegment.None));
            retVal.Add(new MatchEventCommentary(MatchEventType.BadSave, "He's palmed it away with his fingers", MatchSegment.None));
            retVal.Add(new MatchEventCommentary(MatchEventType.Dispossessed, "The ball is intercepted by the opposition", MatchSegment.None));
            retVal.Add(new MatchEventCommentary(MatchEventType.Hoofed, "He hoofs it right up the pitch", MatchSegment.None));
            retVal.Add(new MatchEventCommentary(MatchEventType.Shot, "He's hit it towards the goal...", MatchSegment.None));
            retVal.Add(new MatchEventCommentary(MatchEventType.CornerStart, "He runs up to the corner", MatchSegment.None));
            retVal.Add(new MatchEventCommentary(MatchEventType.Cross, "The ball is crossed in", MatchSegment.None));
            retVal.Add(new MatchEventCommentary(MatchEventType.CornerOpposition, "The ball is safely taken by the opposition", MatchSegment.None));
            retVal.Add(new MatchEventCommentary(MatchEventType.OppositionGotThereFirst, "Their opponents got to the ball before they could", MatchSegment.None));
            return retVal;
        }

        private void PopulatePlayerStatuses()
        {
            List<PlayerStatus> StatusList;

            for(int t = 0; t <= 1; t++)
            {
                MatchStatus.OverallPlayerEffectiveRating[t] = 0;

                TeamAdapter ta = new TeamAdapter();
                Team Team = ta.GetTeam(Fixture.TeamIDs[t]);

                PlayerAdapter pa = new PlayerAdapter();
                StatusList = new List<PlayerStatus>();

                // TODO: This needs putting into a function
                //       Player status only to be refreshed at start of match
                //       Needs doing when we have subs/sending off working
                TacticEvaluation Eval = new TacticEvaluation();

                int Selected = 0;
                int TotalEffectiveRating = 0;

                foreach(KeyValuePair<int, TeamPlayer> kvp in Team.Players)
                {
                
                    PlayerStatus ps = new PlayerStatus();
                    ps.PlayerID = kvp.Value.PlayerID;
                    ps.Playing = kvp.Value.Selected;

                    if(ps.Playing == PlayerSelectionStatus.Starting)
                    {
                        Selected++;

                        Player p = pa.GetPlayer(ps.PlayerID);
                        ps.EffectiveRating = p.EffectiveRating;
                        TotalEffectiveRating += ps.EffectiveRating;

                        Eval.AddRatingForPosition(p.Position, p.EffectiveRating); 
                    }

                    if(ps.Playing != PlayerSelectionStatus.None)
                    {
                        StatusList.Add(ps);
                    }
                    
                    ps = null;
                }

                MatchStatus.OverallPlayerEffectiveRating[t] = TotalEffectiveRating / Selected;
                MatchStatus.PlayerStatuses[t] = StatusList;
                MatchStatus.Evaluation[t] = Eval;
                StatusList = null;
            }
        }

        public void DetermineNextEvent()
        {
            //MatchEventType LastEvent = s.MostRecentEvent;
            EventCountThisSecond = 0;

            bool PossessionChange = u.RandomInclusive(0, MatchStatus.Evaluation[1 - MatchStatus.PossessionTeam].Defence) > u.RandomInclusive(0, MatchStatus.OverallPlayerEffectiveRating[MatchStatus.PossessionTeam] * (Math.Abs(MatchStatus.BallX) + 1));

            if (MatchStatus.PossessionTeam == Constants.HomeTeam && PossessionChange == true)
            {
                if (u.RandomInclusive(0, 100) <= HomeAdvantage)
                {
                    PossessionChange = false; 
                }
            }

            if (PossessionChange)
            {
                if(SuccessfulEvent())
                {
                    MatchStatus.PossessionTeam = 1 - MatchStatus.PossessionTeam;
                    RaiseEvent(MatchEventType.Dispossessed);
                }
            }

            

            bool ShotAttempt = false;
            int ShotDistance;

            if (MatchStatus.PossessionTeam == Constants.HomeTeam)
            {
                ShotDistance = 3 - MatchStatus.BallX;
            } else
            {
                ShotDistance = 3 - (0 - MatchStatus.BallX);
            }

            if(MatchStatus.BallX == -2 && MatchStatus.PossessionTeam == Constants.AwayTeam)
            {
                System.Diagnostics.Debug.Print("");
            }

            int DistanceChance = u.RandomInclusive(0, 100 * ShotDistance);
            int OverallAttemptChance = ((MatchStatus.Evaluation[MatchStatus.PossessionTeam].Midfield / 2) +
                                        MatchStatus.Evaluation[MatchStatus.PossessionTeam].Attack + (MatchStatus.Evaluation[MatchStatus.PossessionTeam].Striker * 2) / 3) / ShotDistance;
            ShotAttempt = DistanceChance <= OverallAttemptChance;

            if (ShotAttempt)
            {
                if (SuccessfulEvent())
                {
                    DoShot();
                }
            }
            else
            {
                if (PossessionChange)
                {
                    if ((MatchStatus.PossessionTeam == Constants.AwayTeam && MatchStatus.BallX == 2) || (MatchStatus.PossessionTeam == Constants.HomeTeam && MatchStatus.BallX == -2))
                    {
                        bool hoofedAway = u.RandomInclusive(0, 100) < MatchStatus.Evaluation[MatchStatus.PossessionTeam].Defence / 4;
                        if (hoofedAway)
                        {
                            RaiseEvent(MatchEventType.Hoofed);
                            MatchStatus.BallX = 0;
                            if(u.RandomInclusive(0,1) == 1)
                            {
                                MatchStatus.PossessionTeam = 1 - MatchStatus.PossessionTeam;
                                RaiseEvent(MatchEventType.OppositionGotThereFirst);
                            }
                        }
                    }
                }

                int ballDirection = MatchStatus.PossessionTeam == Constants.HomeTeam ? 1 : -1;
                bool ballMovesInDirection = SuccessfulEvent();

                if (ballMovesInDirection)
                {
                    if ((MatchStatus.BallX > -2 && ballDirection == -1) || (MatchStatus.BallX < 2 && ballDirection == 1))
                    {
                        MatchStatus.BallX += ballDirection;
                    }
                }
            }

            MatchStatus.PossessionUnits[MatchStatus.PossessionTeam]++;

            if (EventCountThisSecond == 0)
            {
                RaiseEvent(MatchEventType.None);
            }
        }

        private bool SuccessfulEvent()
        {
            int For = 0;
            int Against = 0;

            switch (MatchStatus.BallX)
            {
                case -2:
                    For = MatchStatus.Evaluation[Constants.HomeTeam].Defence;
                    Against = MatchStatus.Evaluation[Constants.AwayTeam].Striker;
                    break;

                case -1:
                    For = MatchStatus.Evaluation[Constants.HomeTeam].Midfield;
                    Against = MatchStatus.Evaluation[Constants.AwayTeam].Attack;
                    break;

                case 0:
                    if (MatchStatus.PossessionTeam == Constants.HomeTeam)
                    {
                        For = MatchStatus.Evaluation[Constants.HomeTeam].Midfield + HomeAdvantage;
                        Against = MatchStatus.Evaluation[Constants.AwayTeam].Midfield;
                    }
                    else
                    {
                        For = MatchStatus.Evaluation[Constants.AwayTeam].Midfield;
                        Against = MatchStatus.Evaluation[Constants.HomeTeam].Midfield;
                    }

                    break;

                case 1:
                    For = MatchStatus.Evaluation[Constants.AwayTeam].Midfield;
                    Against = MatchStatus.Evaluation[Constants.HomeTeam].Attack; 
                    break;

                case 2:
                    For = MatchStatus.Evaluation[Constants.AwayTeam].Defence; 
                    Against = MatchStatus.Evaluation[Constants.HomeTeam].Striker;
                    break;

            }


            return u.RandomInclusive(0, For) >= u.RandomInclusive(0, Against * 2 );
        }

        void DoGoalKick()
        {
            RaiseEvent(MatchEventType.GoalKick);
            MatchStatus.PossessionTeam = 1 - MatchStatus.PossessionTeam;

            int strength = u.RandomInclusive(0, MatchStatus.Evaluation[MatchStatus.PossessionTeam].Goalkeeping);
            int ballDirection = MatchStatus.PossessionTeam == Constants.HomeTeam ? 1 : -1;
            MatchStatus.BallX = 2 * (0 - ballDirection);

            if (strength <= 30)
            {
                MatchStatus.BallX += 1 * ballDirection; // Light kick
            } else if(strength >= 31 && strength <= 66)
            {
                MatchStatus.BallX += 2 * ballDirection; // Medium kick
            } else
            {
                RaiseEvent(MatchEventType.Hoofed);
                MatchStatus.BallX += 3 * ballDirection; // Hard kick
            }

        }

        void DoCorner()
        {
            RaiseEvent(MatchEventType.CornerStart);
            //MatchStatus.PossessionTeam = 1 - MatchStatus.PossessionTeam;
            int ballDirection = MatchStatus.PossessionTeam == Constants.HomeTeam ? 1 : -1;
            MatchStatus.BallX = 2 * (0 - ballDirection);

            int MidChance = u.RandomInclusive(0, MatchStatus.Evaluation[MatchStatus.PossessionTeam].Midfield);
            int DefChance = u.RandomInclusive(0, MatchStatus.Evaluation[1 - MatchStatus.PossessionTeam].Defence);

            RaiseEvent(MatchEventType.Cross);

            if (MidChance > DefChance)
            {
                DoShot(MidChance, DefChance); 
            } else
            {
                RaiseEvent(MatchEventType.CornerOpposition);
                MatchStatus.PossessionTeam = 1 - MatchStatus.PossessionTeam;
            }
        }

        void DoShot()
        {
            DoShot(-1, -1);
        }

        void DoShot(int AttChanceOriginal, int GKChanceOriginal)
        {
            RaiseEvent(MatchEventType.Shot);
            MatchStatus.Shots[MatchStatus.PossessionTeam]++;

            int AttChance;
            int GKChance;

            if (AttChanceOriginal == -1 || GKChanceOriginal == -1)
            {
                AttChance = u.RandomInclusive(0, ((MatchStatus.Evaluation[MatchStatus.PossessionTeam].Striker * 2) + MatchStatus.Evaluation[MatchStatus.PossessionTeam].Attack) / 3);
                GKChance = u.RandomInclusive(0, MatchStatus.Evaluation[1 - MatchStatus.PossessionTeam].Goalkeeping);
            } else
            {
                AttChance = AttChanceOriginal;
                GKChance = GKChanceOriginal;
            }

            bool ShotOnTarget = u.RandomInclusive(0, AttChance) > AttChance * 0.66f; // Two-thirds of a chance of being on target

            if (ShotOnTarget)
            {
                bool ShotSuccess = AttChance > GKChance;
                

                if (ShotSuccess)
                {
                    MatchStatus.Score[MatchStatus.PossessionTeam]++;
                    RaiseEvent(MatchEventType.Goal);

                    MatchStatus.PossessionTeam = 1 - MatchStatus.PossessionTeam;
                    MatchStatus.BallX = 0;
                }
                else
                {
                    bool SafeHands = u.RandomInclusive(0, MatchStatus.Evaluation[1 - MatchStatus.PossessionTeam].Goalkeeping) < GKChance;
                    if (SafeHands)
                    {
                        RaiseEvent(MatchEventType.Save);
                        DoGoalKick();
                    }
                    else
                    {
                        RaiseEvent(MatchEventType.BadSave);
                        DoCorner();
                    }

                }
            }
            else
            {
                RaiseEvent(MatchEventType.Miss);
                DoGoalKick();
            }

            
        }
    }
}
