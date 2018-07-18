using ITSRunning.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITSRunning.DataAccess.Runners
{
    public interface IRunnerRepository : IRepository<Runner, int>
    {
        Runner GetByUsername(string username);
    }
}
