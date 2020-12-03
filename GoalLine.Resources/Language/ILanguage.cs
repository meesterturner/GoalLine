using System.Collections.Generic;
using GoalLine.Structures;

namespace GoalLine.Resources.Language
{
    public interface ILanguage
    {
        string _Name { get; }
        
        List<EmailTemplate> EmailTemplates { get; }


        string AreYouSureYouWantToQuit { get; }
        string AverageRating { get; }
        string BoardOfDirectors { get; }
        string Cancel { get; }
        string DailyUpdate { get; }
        string Date { get; }
        string DateFormat { get; }
        string DateOfBirth { get; }
        string Emails { get; }
        string Error { get; }
        string FirstName { get; }
        string Formation { get; }
        string FormationExists { get; }
        string FormationInvalid { get; }
        string FormationNotSaved { get; }
        string FormationSavedSuccessfully { get; }
        string From { get; }
        string GameHasNotBeenSavedBefore { get; }
        string GameSavedSuccessfully { get; }
        string Home { get; }
        string LastName { get; }
        string League { get; }
        string LeagueTableHeadings { get; }
        string LoadSavedGame { get; }
        string MatchDay { get; }
        string Name { get; }
        string Next { get; }
        string No { get; }
        string NoDataToShow { get; }
        string OK { get; }
        string Players { get; }
        string PleaseEnterDateOfBirth { get; }
        string PleaseEnterYourName { get; }
        string PleaseSelectATeamToManage { get; }
        string Position { get; }
        string Position_Short { get; }
        Dictionary<int, string> PositionsList { get; }
        string QuitGame { get; }
        string Rating { get; }
        string SavedGames { get; }
        string SaveFileDoesNotExist { get; }
        string SaveGame { get; }
        string SaveGameNotSpecified { get; }
        Dictionary<int, string> SidesList { get; }
        string Start { get; }
        string StartNewGame { get; }
        string Subject { get;  }
        string Team { get; }
        string Unread { get; }
        string WelcomeToGoalLine { get; }
        string Yes { get; }
        string YouHaveAMatchAgainst { get; }
        string YouHaveNotSelectedAGame { get; }
        string YouHaveNotSelectedEnoughPlayers { get; }
    }
}
