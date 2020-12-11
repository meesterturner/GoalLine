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

            new EmailTemplate(10100, EmailType.GoodMatch, "An excellent result!", "We are especially pleased about the {0}-{1} win against {2}.\n\nWe hope you can keep this performance up."),

            new EmailTemplate(10150, EmailType.BadMatch, "Disappointing match", "We are disappointed to note the team's performance in the {0}-{1} defeat against {2}.\n\nWe are hopeful that this is only a one-off and won't be repeated.")
        };

        public List<MatchEventCommentary> Commentaries { get; private set; } = new List<MatchEventCommentary>()
        {
            new MatchEventCommentary(MatchEventType.Foul, "His legs were hacked off right in front of the crowd", MatchSegment.None),
            new MatchEventCommentary(MatchEventType.FreeKick, "He goes to take the free kick", MatchSegment.None),
            new MatchEventCommentary(MatchEventType.FullTime, "And that's the end of the match!", MatchSegment.None),
            new MatchEventCommentary(MatchEventType.Goal, "The keeper had no chance with that one!", MatchSegment.None),
            new MatchEventCommentary(MatchEventType.GoalKick, "The keeper boots it down the pitch", MatchSegment.None),
            new MatchEventCommentary(MatchEventType.HalfTime, "The end of a thrilling first half", MatchSegment.None),
            new MatchEventCommentary(MatchEventType.KickOff, "This hotly anticipated game begins!", MatchSegment.FirstHalf),
            new MatchEventCommentary(MatchEventType.KickOff, "The second half gets under way!", MatchSegment.SecondHalf),
            new MatchEventCommentary(MatchEventType.Miss, "Oooh, surely he will be kicking himself for that one!", MatchSegment.None),
            new MatchEventCommentary(MatchEventType.Save, "Cracking reactions by the keeper there", MatchSegment.None),
            new MatchEventCommentary(MatchEventType.BadSave, "He's palmed it away with his fingers", MatchSegment.None),
            new MatchEventCommentary(MatchEventType.Dispossessed, "The ball is intercepted by the opposition", MatchSegment.None),
            new MatchEventCommentary(MatchEventType.Hoofed, "He hoofs it right up the pitch", MatchSegment.None),
            new MatchEventCommentary(MatchEventType.Shot, "He's hit it towards the goal...", MatchSegment.None),
            new MatchEventCommentary(MatchEventType.HitCrossbar, "The ball ricochets off the crossbar", MatchSegment.None),
            new MatchEventCommentary(MatchEventType.HitPost, "Oh, he's booted it straight into the post and it shears off!", MatchSegment.None),
            new MatchEventCommentary(MatchEventType.Clearance, "The ball is cleared from danger", MatchSegment.None),
            new MatchEventCommentary(MatchEventType.CornerStart, "He runs up to the corner", MatchSegment.None),
            new MatchEventCommentary(MatchEventType.CornerAnnounce, "That'll be a corner!", MatchSegment.None),
            new MatchEventCommentary(MatchEventType.Cross, "The ball is crossed in", MatchSegment.None),
            new MatchEventCommentary(MatchEventType.CornerOpposition, "The ball is safely taken by the opposition", MatchSegment.None),
            new MatchEventCommentary(MatchEventType.OppositionGotThereFirst, "Their opponents got to the ball before they could", MatchSegment.None)
        };

        public string Agility => "Agility";
        public string Attitude => "Attitude";
        public string AreYouSureYouWantToQuit => "Are you sure you want to quit, and lose any unsaved progress?";
        public string AreYouSureYouWantToUndo => "Are you sure you want to undo all unsaved changes?";
        public string AverageRating => "Average Rating";
        public string Away => "Away";
        public string Back => "Back";
        public string Balance => "Balance";
        public string BoardOfDirectors => "Board of Directors";
        public string Cancel => "Cancel";
        public string DailyUpdate => "Running Daily Processes...";
        public string Date => "Date";
        public string DateFormat => "dd/MM/yyyy";
        public string DateOfBirth => "Date of Birth";
        public string Emails => "Emails";
        public string Error => "Error";
        public string FirstName => "First Name";
        public string Fitness => "Fitness";
        public string Formation => "Formation";
        public string FormationExists => "Formation with same name exists.";
        public string FormationInvalid => "Formation must have exactly 11 points";
        public string FormationNotSaved => "You have made changes to your formation that you have not saved.";
        public string From => "From";
        public string FormationSavedSuccessfully => "Formation has been saved successfully.";
        public string GameHasNotBeenSavedBefore => "This game has not been saved before, please give it a name.";
        public string GameSavedSuccessfully => "Game saved successfully as \"{0}\"";
        public string Handling => "Handling";
        public string Heading => "Heading";
        public string Health => "Health";
        public string Home => "Home";
        public string Influence => "Influence";
        public string LastName => "Last Name";
        public string League => "League";
        public string LeagueTableHeadings => "Pos," + Team + ",P,W,D,L,F,A,GD,Pts";
        public string LoadSavedGame => "Load Saved Game";
        public string Marking => "Marking";
        public string MatchDay => "Match Day";
        public string Name => "Name";
        public string Next => "Next";
        public string No => "No";
        public string NoDataToShow => "There is no data to show in this list";
        public string OK => "OK";
        public string Passing => "Passing";
        public string Players => "Players";
        public string PleaseEnterDateOfBirth => "Please enter a valid date of birth.";
        public string PleaseEnterYourName => "Please enter your name.";
        public string Position => "Position";
        public string Position_Short => "Pos";
        public Dictionary<int, string> PositionsList => new Dictionary<int, string>()
        {
            {0, "None Specified" },
            {1, "Goalkeeper" },
            {2, "Defender" },
            {3, "Midfielder" },
            {4, "Attacker" },
            {5, "Striker" }
        };
        public string PleaseSelectATeamToManage => "Please select a team to manage.";
        public string QuitGame => "Quit Game";
        public string Rating => "Rating";
        public string Save => "Save";
        public string SavedGames => "Saved Games";
        public string SaveFileDoesNotExist => "File {0} does not exist in this save game.";
        public string SaveGame => "Save Game";
        public string SaveGameNotSpecified => "SaveGameName not specified";
        public string Shooting => "Shooting";
        public Dictionary<int, string> SidesList => new Dictionary<int, string>()
        {
            {-10, "Left" },
            {0, "Centre" },
            {10, "Right" }
        };

        public string Speed => "Speed";
        public string Stamina => "Stamina";
        public string Start => "Start";
        public string StartNewGame => "Start New Game";
        public string Strength => "Strength";
        public string Subject => "Subject";
        public string Tackling => "Tackling";
        public string Tactics => "Tactics";
        public string Team => "Team";
        public string UndoChanges => "Undo Changes";
        public string Unread => "Unr";
        public string WelcomeToGoalLine => "Welcome To GoalLine";
        public string Yes => "Yes";
        public string YouHaveAMatchAgainst => "You have a match today against {0}.";
        public string YouHaveNotSelectedAGame => "You have not selected a game to load.";
        public string YouHaveNotSelectedEnoughPlayers => "You cannot continue to the match as you have selected {0} players out of {1}";
    }
}
