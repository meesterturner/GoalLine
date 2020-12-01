using System;
using System.Collections.Generic;
using System.Reflection;
using GoalLine.Resources.Language;

namespace GoalLine.Resources
{
    public static class LangResources
    {
        public static ILanguage CurLang { get; private set; } = new English();

        public static List<string> AllLanguages()
        {
            List<string> retVal = new List<string>();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (typeof(ILanguage).IsAssignableFrom(type))
                {
                    try
                    {
                        ILanguage result = Activator.CreateInstance(type) as ILanguage;
                        if (result != null)
                        {
                            retVal.Add(result._Name);
                        }
                    }
                    catch (Exception)
                    {
                        // Nothing
                    }

                }
            }

            return retVal;
        }

        public static void SetLanguage(string Name)
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (typeof(ILanguage).IsAssignableFrom(type))
                {
                    try
                    {
                        ILanguage result = Activator.CreateInstance(type) as ILanguage;
                        if (result != null)
                        {
                            if (result._Name.ToUpper() == Name.ToUpper())
                            {
                                CurLang = result;
                                return;
                            };
                        }
                    }
                    catch (Exception)
                    {
                        // Nothing
                    }

                }
            }

            throw new ArgumentOutOfRangeException("Unknown Locale Name chosen");
        }
    }
}
