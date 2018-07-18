using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ITSRunning.Models.Requests
{
    public class NewTrainingRequest : Request
    {
        public NewTrainingRequest()
        {
        }
        [Required]
        public string RunnerUsername { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
