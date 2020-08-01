using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.UI.GameScreens
{
    public class ScreenReturnData
    {
        public ScreenReturnCode ReturnCode { get; set; }
        public string Message { get; set; }

        public ScreenReturnData()
        {

        }

        public ScreenReturnData(ScreenReturnCode ReturnCode)
        {
            this.ReturnCode = ReturnCode;
        }

        public ScreenReturnData(ScreenReturnCode ReturnCode, string Message)
        {
            this.ReturnCode = ReturnCode;
            this.Message = Message;
        }
    }
}
