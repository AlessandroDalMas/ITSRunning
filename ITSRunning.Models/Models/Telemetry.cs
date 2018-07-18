using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ITSRunning.Models.Models
{
    public class Telemetry
    {
        public int Id { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public DateTime Instant { get; set; }
        public int IdRunner { get; set; }
        public int IdActivity { get; set; }
        public string UriSelfie { get; set; }
    }
}
