using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Resources.Language
{
    class English : ILanguage
    {
        public string _Name => "English";


        public string AreYouSureYouWantToQuit => "Are you sure you want to quit, and lose any unsaved progress?";
        public string AverageRating => "Average Rating";
        public string Cancel => "Cancel";
        public string Date => "Date";
        public string DateOfBirth => "Date of Birth";
        public string FirstName => "First Name";
        public string Formation => "Formation";
        public string FormationExists => "Formation with same name exists.";
        public string FormationInvalid => "Formation must have exactly 11 points";
        public string FormationSavedSuccessfully => "Formation has been saved successfully.";
        public string GameHasNotBeenSavedBefore => "This game has not been saved before, please give it a name.";
        public string GameSavedSuccessfully => "Game saved successfully as \"{0}\"";
        public string Home => "Home";
        public string LastName => "Last Name";
        public string League => "League";
        public string LeagueTableHeadings => "Pos," + Team + ",P,W,D,L,F,A,GD,Pts";
        public string LoadSavedGame => "Load Saved Game";
        public string MatchDay => "Match Day";
        public string Name => "Name";
        public string Next => "Next";
        public string No => "No";
        public string NoDataToShow => "There is no data to show in this list";
        public string OK => "OK";
        public string PleaseEnterDateOfBirth => "Please enter a valid date of birth.";
        public string PleaseEnterYourName => "Please enter your name.";
        public string PleaseSelectATeamToManage => "Please select a team to manage.";
        public string QuitGame => "Quit Game";
        public string SavedGames => "Saved Games";
        public string SaveGame => "Save Game";
        public string Start => "Start";
        public string StartNewGame => "Start New Game";
        public string Team => "Team";
        public string WelcomeToGoalLine => "Welcome To GoalLine";
        public string Yes => "Yes";
        public string YouHaveNotSelectedAGame => "You have not selected a game to load.";
        

    }
}
