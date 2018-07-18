using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ITSRunning.Models.Telemetries
{
    public class TelemetryData
    {
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public DateTime Instant { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public int IdActivity { get; set; }
    }
}
