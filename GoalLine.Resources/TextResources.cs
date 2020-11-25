using System;
using System.IO;
using System.Reflection;

namespace GoalLine.Resources
{

    public static class TextResources
    {
        private const string Namespace = "GoalLine.Resources";
        
        public static string[] NameList(NameListResource NameList, ResourceLanguage Lang)
        {
            string res = String.Format("{0}.NameLists.{1}_{2}.txt", Namespace, NameList.ToString(), Lang.ToString());
            return TextResourceToStringArray(res);
        }

        public static string[] TeamList(TeamListResource TeamList, ResourceLanguage Lang)
        {
            
            string res = String.Format("{0}.TeamLists.{1}_{2}.txt", Namespace, TeamList.ToString(), Lang.ToString());
            return TextResourceToStringArray(res);
        }

        private static string[] TextResourceToStringArray(string res)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            StreamReader r = new StreamReader(asm.GetManifestResourceStream(res));
            string WholeFile = r.ReadToEnd();

            // TODO: There's got to be a better way to deal with the line encodings.... This will do for now.
            if (!WholeFile.Contains(Convert.ToChar(13).ToString()))
            {
                WholeFile = WholeFile.Replace(Convert.ToChar(10).ToString(), Convert.ToChar(13).ToString());
            } else
            {
                WholeFile = WholeFile.Replace(Convert.ToChar(10).ToString(), "");
            }

            return WholeFile.Split(Convert.ToChar(13));
        }
    }
}
