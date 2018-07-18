using ITSRunning.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITSRunning.Models.Responses
{
    public class ListTrainingResponse : Response
    {
        public IEnumerable<Activity> Activities { get; set; }
        public string RunnerUsername { get; set; }
    }
}
