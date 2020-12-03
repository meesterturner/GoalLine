using System;
using System.Windows.Controls;
using GoalLine.Data;
using GoalLine.Structures;
using GoalLine.Resources;
using GoalLine.UI.Utils;
using System.Collections.Generic;

namespace GoalLine.UI.GameScreens
{
    /// <summary>
    /// Interaction logic for TacticsScreen.xaml
    /// </summary>
    public partial class TacticsScreen : UserControl, IGameScreen
    {
        private bool MyTeam = false;
        public TacticsScreen()
        {
            InitializeComponent();
        }

        public GameScreenSetup SetupData { get; set; }

        public ScreenReturnData ContinueButtonClick()
        {
            if (ctlFormation.ChangesNotSaved)
            {
                UiUtils.OpenDialogBox(UiUtils.MainWindowGrid, LangResources.CurLang.Formation, LangResources.CurLang.FormationNotSaved, new List<DialogButton>() { new DialogButton(LangResources.CurLang.OK, null, null) });
                return new ScreenReturnData(ScreenReturnCode.Cancel);
            }
            else
            {
                return new ScreenReturnData(ScreenReturnCode.Ok);
            }
        }

        public ScreenReturnData MainButtonClick(int buttonId)
        {
            switch(buttonId)
            {
                case 1:
                    ctlFormation.SaveFormation();
                    break;

                case 0:
                    UndoChanges(null);
                    break;

                default:
                    throw new NotImplementedException();
            }

            return new ScreenReturnData(ScreenReturnCode.None);
        }

        private void UndoChanges(object Data)
        {
            if(Data == null)
            {
                UiUtils.OpenDialogBox(UiUtils.MainWindowGrid, LangResources.CurLang.Tactics, LangResources.CurLang.AreYouSureYouWantToUndo, 
                    new List<DialogButton>() { 
                        new DialogButton(LangResources.CurLang.Yes, UndoChanges, true),
                        new DialogButton(LangResources.CurLang.No, null, null)
                    });

            }
            else
            {
                ctlFormation.SetupTeam(false);
            }
        }

        public void SetupGameScreenData(GameScreenSetup dataFromUI)
        {
            if (dataFromUI.TeamData == null)
            {
                throw new Exception("TeamData is null");
            }

            ManagerAdapter ma = new ManagerAdapter();
            WorldAdapter wa = new WorldAdapter();

            SetupData = dataFromUI;
            SetupData.ManagerData = ma.GetManager(SetupData.TeamData.ManagerID);
            SetupData.MainButtons.Add(LangResources.CurLang.Save);
            SetupData.MainButtons.Add(LangResources.CurLang.UndoChanges);

            MyTeam = (wa.CurrentManagerID == SetupData.TeamData.ManagerID);

            SetupData.ShowContinueButton = true;

            SetupData.Title1 = SetupData.TeamData.Name;
            SetupData.Title2 = SetupData.ManagerData.Name;

            ctlFormation.team = SetupData.TeamData;
        }
    }
}
