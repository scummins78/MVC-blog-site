using System.Data.Entity;

namespace DataRepository.Repository.EF
{
    public class BaseContext<TContext> : DbContext where TContext : DbContext
    {
        static BaseContext()
        { 
            Database.SetInitializer<TContext>(null);
        }

        protected BaseContext()
            : base("name=BlogConnection")
        {
        }
    }
}
