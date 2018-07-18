using System;
using System.Collections.Generic;
using System.Text;

namespace ITSRunning.Models.Requests
{
    public class ListTrainingRequest : Request
    {
        public ListTrainingRequest()
        {

        }
        public string RunnerUsername { get; set; }
    }
}
