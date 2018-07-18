using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ITSRunning.Models.Requests
{
    public class StartTrainingRequest : Request
    {
        public StartTrainingRequest()
        {
        }
        [Required]
        public string Username { get; set; }
        [Required]
        public int IdActivity { get; set; }
    }
}
