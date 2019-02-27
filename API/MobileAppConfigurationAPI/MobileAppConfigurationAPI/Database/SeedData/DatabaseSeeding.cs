namespace MobileAppConfigurationAPI.Database.SeedData
{
    using System;
    using Microsoft.EntityFrameworkCore;

    public class DatabaseSeeding
    {
        #region Methods

        public static void InitialiseDatabase(MobileConfiguration mobileConfiguration)
        {
            try
            {
                if (mobileConfiguration.Database.IsMySql())
                {
                    mobileConfiguration.Database.Migrate();
                }

                mobileConfiguration.SaveChanges();
            }
            catch(Exception ex)
            {
                String connString = mobileConfiguration.Database.GetDbConnection().ConnectionString;

                Exception newException = new Exception($"Connection String [{connString}]", ex);
                throw newException;
            }
        }

        #endregion
    }
}