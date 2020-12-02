using System.Collections.Generic;
using GoalLine.Structures;

namespace GoalLine.Resources.Language
{
    class English : ILanguage
    {
        public string _Name => "English";

        public List<EmailTemplate> EmailTemplates { get; private set; } = new List<EmailTemplate>()
        {
            new EmailTemplate(10000, EmailType.Welcome, "Welcome to the Club", "The board warmly welcomes {0} to {1}.\n\nWe hope for a long and prosperous future under your management."),
            new EmailTemplate(10001, EmailType.Welcome, "Your appointment to the Club", "Congratulations to {0} on your appointment to {1}.\n\nWe look forward to seeing your performance this coming season."),

            new EmailTemplate(10100, EmailType.GoodMatch, "An excellent result!", "We are especially pleased about the {0}:{1} win against {2}.\n\nWe hope you can keep this performance up."),

            new EmailTemplate(10150, EmailType.BadMatch, "Disappointing match", "We are disappointed to note the team's performance in the {0}:{1} defeat against {2}.\n\nWe are hopeful that this is only a one-off and won't be repeated.")
        };

        public string AreYouSureYouWantToQuit => "Are you sure you want to quit, and lose any unsaved progress?";
        public string AverageRating => "Average Rating";
        public string BoardOfDirectors => "Board of Directors";
        public string Cancel => "Cancel";
        public string Date => "Date";
        public string DateFormat => "dd/MM/yyyy";
        public string DateOfBirth => "Date of Birth";
        public string FirstName => "First Name";
        public string Formation => "Formation";
        public string FormationExists => "Formation with same name exists.";
        public string FormationInvalid => "Formation must have exactly 11 points";
        public string From => "From";
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
        public string Subject => "Subject";
        public string Team => "Team";
        public string Unread => "Unr";
        public string WelcomeToGoalLine => "Welcome To GoalLine";
        public string Yes => "Yes";
        public string YouHaveAMatchAgainst => "You have a match today against {0}.";
        public string YouHaveNotSelectedAGame => "You have not selected a game to load.";
    }
}
