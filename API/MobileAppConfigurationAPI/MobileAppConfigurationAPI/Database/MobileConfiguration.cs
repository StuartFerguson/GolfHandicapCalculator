using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileAppConfigurationAPI.Database
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class MobileConfiguration : DbContext
    {
        private readonly String ConnectionString;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileConfiguration"/> class.
        /// </summary>
        public MobileConfiguration()
        {
            // Use this for migrations
            this.ConnectionString = "server=localhost;database=MobileConfiguration;user id=root;password=Pa55word";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileConfiguration" /> class using the connection string passed in.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public MobileConfiguration(String connectionString)
        {
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileConfiguration"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MobileConfiguration(DbContextOptions<MobileConfiguration> options) : base(options)
        {
        }

        #endregion

        #region protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)        
        /// <summary>
        /// <para>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// This method is called for each instance of the context that is created.
        /// </para>
        /// <para>
        /// In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" /> may or may not have been passed
        /// to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" /> to determine if
        /// the options have already been set, and skip some or all of the logic in
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
        /// </para>
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or modify options for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you to configure the context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!String.IsNullOrWhiteSpace(this.ConnectionString))
            {
                optionsBuilder.UseMySql(this.ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }
        #endregion

        public DbSet<ApplicationConfiguration> ApplicationConfiguration { get; set; }
    }
}
