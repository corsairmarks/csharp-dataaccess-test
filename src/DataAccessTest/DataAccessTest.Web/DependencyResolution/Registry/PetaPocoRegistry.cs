namespace DataAccessTest.Web.DependencyResolution.Registry
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using DataAccessTest.Library.Value;
    using DataAccessTest.Repository;
    using DataAccessTest.Repository.PetaPoco;
    using DataAccessTest.Repository.PetaPoco.Mapping;
    using PetaPoco;
    using StructureMap.Configuration.DSL;
    using StructureMap.Pipeline;

    /// <summary>
    /// Configures dependency resolution for PetaPoco.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Proper name.")]
    public class PetaPocoRegistry : Registry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PetaPocoRegistry"/> class.
        /// </summary>
        public PetaPocoRegistry()
        {
            this.IncludeRegistry<SqlDatabaseRegistry>();
            this.ForConcreteType<Database>().Configure
                .LifecycleIs<ContainerLifecycle>()
                .SelectConstructor(() => new Database(null as IDbConnection))
                .Ctor<IDbConnection>()
                .Is(
                    "Use the registered IDbConnection, but open it first.",
                    c =>
                    {
                        var dbConnection = c.GetInstance<IDbConnection>();
                        dbConnection.Open();
                        return dbConnection;
                    });
            this.For<ITransaction>()
                .LifecycleIs<ContainerLifecycle>()
                .Use(c => c.GetInstance<Database>().GetTransaction());

            this.Scan(sc =>
            {
                sc.AssemblyContainingType<ISingleTypeMapper>();
                sc.AddAllTypesOf<ISingleTypeMapper>();
            });

            this.For<IUnitOfWork>().Use<PetaPocoUnitOfWork>();

            this.ForSingletonOf<IDataAccessInitializer>()
                .Use<PetaPocoDbInitializer>()
                .Ctor<Func<IEnumerable<ISingleTypeMapper>>>()
                .Is(c => c.GetAllInstances<ISingleTypeMapper>);

            this.ConfigureGenericRepository<ValueModel>();
        }

        /// <summary>
        /// Configure a generic PetaPoco repository for type <typeparamref name="TModel"/>.
        /// </summary>
        /// <typeparam name="TModel">The type of the repository model.</typeparam>
        private void ConfigureGenericRepository<TModel>()
            where TModel : class
        {
            this.For<PetaPocoGenericRepository<TModel>>().LifecycleIs<ContainerLifecycle>();
            this.Forward<PetaPocoGenericRepository<TModel>, IGenericReadRepository<TModel>>();
            this.Forward<PetaPocoGenericRepository<TModel>, IGenericWriteRepository<TModel>>();
            this.Forward<PetaPocoGenericRepository<TModel>, IGenericRepository<TModel>>();
        }
    }
}