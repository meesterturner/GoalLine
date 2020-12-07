using System;
using System.Collections.Generic;
using System.Linq;
using GoalLine.Data;
using GoalLine.Structures;

namespace GoalLine.Matchday
{
    class MatchPlayer
    {
        Maths u = new Maths();
        private int HomeAdvantage = 7;
        private int EventCountThisSecond;

        public Fixture Fixture { get; set; }
        public bool Interactive { get; set; }
        public IMatchCallback MatchCallback { get; set; }

        List<MatchEventCommentary> CommentaryList;

        MatchStatus MatchStatus;

        const double BALLXMIN = 0;
        const double BALLXMAX = 7;
        const double BALLXCENTRE = BALLXMAX / 2;
        const double BALLYMIN = 0;
        const double BALLYMAX = 4;
        const double BALLYCENTRE = BALLYMAX / 2;

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
            MatchCallback.MatchStarting(Fixture, Interactive);
           

            for (int h = 1; h <= 2; h++)
            {
                if(h == 1)
                {
                    MatchStatus.Segment = MatchSegment.FirstHalf;
                } else
                {
                    MatchStatus.Segment = MatchSegment.SecondHalf;
                }

                MatchStatus.BallX = BALLXCENTRE;
                MatchStatus.BallY = BALLYCENTRE;
                MatchStatus.PossessionTeam = h - 1; // TODO: Randomise for the start, but opposite team for second half.

                RaiseEvent(MatchEventType.KickOff);

                // TODO: Just in case we change the grid, this may need to change. But gets us whole numbers.
                //MatchStatus.BallX = (PossessionHome() ? Math.Floor(MatchStatus.BallX) : Math.Ceiling(MatchStatus.BallX)); 

                for (int s = 0; s <= Constants.MinsPerHalf * 60 - Constants.EventIntervalSecs; s += Constants.EventIntervalSecs)
                {
                    MatchStatus.TotalTimeUnits++;
                    MatchStatus.SegmentTimeSeconds = s;
                    MatchStatus.MatchTimeSeconds += Constants.EventIntervalSecs;

                    DetermineNextEvent();
                }

                MatchStatus.BallX = BALLXCENTRE;
                MatchStatus.BallY = BALLYCENTRE;

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
            Fixture.Played = true;

            FixtureAdapter fa = new FixtureAdapter();
            fa.UpdateFixture(Fixture);

            MatchCallback.MatchFinished(Fixture, Interactive);

        }

        private void RaiseEvent(MatchEventType Ev)
        {
            EventCountThisSecond++;

            if(!Interactive)
            {
                return; // If non-interactive, no point raising events or deciding on commentary for the UI
            }

            switch(Ev)
            {
                case MatchEventType.None:
                    MatchCallback.Commentary = "";
                    break;

                case MatchEventType.Goal:
                default:
                    MatchCallback.Commentary = FindCommentary(Ev);
                    break;
            }

            MatchCallback.MatchStatus = MatchStatus;
            MatchCallback.EventType = Ev;
            MatchCallback.UpdateUI();
        }

        private bool PossessionHome()
        {
            return MatchStatus.PossessionTeam == Constants.HomeTeam;
        }

        private bool PossessionAway()
        {
            return !PossessionHome();
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
            retVal.Add(new MatchEventCommentary(MatchEventType.CornerAnnounce, "That'll be a corner!", MatchSegment.None));
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

                MatchStatus.OverallPlayerEffectiveRating[t] = (Selected > 0 ? TotalEffectiveRating / Selected : 0);

                MatchStatus.PlayerStatuses[t] = StatusList;
                MatchStatus.Evaluation[t] = Eval;
                StatusList = null;
            }
        }

