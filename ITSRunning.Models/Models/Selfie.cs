using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITSRunning.Models.Models
{
    public class Selfie
    {
        public string Pic { get; set; }
        public int IdActivity { get; set; }
        public DateTime Instant { get; set; }
    }
}
