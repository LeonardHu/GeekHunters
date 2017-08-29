using GeekHunter.Data.Implementation.Entities;
using System;

namespace GeekHunter.Data.Implementation.Responsitories
{
    public abstract class BaseReponsitory:IDisposable
    {
        protected GeekHunterEntities ctx;

        public BaseReponsitory()
        {
            ctx = new GeekHunterEntities();
        }

        public BaseReponsitory(GeekHunterEntities context)
        {
            ctx = context;
        }

        public void Dispose()
        {
            ctx.Database.Connection.Close();
            ctx.Dispose();
        }
    }
}
