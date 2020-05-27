using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;
using GoalLine.Structures;

namespace GoalLine.Data
{
    public class GameIO
    {
        private const string FileExtension = ".zip";

        public string SaveGameName { get; set; }
        
        private string Filename;
        private string TempFolder;

        private string SavePath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GoalLine");
            }
        }

        private string FilenameWithExtension(string Filename)
        {
            if (!Filename.ToLower().Trim().EndsWith(FileExtension))
            {
                return Filename.Trim() + FileExtension;
            } else
            {
                return Filename.Trim();
            }
        }

        public bool SaveGame()
        {
            bool retVal = false;

            if(SaveGameName == "")
            {
                throw new Exception("SaveGameName not specified");
            }

            Filename = Path.Combine(SavePath, FilenameWithExtension(SaveGameName));
            TempFolder = Path.Combine(SavePath, SaveGameName);

            if(Directory.Exists(TempFolder))
            {
                Directory.Delete(TempFolder, true);
            }

            if(File.Exists(Filename))
            {
                File.Delete(Filename);
            }

            Directory.CreateDirectory(TempFolder);

            SerialiseToDisk(World.WorldState, "worldstate.json");
            SerialiseToDisk(World.Players, "players.json");
            SerialiseToDisk(World.Teams, "teams.json");
            SerialiseToDisk(World.Managers, "managers.json");
            SerialiseToDisk(World.Leagues, "leagues.json");
            SerialiseToDisk(World.Fixtures, "fixtures.json");


            ZipFile.CreateFromDirectory(TempFolder, Filename);

            Directory.Delete(TempFolder, true);

            retVal = true;

            return retVal;
        }

        public List<SaveGameInfo> ListSaveGames()
        {
            List<SaveGameInfo> retVal = new List<SaveGameInfo>();

            foreach(string Filename in Directory.GetFiles(SavePath, "*" + FileExtension))
            {
                SaveGameInfo sg = new SaveGameInfo();
                sg.Name = Path.GetFileNameWithoutExtension(Filename);
                sg.SaveDate = File.GetLastWriteTime(Filename);

                retVal.Add(sg);
            }

            return retVal;
        }

        public bool LoadGame()
        {
            bool retVal = false;

            if (SaveGameName == "")
            {
                throw new Exception("SaveGameName not specified");
            }

            Filename = Path.Combine(SavePath, FilenameWithExtension(SaveGameName));
            TempFolder = Path.Combine(SavePath, SaveGameName);

            if (Directory.Exists(TempFolder))
            {
                Directory.Delete(TempFolder, true);
            }

            ZipFile.ExtractToDirectory(Filename, TempFolder);

            World.WorldState = JsonConvert.DeserializeObject<State>(SerialisedData("worldstate.json"));
            World.Players = JsonConvert.DeserializeObject<List<Player>>(SerialisedData("players.json"));
            World.Teams = JsonConvert.DeserializeObject<List<Team>>(SerialisedData("teams.json"));
            World.Managers = JsonConvert.DeserializeObject<List<Manager>>(SerialisedData("managers.json"));
            World.Leagues = JsonConvert.DeserializeObject<List<League>>(SerialisedData("leagues.json"));
            World.Fixtures = JsonConvert.DeserializeObject<List<Fixture>>(SerialisedData("fixtures.json"));

            Directory.Delete(TempFolder, true);

            retVal = true;

            return retVal;
        }

        private void SerialiseToDisk(Object o, string FileName)
        {
            string ObjectJson = JsonConvert.SerializeObject(o);
            File.WriteAllText(Path.Combine(TempFolder, FileName), ObjectJson);
        }

        private string SerialisedData(string FileName)
        {
            return File.ReadAllText(Path.Combine(TempFolder, FileName));
        }
    }
}
