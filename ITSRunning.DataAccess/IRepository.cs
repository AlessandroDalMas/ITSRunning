using System;
using System.Collections.Generic;
using System.Text;

namespace ITSRunning.DataAccess
{
    public interface IRepository<T, TKey> where T : new() where TKey : struct
    {
        IEnumerable<T> Get();

        T Get(TKey id);

        void Update(T value);

        void Insert(T value);

        void Delete(TKey id);
    }
}
