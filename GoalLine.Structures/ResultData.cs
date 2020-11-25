using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalLine.Structures
{
    public class ResultData
    {
        public bool Success { get; set; }
        public string Description { get; set; }

        public ResultData()
        {

        }

        public ResultData(bool Success)
        {
            this.Success = Success;
            this.Description = "";
        }

        public ResultData(bool Success, string Description)
        {
            this.Success = Success;
            this.Description = Description;
        }
    }
}
