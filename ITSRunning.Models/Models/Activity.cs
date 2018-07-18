using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ITSRunning.Models.Models
{
    public class Activity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int IdRunner { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public string Location { get; set; }
        [Required]
        public int Type { get; set; }
        public string UriMatch { get; set; }
        [Required]
        public int State { get; set; }

    }
}
