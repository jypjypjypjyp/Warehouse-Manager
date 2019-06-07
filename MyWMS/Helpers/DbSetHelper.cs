using System.Data.Entity;

namespace MyWMS.Helpers
{
    public static class DbSetHelper
    {
        public static void Update<T>(this DbSet<T> dbSet, DbContext context, T item) where T : class
        {
            dbSet.Attach(item);
            context.Entry(item).State = EntityState.Modified;
        }
    }
}
