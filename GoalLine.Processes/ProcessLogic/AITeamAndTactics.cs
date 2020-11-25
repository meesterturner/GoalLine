using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Data;
using GoalLine.Structures;

namespace GoalLine.Processes.ProcessLogic
{
    class AITeamAndTactics
    {
        /// <summary>
        /// Loop round all AI teams, and select a team appropriate to play the fixture.
        /// </summary>
        public void SelectTeamIfPlaying()
        {
            FixtureAdapter fa = new FixtureAdapter();
            WorldAdapter wa = new WorldAdapter();
            TeamAdapter ta = new TeamAdapter();
            ManagerAdapter ma = new ManagerAdapter();

            List<Fixture> AllFixtures = fa.GetFixtures(wa.CurrentDate);

            int TESTf = 0;

            foreach (Fixture f in AllFixtures)
            {
                TESTf++;
                Debug.Print("Fixture " + f.ToString());

                for (int t = 0; t <= 1; t++)
                {
                    Team ThisTeam = ta.GetTeam(f.TeamIDs[t]);
                    Manager M = ma.GetManager(ThisTeam.ManagerID);


                    if (!M.Human)
                    {
                        Team Opposition = ta.GetTeam(f.TeamIDs[1 - t]);
                        PlayerAdapter pa = new PlayerAdapter();

                        // Reset selection
                        foreach (KeyValuePair<int, TeamPlayer> kvp in ThisTeam.Players)
                        {
                            ta.SetTeamPlayerSelection(ThisTeam.UniqueID, kvp.Value.PlayerID, PlayerSelectionStatus.None);
                        }

                        // TODO: This logic is absolutely rubbish, but it will do for testing, hopefully!
                        int CountGK = 0;
                        int CountDef = 0;
                        int CountDefC = 0;
                        int CountMid = 0;
                        int CountMidC = 0;
                        int CountAtt = 0;
                        int CountAttC = 0;
                        int CountStriker = 0;
                        int CountSubs = 0;
                        int CountStarting = 0;

                        // Basic selection
                        foreach (KeyValuePair<int, TeamPlayer> kvp in ThisTeam.Players)
                        {
                            Player p = pa.GetPlayer(kvp.Value.PlayerID);

                            if (CountStarting == 11)
                            {
                                if (CountSubs < 4)
                                {
                                    ta.SetTeamPlayerSelection(ThisTeam.UniqueID, kvp.Value.PlayerID, PlayerSelectionStatus.Sub);
                                    CountSubs++;
                                }
                                else
                                {
                                    break; // Finish picking
                                }
                            }
                            else
                            {
                                switch (p.Position)
                                {
                                    case PlayerPosition.Goalkeeper:


                                        if (CountGK == 0)
                                        {
                                            ta.SetTeamPlayerSelection(ThisTeam.UniqueID, kvp.Value.PlayerID, PlayerSelectionStatus.Starting);
                                            CountStarting++;
                                        }
                                        else if (CountGK == 1)
                                        {
                                            ta.SetTeamPlayerSelection(ThisTeam.UniqueID, kvp.Value.PlayerID, PlayerSelectionStatus.Sub);
                                            CountSubs++;
                                        }
                                        CountGK++;
                                        break;

                                    case PlayerPosition.Defender:
                                        if (CountDef < 4)
                                        {
                                            if (p.PreferredSide == PlayerPositionSide.Left || p.PreferredSide == PlayerPositionSide.Right)
                                            {
                                                ta.SetTeamPlayerSelection(ThisTeam.UniqueID, kvp.Value.PlayerID, PlayerSelectionStatus.Starting);
                                                CountDef++;
                                                CountStarting++;
                                            }
                                            else if (p.PreferredSide == PlayerPositionSide.Centre && CountDefC < 2)
                                            {
                                                ta.SetTeamPlayerSelection(ThisTeam.UniqueID, kvp.Value.PlayerID, PlayerSelectionStatus.Starting);
                                                CountDef++;
                                                CountDefC++;
                                                CountStarting++;
                                            }
                                        }
                                        else if (CountSubs <= 2)
                                        {
                                            ta.SetTeamPlayerSelection(ThisTeam.UniqueID, kvp.Value.PlayerID, PlayerSelectionStatus.Sub);
                                            CountSubs++;
                                        }
                                        break;

                                    case PlayerPosition.Midfielder:
                                        if (CountMid < 4)
                                        {
                                            if (p.PreferredSide == PlayerPositionSide.Left || p.PreferredSide == PlayerPositionSide.Right)
                                            {
                                                ta.SetTeamPlayerSelection(ThisTeam.UniqueID, kvp.Value.PlayerID, PlayerSelectionStatus.Starting);
                                                CountMid++;
                                                CountStarting++;
                                            }
                                            else if (p.PreferredSide == PlayerPositionSide.Centre && CountMidC < 2)
                                            {
                                                ta.SetTeamPlayerSelection(ThisTeam.UniqueID, kvp.Value.PlayerID, PlayerSelectionStatus.Starting);
                                                CountMid++;
                                                CountMidC++;
                                                CountStarting++;
                                            }
                                        }
                                        else if (CountSubs <= 2)
                                        {
                                            ta.SetTeamPlayerSelection(ThisTeam.UniqueID, kvp.Value.PlayerID, PlayerSelectionStatus.Sub);
                                            CountSubs++;
                                        }
                                        break;


                                    case PlayerPosition.Attacker:
                                        if (CountAtt < 2)
                                        {
                                            if (p.PreferredSide == PlayerPositionSide.Left || p.PreferredSide == PlayerPositionSide.Right)
                                            {
                                                ta.SetTeamPlayerSelection(ThisTeam.UniqueID, kvp.Value.PlayerID, PlayerSelectionStatus.Starting);
                                                CountAtt++;
                                                CountStarting++;
                                            }
                                            else if (p.PreferredSide == PlayerPositionSide.Centre && CountAttC <= 2)
                                            {
                                                ta.SetTeamPlayerSelection(ThisTeam.UniqueID, kvp.Value.PlayerID, PlayerSelectionStatus.Starting);
                                                CountAtt++;
                                                CountAttC++;
                                                CountStarting++;
                                            }
                                        }
                                        else if (CountSubs < 3)
                                        {
                                            ta.SetTeamPlayerSelection(ThisTeam.UniqueID, kvp.Value.PlayerID, PlayerSelectionStatus.Sub);
                                            CountSubs++;
                                        }
                                        break;

                                    case PlayerPosition.Striker:
                                        if (CountStriker == 0)
                                        {
                                            ta.SetTeamPlayerSelection(ThisTeam.UniqueID, kvp.Value.PlayerID, PlayerSelectionStatus.Starting);
                                            CountStriker++;
                                            CountStarting++;
                                        }
                                        else if (CountSubs < 4)
                                        {
                                            ta.SetTeamPlayerSelection(ThisTeam.UniqueID, kvp.Value.PlayerID, PlayerSelectionStatus.Sub);
                                            CountSubs++;
                                        }
                                        break;
                                }
                            }


                        }


                    }
                }
            }
        }
    }
}
