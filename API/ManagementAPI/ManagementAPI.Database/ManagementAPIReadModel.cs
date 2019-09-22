namespace ManagementAPI.Database
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ManagementAPIReadModel : DbContext
    {
        #region Fields

        private readonly String ConnectionString;

        #endregion

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

        #region Properties

        /// <summary>
        /// Gets or sets the golf club.
        /// </summary>
        /// <value>
        /// The golf club.
        /// </value>
        public DbSet<GolfClub> GolfClub { get; set; }

        /// <summary>
        /// Gets or sets the player club membership.
        /// </summary>
        /// <value>
        /// The player club membership.
        /// </value>
        public DbSet<PlayerClubMembership> PlayerClubMembership { get; set; }

        /// <summary>
        /// Gets or sets the tournament.
        /// </summary>
        /// <value>
        /// The tournament.
        /// </value>
        public DbSet<Tournament> Tournament { get; set; }

        /// <summary>
        /// Gets or sets the tournament result for player score.
        /// </summary>
        /// <value>
        /// The tournament result for player score.
        /// </value>
        public DbSet<TournamentResultForPlayerScore> TournamentResultForPlayerScore { get; set; }

        /// <summary>
        /// Gets or sets the golf club membership reporting.
        /// </summary>
        /// <value>
        /// The golf club membership reporting.
        /// </value>
        public DbSet<GolfClubMembershipReporting> GolfClubMembershipReporting { get; set; }

        /// <summary>
        /// Gets or sets the player handicap list reporting.
        /// </summary>
        /// <value>
        /// The player handicap list reporting.
        /// </value>
        public DbSet<PlayerHandicapListReporting> PlayerHandicapListReporting { get; set; }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the published player scores.
        /// </summary>
        /// <value>
        /// The published player scores.
        /// </value>
        public DbSet<PublishedPlayerScore> PublishedPlayerScores { get; set; }

        /// <summary>
        /// Gets or sets the player tournament sign ups.
        /// </summary>
        /// <value>
        /// The player tournament sign ups.
        /// </value>
        public DbSet<PlayerTournamentSignUp> PlayerTournamentSignUps { get; set; }

        /// <summary>
        /// Gets or sets the measured courses.
        /// </summary>
        /// <value>
        /// The measured courses.
        /// </value>
        public DbSet<MeasuredCourse> MeasuredCourses { get; set; }

        #endregion

        #region Methods

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
            if (!string.IsNullOrWhiteSpace(this.ConnectionString))
            {
                optionsBuilder.UseMySql(this.ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerClubMembership>().HasKey(p => new
                                                                    {
                                                                        p.PlayerId,
                                                                        p.GolfClubId
                                                                    });

            modelBuilder.Entity<User>().HasKey(p => new
                                                                    {
                                                                        p.GolfClubId,
                                                                        p.UserId
                                                                    });

            modelBuilder.Entity<GolfClubMembershipReporting>().HasKey(p => new
                                                    {
                                                        p.GolfClubId,
                                                        p.PlayerId
                                                    });

            modelBuilder.Entity<PlayerHandicapListReporting>().HasKey(p => new
                                                                           {
                                                                               p.PlayerId,
                                                                               p.GolfClubId
                                                                           });

            modelBuilder.Entity<PublishedPlayerScore>().HasKey(p => new
                                                                    {
                                                                        p.PlayerId,
                                                                        p.GolfClubId,
                                                                        p.TournamentId,
                                                                        p.MeasuredCourseId
                                                                    });

            modelBuilder.Entity<PlayerTournamentSignUp>().HasKey(p => new
                                                                    {
                                                                        p.PlayerId,
                                                                        p.TournamentId,
                                                                    });

            modelBuilder.Entity<MeasuredCourse>().HasKey(p => new
                                                                      {
                                                                          p.GolfClubId,
                                                                          p.MeasuredCourseId,
                                                                      });
        }

        #endregion
    }
}