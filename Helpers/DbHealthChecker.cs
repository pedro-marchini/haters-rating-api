using Microsoft.EntityFrameworkCore;

namespace HatersRating.Helpers
{
    public class DbHealthChecker
    {
        private static async Task TestConnection(DbContext context)
        {
            const int maxAttemps = 10;
            const int delay = 5000;

            for (var i = 0; i < maxAttemps; i++)
            {
                var canConnect = CanConnect(context);
                if (canConnect)
                {
                    return;
                }
                await Task.Delay(delay);
            }

            // after a few attemps we give up
            throw new Exception("Error waiting database. Check ConnectionString and ensure database exist");
        }

        public static async Task WaitForTable<T>(DbContext context) where T : class
        {
            const int maxAttemps = 10;
            const int delay = 5000;

            for (var i = 0; i < maxAttemps; i++)
            {
                var tableExist = await CheckTable<T>(context);
                if (tableExist)
                {
                    return;
                }
                await Task.Delay(delay);
            }

            // after a few attemps we give up
            throw new Exception("Error waiting database. Check ConnectionString and ensure database exist");
        }

        private static bool CanConnect(DbContext context)
        {
            try
            {
                context.Database.GetAppliedMigrations();   // Check the database connection
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        /// <summary>
        /// Check if data table is exist in application
        /// </summary>
        /// <typeparam name="T">Class of data table to check</typeparam>
        /// <param name="db">DB Object</param>
        public static async Task<bool> CheckTableExists<T>(DbContext db) where T : class
        {
            await TestConnection(db);
            return await CheckTable<T>(db);
        }

        private static async Task<bool> CheckTable<T>(DbContext db) where T : class
        {
            try
            {
                await db.Set<T>().AnyAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}