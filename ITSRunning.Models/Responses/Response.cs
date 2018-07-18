using System;
using System.Collections.Generic;
using System.Text;

namespace ITSRunning.Models.Responses
{
    public abstract class Response
    {
        public Response()
        {
            ResponseType = this.GetType().Name;
        }
        public string ResponseType { get; set; }
    }
}
