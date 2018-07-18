using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITSRunning.Models.Requests
{
    public class UpdateSelfieRequest : Request
    {
        public UpdateSelfieRequest()
        {
            
        }
        public string UriPic { get; set; }
        public int IdActivity { get; set; }
        public DateTime Instant { get; set; }

    }
}
