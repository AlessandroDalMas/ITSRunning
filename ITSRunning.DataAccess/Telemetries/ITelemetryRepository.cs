using ITSRunning.Models.Models;
using ITSRunning.Models.Telemetries;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITSRunning.DataAccess.Telemetries
{
    public interface ITelemetryRepository : IRepository<Telemetry, int>
    {
        void DeleteAll(int id);
        IEnumerable<TelemetryDetails> GetAll(int id);
        void Insert(Telemetry telemetry, string username);
        void Update(string uriPic, int idActivity, DateTime instant);
    }
}
