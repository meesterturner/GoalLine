using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using GoalLine.Structures;
using GoalLine.Resources;

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
        private const string FILE_EMAILS = "emails.json";
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
                throw new Exception(LangResources.CurLang.SaveGameNotSpecified);
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
            SerialiseToDisk(World.Emails, FILE_EMAILS);


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
                throw new Exception(LangResources.CurLang.SaveGameNotSpecified);
            }

            Filename = Path.Combine(SavePath, FilenameWithExtension(SaveGameName));
            TempFolder = Path.Combine(SavePath, SaveGameName);

            if (Directory.Exists(TempFolder))
            {
                Directory.Delete(TempFolder, true);
            }

            ZipFile.ExtractToDirectory(Filename, TempFolder);

            World.WorldState = JsonSerializer.Deserialize<State>(SerialisedData(FILE_WORLDSTATE));
            World.Formations = JsonSerializer.Deserialize<List<Formation>>(SerialisedData(FILE_FORMATIONS));
            World.Players = JsonSerializer.Deserialize<List<Player>>(SerialisedData(FILE_PLAYERS));
            World.Teams = JsonSerializer.Deserialize<List<Team>>(SerialisedData(FILE_TEAMS));
            World.Managers = JsonSerializer.Deserialize<List<Manager>>(SerialisedData(FILE_MANAGERS));
            World.Leagues = JsonSerializer.Deserialize<List<League>>(SerialisedData(FILE_LEAGUES));
            World.Fixtures = JsonSerializer.Deserialize<List<Fixture>>(SerialisedData(FILE_FIXTURES));
            World.Emails = JsonSerializer.Deserialize<Dictionary<int, List<Email>>>(SerialisedData(FILE_EMAILS));

            Directory.Delete(TempFolder, true);

            retVal = true;

            return retVal;
        }

        private void SerialiseToDisk(Object o, string FileName)
        {
            string ObjectJson = JsonSerializer.Serialize(o);
            File.WriteAllText(Path.Combine(TempFolder, FileName), ObjectJson);
        }

        private string SerialisedData(string FileName)
        {
            string FullName = Path.Combine(TempFolder, FileName);

            if(!File.Exists(FullName))
            {
                throw new Exception(string.Format(LangResources.CurLang.SaveFileDoesNotExist, FileName));
            }

            return File.ReadAllText(FullName);
        }
    }
}
