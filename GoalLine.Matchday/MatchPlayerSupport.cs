using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Structures;
using GoalLine.Data;
using GoalLine.Resources;

namespace GoalLine.Matchday
{
    /// <summary>
    /// Supporting calculations and peripheral functions for playing a match.
    /// </summary>
    class MatchPlayerSupport
    {
        bool Interactive;
        List<MatchEventCommentary> CommentaryList;

        public MatchPlayerSupport(bool Interactive)
        {
            this.Interactive = Interactive;
            if (Interactive)
            {
                CommentaryList = LangResources.CurLang.Commentaries;
            }
        }

        public string FindCommentary(MatchEventType Ev, MatchStatus ms)
        {
            string retVal = "";

            List<MatchEventCommentary> PossComment = (from C in CommentaryList
                                                      where C.EventType == Ev && (C.Segment == MatchSegment.None || C.Segment == ms.Segment)
                                                      select C).ToList();

            retVal = PossComment[0].RawText;
            return retVal;
        }

        Maths maths = new Maths();

        public MatchEventType RandomiseEvent(List<MatchEventChanceItem> events)
        {
            double totChance = 0;
            List<MatchEventChanceItem> aggregated = new List<MatchEventChanceItem>();
            int count = events.Count();

            for(int i = 0; i < count; i++)
            {
                totChance += events[i].Chance;
                aggregated.Add(new MatchEventChanceItem(events[i].evType, totChance));
            }

            if(totChance < 1.0f)
            {
                count++;
                totChance = 1.0;
                aggregated.Add(new MatchEventChanceItem(MatchEventType.None, totChance));
            }

            double rand = maths.GaussianDistributedRandom(0, totChance);

            for (int i = 0; i < count; i++)
            {
                if(aggregated[i].Chance <= rand)
                {
                    return aggregated[i].evType;
                }
            }

            throw new ArithmeticException();
        }

        public void PopulatePlayerStatuses(MatchStatus ms, Fixture f)
        {
            List<PlayerStatus> StatusList;

            for (int t = 0; t <= 1; t++)
            {
                ms.OverallPlayerEffectiveRating[t] = 0;

                TeamAdapter ta = new TeamAdapter();
                Team Team = ta.GetTeam(f.TeamIDs[t]);

                PlayerAdapter pa = new PlayerAdapter();
                StatusList = new List<PlayerStatus>();

                TacticEvaluation Eval = new TacticEvaluation();

                int selected = 0;
                int totalEffectiveRating = 0;
                const double HealthDeteriorationPower = 4;

                foreach (KeyValuePair<int, TeamPlayer> kvp in Team.Players)
                {
                    Player p = pa.GetPlayer(kvp.Value.PlayerID);

                    PlayerStatus ps = new PlayerStatus();
                    ps.PlayerID = kvp.Value.PlayerID;
                    ps.Playing = kvp.Value.Selected;
                    ps.CurrentHealth = p.Health;
                    ps.CurrentHealthDeterioration = Math.Pow(1 - (Convert.ToDouble(p.Fitness) / 100), HealthDeteriorationPower);

                    if (ps.Playing == PlayerSelectionStatus.Starting)
                    {
                        selected++;
                        
                        PlayerPosition pos = GridLengthToPosition(kvp.Value);
                        ps.EffectiveRating = CalculatePlayerEffectivenessInPosition(kvp.Value);
                        totalEffectiveRating += ps.EffectiveRating;
                        
                        Eval.AddRatingForPosition(pos, ps.EffectiveRating);
                        StatusList.Add(ps);
                    }

                    ps = null;
                }

                ms.OverallPlayerEffectiveRating[t] = (selected > 0 ? totalEffectiveRating / selected : 0);

                ms.PlayerStatuses[t] = StatusList;
                ms.Evaluation[t] = Eval;
                StatusList = null;
            }
        }

