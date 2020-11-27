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
        private const string FILE_WORLDSTATE = "worldstate.json";
        private const string FILE_FORMATIONS = "formations.json";
        private const string FILE_PLAYERS = "players.json";
        private const string FILE_TEAMS = "teams.json";
        private const string FILE_MANAGERS = "managers.json";
        private const string FILE_LEAGUES = "leagues.json";
        private const string FILE_FIXTURES = "fixtures.json";

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

            SerialiseToDisk(World.WorldState, FILE_WORLDSTATE);
            SerialiseToDisk(World.Formations, FILE_FORMATIONS);
            SerialiseToDisk(World.Players, FILE_PLAYERS);
            SerialiseToDisk(World.Teams, FILE_TEAMS);
            SerialiseToDisk(World.Managers, FILE_MANAGERS);
            SerialiseToDisk(World.Leagues, FILE_LEAGUES);
            SerialiseToDisk(World.Fixtures, FILE_FIXTURES);


            ZipFile.CreateFromDirectory(TempFolder, Filename);

            Directory.Delete(TempFolder, true);

            retVal = true;

            return retVal;
        }

        public List<SaveGameInfo> ListSaveGames()
        {
            List<SaveGameInfo> retVal = new List<SaveGameInfo>();

            if(Directory.Exists(SavePath))
            {
                foreach(string Filename in Directory.GetFiles(SavePath, "*" + FileExtension))
                {
                    SaveGameInfo sg = new SaveGameInfo();
                    sg.Name = Path.GetFileNameWithoutExtension(Filename);
                    sg.SaveDate = File.GetLastWriteTime(Filename);

                    retVal.Add(sg);
                }
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

            World.WorldState = JsonConvert.DeserializeObject<State>(SerialisedData(FILE_WORLDSTATE));
            World.Formations = JsonConvert.DeserializeObject<List<Formation>>(SerialisedData(FILE_FORMATIONS));
            World.Players = JsonConvert.DeserializeObject<List<Player>>(SerialisedData(FILE_PLAYERS));
            World.Teams = JsonConvert.DeserializeObject<List<Team>>(SerialisedData(FILE_TEAMS));
            World.Managers = JsonConvert.DeserializeObject<List<Manager>>(SerialisedData(FILE_MANAGERS));
            World.Leagues = JsonConvert.DeserializeObject<List<League>>(SerialisedData(FILE_LEAGUES));
            World.Fixtures = JsonConvert.DeserializeObject<List<Fixture>>(SerialisedData(FILE_FIXTURES));

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
