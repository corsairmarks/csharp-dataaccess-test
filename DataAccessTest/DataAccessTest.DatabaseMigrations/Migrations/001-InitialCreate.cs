namespace DataAccessTest.DatabaseMigrations.Migrations
{
    using System;
    using FluentMigrator;

    /// <summary>
    /// Initial database migration (create initial tables).
    /// </summary>
    [Migration(1)]
    public class InitialCreate : AutoReversingMigration
    {
        #region Overridden Members

        /// <summary>
        /// Migrate the database up one revision.
        /// </summary>
        public override void Up()
        {
            Create.Table("Value").InSchema("dbo")
                .WithColumn("Id").AsInt32().Identity().NotNullable()
                .WithColumn("Desc").AsString(256).NotNullable();
        }

        #endregion
    }
}