        public void UpdatePlayerHealthForMatch(MatchStatus ms)
        {
            for (int t = 0; t <= 1; t++)
            {
                for(int s = 0; s < ms.PlayerStatuses[t].Count; s++)
                {
                    PlayerStatus stat = ms.PlayerStatuses[t][s];

                    if (stat.Playing == PlayerSelectionStatus.Starting)
                    {
                        stat.CurrentHealth -= stat.CurrentHealthDeterioration;
                        if (stat.CurrentHealth < 0)
                            stat.CurrentHealth = 0;

                        ms.PlayerStatuses[t][s] = stat;
                    }
                }
            }
        }

        public void UpdatePlayerHealthForWorld(MatchStatus ms)
        {
            PlayerAdapter pa = new PlayerAdapter();

            for (int t = 0; t <= 1; t++)
            {
                for (int s = 0; s < ms.PlayerStatuses[t].Count; s++)
                {
                    PlayerStatus stat = ms.PlayerStatuses[t][s];
                    Player p = pa.GetPlayer(stat.PlayerID);
                    p.Health = stat.CurrentHealth;
                    pa.UpdatePlayer(p);
                }
            }
        }

        private PlayerPosition GridLengthToPosition(TeamPlayer tp)
        {
            switch(tp.PlayerGridY)
            {
                case 0:
                    return PlayerPosition.Goalkeeper;

                case 1:
                case 2:
                    return PlayerPosition.Defender;

                case 3:
                case 4:
                    return PlayerPosition.Midfielder;

                case 5:
                    return PlayerPosition.Attacker;

                case 6:
                case 7:
                    return PlayerPosition.Striker;
            }

            throw new ArgumentOutOfRangeException();
        }

        private int CalculatePlayerEffectivenessInPosition(TeamPlayer tp)
        {
            const double PlayerOutOfPositionMultiplier = 0.85;
            const double GKOutOfPositionMultiplier = 0.3;

            PlayerAdapter pa = new PlayerAdapter();
            Player p = pa.GetPlayer(tp.PlayerID);
            (int from, int to) range;

            int pitchLength = tp.PlayerGridY; // Flipped around due to axis changes.
            int pitchWidth = tp.PlayerGridX;

            double retVal = (double)p.OverallRating / 100; // Starting point - current rating, decimalised
            // TODO: Health percent needs to affect this as well. 
            switch (p.Position)
            {
                case PlayerPosition.Goalkeeper:
                    range = (0, 0);
                    break;

                case PlayerPosition.Defender:
                    range = (1, 2);
                    break;

                case PlayerPosition.Midfielder:
                    range = (2, 4);
                    break;

                case PlayerPosition.Attacker:
                    range = (4, 5);
                    break;

                case PlayerPosition.Striker:
                    range = (6, 6);
                    break;

                default:
                    throw new IndexOutOfRangeException();
            }

            if (pitchLength < range.from)
                retVal *= Math.Pow(PlayerOutOfPositionMultiplier, range.from - pitchLength);

            if (pitchLength > range.to)
                retVal *= Math.Pow(PlayerOutOfPositionMultiplier, pitchLength - range.to);

            switch (p.PreferredSide)
            {
                case PlayerPositionSide.Left:
                    range = (0, 1);
                    break;

                case PlayerPositionSide.Centre:
                    range = (1, 3);
                    break;

                case PlayerPositionSide.Right:
                    range = (3, 4);
                    break;

                default:
                    throw new IndexOutOfRangeException();

            }

            if (pitchWidth < range.from)
                retVal *= Math.Pow(PlayerOutOfPositionMultiplier, range.from - pitchWidth);

            if (pitchWidth > range.to)
                retVal *= Math.Pow(PlayerOutOfPositionMultiplier, pitchWidth - range.to);

            // If GK is out of position, or another player is in GK position, automatically set to 30%
            // on top of what may already have been worked out.
            if (pitchLength == 0 && pitchWidth == 2 && p.Position != PlayerPosition.Goalkeeper)
                retVal *= GKOutOfPositionMultiplier;

            if ((pitchLength != 0 || pitchWidth != 2) && p.Position == PlayerPosition.Goalkeeper)
                retVal *= GKOutOfPositionMultiplier;


            return Convert.ToInt32(retVal * 100);
        }
    }
}
