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
                return new ScreenReturnData(ScreenReturnCode.None);
            }
            else
            {
                if (MyTeam)
                    return new ScreenReturnData(ScreenReturnCode.Next);
                else
                    return new ScreenReturnData(ScreenReturnCode.Ok);
            }
        }

        public ScreenReturnData MainButtonClick(int buttonId)
        {
            switch(buttonId)
            {
                case 2:
                    ctlFormation.SaveFormation();
                    return new ScreenReturnData(ScreenReturnCode.None);

                case 1:
                    UndoChanges(null);
                    return new ScreenReturnData(ScreenReturnCode.None);

                case 0:
                    return new ScreenReturnData(ScreenReturnCode.Cancel);

                default:
                    throw new NotImplementedException();
            }
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

            MyTeam = (wa.CurrentManagerID == SetupData.TeamData.ManagerID);

            if (MyTeam)
            {
                SetupData.MainButtons.Add(LangResources.CurLang.Save);
                SetupData.MainButtons.Add(LangResources.CurLang.UndoChanges);
            }
            
            SetupData.MainButtons.Add(LangResources.CurLang.Back);
            SetupData.ShowContinueButton = MyTeam;

            SetupData.Title1 = SetupData.TeamData.Name;
            SetupData.Title2 = SetupData.ManagerData.Name;

            ctlFormation.team = SetupData.TeamData;
        }
    }
}
