﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalLine.Data;
using GoalLine.Structures;

namespace GoalLine.Processes
{
    public static class ProcessManager
    {
        private static List<IProcess> ProcessHandlers = new List<IProcess>();
        private static bool RegisteredStandardHandlers = false;

        public static void RegisterProcessHandler(IProcess ProcessorClass) // Call e.g.: ProcessManager.RegisterProcessHandler(new SomeProcessClass());
        {
            ProcessHandlers.Add(ProcessorClass);
        }

        private static void RegisterAllStandardHandlers()
        {
            RegisterProcessHandler(new SeasonProcesses());
            RegisterProcessHandler(new AIProcesses());
            RegisterProcessHandler(new DirectorProcesses());
            RegisterProcessHandler(new MedicalProcesses());

            RegisteredStandardHandlers = true;
        }

        public static void RunStartOfDay(bool SaveGameJustLoaded)
        {
            if(!RegisteredStandardHandlers)
            {
                RegisterAllStandardHandlers();
            }

            if(SaveGameJustLoaded)
            {
                return;
            }

            WorldAdapter wa = new WorldAdapter();

            foreach (IProcess Handler in ProcessHandlers)
            {
                
                for(int i = 1; i <= 4; i++)
                {
                    try
                    {
                        switch(i)
                        {
                            case 1:
                                Handler.StartOfDay();
                                break;

                            case 2:
                                if (wa.CurrentDate == wa.PreSeasonDate)
                                {
                                    Handler.PreSeasonStart();
                                }
                                break;

                            case 3:
                                if (wa.CurrentDate == wa.MainSeasonDate)
                                {
                                    Handler.SeasonStart();
                                }
                                break;

                            case 4:

                                if(new FixtureAdapter().IsTodayAMatchDay())
                                {
                                    Handler.MatchDayStart();
                                }
                                break;
                        }
           
                    }
#pragma warning disable CS0168
                    catch (NotImplementedException ex)
#pragma warning restore CS0168
                    {
                        // Silently fail on a NotImplementedException ONLY, because not all classes will implement something
                    }

                }
                
                
            }

        }

        public static void RunEndOfDay()
        {
            if (!RegisteredStandardHandlers)
            {
                RegisterAllStandardHandlers();
            }

            WorldAdapter wa = new WorldAdapter();

            foreach (IProcess Handler in ProcessHandlers)
            {
                for (int i = 1; i <= 4; i++)
                {
                    try
                    {
                        switch (i)
                        {
                            case 1:
                                if (new FixtureAdapter().IsTodayAMatchDay())
                                {
                                    Handler.MatchDayEnd();
                                }
                                break;

                            case 2:
                                Handler.SeasonEnd();
                                break;

                            case 3:
                                if (wa.CurrentDate == wa.MainSeasonDate.AddDays(-1))
                                {
                                    Handler.PreSeasonEnd();
                                }
                                break;

                            case 4:
                                Handler.EndOfDay();
                                break;
                        }

                    }
#pragma warning disable CS0168
                    catch (NotImplementedException ex)
#pragma warning restore CS0168
                    {
                        // Silently fail on a NotImplementedException ONLY, because not all classes will implement something
                    }

                }
            }

            // Go to next day, whoop!
            wa.AdvanceDate();
        }
    }
}
