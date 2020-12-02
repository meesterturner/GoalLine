using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.UI.GameScreens
{
    public interface IGameScreen
    {
        GameScreenSetup SetupData { get; set; }
        ScreenReturnData MainButtonClick(int buttonId);
        void SetupGameScreenData(GameScreenSetup dataFromUI);
        ScreenReturnData ContinueButtonClick();
    }
}
