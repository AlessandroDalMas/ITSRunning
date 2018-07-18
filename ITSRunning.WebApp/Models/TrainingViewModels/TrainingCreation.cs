using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITSRunning.WebApp.Models.TrainingViewModels
{
    public class TrainingCreation
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
    }
}
