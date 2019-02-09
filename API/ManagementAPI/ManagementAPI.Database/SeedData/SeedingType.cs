using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ManagementAPI.Database.SeedData
{
    public enum SeedingType
    {
        NotSet = 0,
        IntegrationTest,
        Development,
        Staging,
        Production
    }

    public class DatabaseSeeding
    {
        public static void InitialiseDatabase(ManagementAPIReadModel managementApiReadModel, 
                                              SeedingType seedingType)
        {
            try
            {
                if (managementApiReadModel.Database.IsMySql())
                {
                    managementApiReadModel.Database.Migrate();
                }
                
                managementApiReadModel.SaveChanges();
                
            }
            catch (Exception ex)
            {
                String connString = managementApiReadModel.Database.GetDbConnection().ConnectionString;

                Exception newException = new Exception($"Connection String [{connString}]", ex);
                throw newException;
            }
        }
        
        
    }
}