        public void DetermineNextEvent()
        {
            EventCountThisSecond = 0;

            double AttackAdrenaline = (MatchStatus.BallX > BALLXCENTRE ? MatchStatus.BallX : BALLXMAX - MatchStatus.BallX) * 0.4;
            bool PossessionChange = u.RandomInclusive(0, MatchStatus.Evaluation[1 - MatchStatus.PossessionTeam].Defence) 
                > u.RandomInclusive(0, Convert.ToInt32(MatchStatus.OverallPlayerEffectiveRating[MatchStatus.PossessionTeam] * AttackAdrenaline));

            if (PossessionHome() && PossessionChange == true)
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
            double ShotDistance = (PossessionHome() ?
                    BALLXMAX - MatchStatus.BallX :
                    Math.Abs(MatchStatus.BallX - BALLXMIN));

            double DistanceChance = u.RandomInclusive(0, 100 * Convert.ToInt32(ShotDistance));
            double OverallAttemptChance = ((MatchStatus.Evaluation[MatchStatus.PossessionTeam].Midfield / 2) +
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
                    if ((PossessionAway() && MatchStatus.BallX >= BALLXMAX - 1) || (PossessionHome() && MatchStatus.BallX <= BALLXMIN + 1))
                    {
                        bool hoofedAway = u.RandomInclusive(0, 100) < MatchStatus.Evaluation[MatchStatus.PossessionTeam].Defence / 4;
                        if (hoofedAway)
                        {
                            RaiseEvent(MatchEventType.Hoofed);

                            // TODO: Randomise this based on strength of players
                            int hoofDistance = u.RandomInclusive(1, 3);
                            MatchStatus.BallX += (PossessionHome() ? hoofDistance : 0 - hoofDistance);
                            if(u.RandomInclusive(0, 1) == 1)
                            {
                                MatchStatus.PossessionTeam = 1 - MatchStatus.PossessionTeam;
                                RaiseEvent(MatchEventType.OppositionGotThereFirst);
                            }
                        }
                    }
                }

                // TODO: Make the second part of this depend on passing
                double ballXDir = (PossessionHome() ? 1 : -1) * u.GaussianDistributedRandom(-1.5, 1.5);
                if(ballXDir != 0)
                {
                    if (SuccessfulEvent())
                    {
                        MatchStatus.BallX += ballXDir;

                        if (ballXDir < BALLXMIN)
                            MatchStatus.BallX = BALLXMIN;

                        if (ballXDir > BALLXMAX)
                            MatchStatus.BallX = BALLXMAX;
                    }
                }

                // TODO: Make this depend on passing
                double ballYDir = u.RandomInclusive(-1, 1) * u.GaussianDistributedRandom(-1.5, 1.5);
                if (SuccessfulEvent())
                {
                    MatchStatus.BallY += ballYDir;

                    if (ballYDir < BALLYMIN)
                        MatchStatus.BallY = BALLYMIN;

                    if (ballYDir > BALLYMAX)
                        MatchStatus.BallY = BALLYMAX;
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
            double For = 0;
            double Against = 0;

            int ForTeam = (MatchStatus.BallX < BALLXCENTRE ? Constants.AwayTeam : Constants.HomeTeam);
            int AgainstTeam = 1 - ForTeam;

            switch (Math.Floor(MatchStatus.BallX))
            {
                case 0:
                case 7:
                    For = (MatchStatus.Evaluation[ForTeam].Defence + MatchStatus.Evaluation[ForTeam].Goalkeeping) * 0.8;
                    Against = MatchStatus.Evaluation[AgainstTeam].Striker;
                    break;

                case 1:
                case 6:
                    For = MatchStatus.Evaluation[ForTeam].Defence;
                    Against = MatchStatus.Evaluation[AgainstTeam].Striker;
                    break;

                case 2:
                case 5:
                    For = (MatchStatus.Evaluation[ForTeam].Defence + MatchStatus.Evaluation[ForTeam].Midfield) * 0.6;
                    Against = MatchStatus.Evaluation[AgainstTeam].Attack;
                    break;

                case 3:
                case 4:
                    For = MatchStatus.Evaluation[ForTeam].Midfield;
                    Against = (MatchStatus.Evaluation[AgainstTeam].Midfield + MatchStatus.Evaluation[AgainstTeam].Attack) * 0.6;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();

            }

            // TODO: "x2" multiplier depends on player factors. Or maybe the above mulipliers do..... 
            return u.RandomInclusive(0, Convert.ToInt32(For)) >= u.RandomInclusive(0, Convert.ToInt32(Against) * 2 );
        }

        void DoGoalKick()
        {
            MatchStatus.PossessionTeam = 1 - MatchStatus.PossessionTeam;
            MatchStatus.BallX = (PossessionHome() ? BALLXMIN : BALLXMAX);
            MatchStatus.BallY = BALLYCENTRE;
            RaiseEvent(MatchEventType.GoalKick);
            

            // TODO: Take into account goalkeeper strength
            int strength = u.RandomInclusive(0, MatchStatus.Evaluation[MatchStatus.PossessionTeam].Goalkeeping);
            int ballDirection = PossessionHome() ? 1 : -1;

            //MatchStatus.BallX += (0 - ballDirection);

            if (strength <= 30)
            {
                MatchStatus.BallX += 2 * ballDirection; // Light kick
                MatchStatus.BallY = u.RandomInclusive(Convert.ToInt32(BALLYMIN + 1), Convert.ToInt32(BALLYMAX - 1));
            } else if(strength >= 31 && strength <= 66)
            {
                MatchStatus.BallX += 3 * ballDirection; // Medium kick
                MatchStatus.BallY = u.RandomInclusive(Convert.ToInt32(BALLYMIN + 1), Convert.ToInt32(BALLYMAX - 1));
            } else
            {
                MatchStatus.BallY = u.RandomInclusive(Convert.ToInt32(BALLYMIN), Convert.ToInt32(BALLYMAX));
                MatchStatus.BallX += 4 * ballDirection; // Hard kick
                RaiseEvent(MatchEventType.Hoofed);
            }

        }

        void DoCorner()
        {
            // TODO: Some logic in here to move the ball to the "Y" position of the correct corner
            MatchStatus.BallX = (PossessionHome() ? BALLXMIN : BALLXMAX);
            MatchStatus.BallY = (MatchStatus.BallY >= BALLYCENTRE ? BALLYMAX : BALLYMIN);
            RaiseEvent(MatchEventType.CornerStart);
            
            int ballXDir = (PossessionHome() ? 1 : -1);
            int ballYDir = (MatchStatus.BallY == BALLYMIN ? 1 : -1);
            MatchStatus.BallX += ballXDir;

            int MidChance = u.RandomInclusive(0, MatchStatus.Evaluation[MatchStatus.PossessionTeam].Midfield);
            int DefChance = u.RandomInclusive(0, MatchStatus.Evaluation[1 - MatchStatus.PossessionTeam].Defence);

            if(SuccessfulEvent())
            {
                ballYDir = ballYDir * u.RandomInclusive(1, 4);
                MatchStatus.BallY += ballYDir;
                RaiseEvent(MatchEventType.Cross); // TODO: This should be Corner Taken

                if (MidChance > DefChance)
                {
                    DoShot(MidChance, DefChance);
                }
                else
                {
                    MatchStatus.BallX += ballXDir * u.RandomInclusive(0, 2);
                    MatchStatus.PossessionTeam = 1 - MatchStatus.PossessionTeam;
                    RaiseEvent(MatchEventType.Dispossessed);
                }
            } else
            {

                ballYDir = ballYDir * u.RandomInclusive(1, 2);
                MatchStatus.BallY += ballYDir;
                MatchStatus.PossessionTeam = 1 - MatchStatus.PossessionTeam;
                RaiseEvent(MatchEventType.CornerOpposition);
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

            bool ShotOnTarget = u.RandomInclusive(0, AttChance) > AttChance * 0.66f; // TODO: Two-thirds of a chance of being on target

            if (ShotOnTarget)
            {
                bool ShotSuccess = AttChance > GKChance;
                

                if (ShotSuccess)
                {
                    MatchStatus.Score[MatchStatus.PossessionTeam]++;
                    MatchStatus.BallX = BALLXMAX * (1 - MatchStatus.PossessionTeam);
                    MatchStatus.BallY = BALLYCENTRE;
                    RaiseEvent(MatchEventType.Goal);

                    MatchStatus.PossessionTeam = 1 - MatchStatus.PossessionTeam;
                    MatchStatus.BallX = BALLXCENTRE;
                    MatchStatus.BallY = BALLYCENTRE;
                }
                else
                {
                    bool SafeHands = (u.RandomInclusive(0, MatchStatus.Evaluation[1 - MatchStatus.PossessionTeam].Goalkeeping) < GKChance);
                    if (SafeHands)
                    {
                        RaiseEvent(MatchEventType.Save);
                        DoGoalKick();
                    }
                    else
                    {
                        RaiseEvent(MatchEventType.BadSave); // TODO: Possible for someone to get it....
                        RaiseEvent(MatchEventType.CornerAnnounce);
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
