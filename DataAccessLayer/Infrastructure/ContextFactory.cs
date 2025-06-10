using System.Data.Entity.Infrastructure;

namespace DataAccessLayer.Infrastructure
{
    public class ContextFactory : IDbContextFactory<DataAccessLayer.Context.Context>
    {
        public DataAccessLayer.Context.Context Create()
        {
            return new DataAccessLayer.Context.Context();
        }
    }
}
