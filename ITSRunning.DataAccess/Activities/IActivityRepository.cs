using ITSRunning.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITSRunning.DataAccess.Activities
{
    public interface IActivityRepository: IRepository<Activity, int>
    {
        IEnumerable<Activity> GetByTypeAndUsername(int type, string username);
    }
}
