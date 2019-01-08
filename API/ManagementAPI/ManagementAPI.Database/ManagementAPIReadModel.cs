using ManagementAPI.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.Database
{
    public class ManagementAPIReadModel : DbContext
    {
        private readonly String ConnectionString;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementAPIReadModel"/> class.
        /// </summary>
        public ManagementAPIReadModel()
        {
            // Use this for migrations
            this.ConnectionString = "server=localhost;database=ManagementAPIReadModel;user id=root;password=Pa55word";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementAPIReadModel" /> class using the connection string passed in.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ManagementAPIReadModel(String connectionString)
        {
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementAPIReadModel"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ManagementAPIReadModel(DbContextOptions<ManagementAPIReadModel> options) : base(options)
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

        #region Entities

        /// <summary>
        /// Gets or sets the golf club.
        /// </summary>
        /// <value>
        /// The golf club.
        /// </value>
        public DbSet<GolfClub> GolfClub { get; set; }

        /// <summary>
        /// Gets or sets the club membership request.
        /// </summary>
        /// <value>
        /// The club membership request.
        /// </value>
        public DbSet<ClubMembershipRequest> ClubMembershipRequest { get; set; }

        #endregion
    }
}
