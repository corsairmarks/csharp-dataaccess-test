﻿namespace DataAccessTest.Web.DependencyResolution.Registry
{
    using System.Diagnostics;
    using System.Reflection;
    using DataAccessTest.DatabaseMigrations.Migrations;
    using FluentMigrator;
    using FluentMigrator.Runner;
    using FluentMigrator.Runner.Announcers;
    using FluentMigrator.Runner.Initialization;
    using FluentMigrator.Runner.Processors;
    using FluentMigrator.Runner.Processors.SqlServer;
    using StructureMap.Configuration.DSL;

    /// <summary>
    /// Configures dependency resolution for database migrations.
    /// </summary>
    public class DatabaseMigrationRegistry : Registry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseMigrationRegistry"/> class.
        /// </summary>
        public DatabaseMigrationRegistry()
        {
            this.IncludeRegistry<SqlDatabaseRegistry>();
            this.ForSingletonOf<IMigrationProcessorOptions>().Use<ProcessorOptions>()
                .Setter<bool>(po => po.PreviewOnly).Is(false)
                .Setter<int>(po => po.Timeout).Is(60);

            // StructureMap does not properly find the Action<string> controller when
            // attempting injection, so I resorted to specifying a concrete object for the Singleton
            this.ForSingletonOf<IAnnouncer>().Use(new TextWriterAnnouncer(s => Debug.Write(s)));
            var migrationsAssembly = Assembly.GetAssembly(typeof(InitialCreate));
            this.ForSingletonOf<IRunnerContext>()
                .Use<RunnerContext>()
                .Setter<string>(rc => rc.Namespace)
                .Is(typeof(InitialCreate).Namespace);
            this.ForSingletonOf<IMigrationProcessorFactory>().Use<SqlServer2014ProcessorFactory>();
            this.ForSingletonOf<IMigrationProcessor>()
                .Use(c => c.GetInstance<IMigrationProcessorFactory>().Create(
                        c.GetInstance<string>("DefaultConnectionString"),
                            c.GetInstance<IAnnouncer>(),
                            c.GetInstance<IMigrationProcessorOptions>()));

            this.ForSingletonOf<IMigrationRunner>()
                .Use<MigrationRunner>()
                .SelectConstructor(() => new MigrationRunner(null as Assembly, null, null))
                .Ctor<Assembly>()
                .Is(migrationsAssembly);
        }
    }
}