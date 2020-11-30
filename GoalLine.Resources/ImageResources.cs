using System;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GoalLine.Resources
{
    public static class ImageResourceList
    {
        public const string Background = "Background.jpg";
        public const string Logo = "Logo.png";
        public const string LogoSmall = "LogoSmall.png";
        public const string Pitch = "Pitch.jpg";
    }

    public static class ImageResources
    {
        private const string Namespace = "GoalLine.Resources";

        public static BitmapImage GetImage(string Name)
        { 
            string res = String.Format("{0}.Images.{1}", Namespace, Name);

            Assembly asm = Assembly.GetExecutingAssembly();
            StreamReader r = new StreamReader(asm.GetManifestResourceStream(res));

            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = r.BaseStream;
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();

            return img;
        }
    }
}
