namespace ITSRunning.Models.Requests
{
    public abstract class Request
    {
        public Request()
        {
            RequestType = this.GetType().Name;
        }
        public string RequestType { get; set; }
    }
}