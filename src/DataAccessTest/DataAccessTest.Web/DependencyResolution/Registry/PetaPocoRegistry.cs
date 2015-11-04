namespace DataAccessTest.Web.DependencyResolution.Registry
{
    using System;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
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
                .Is(c => c.GetInstance<IDbConnection>());
            this.For<ITransaction>()
                .LifecycleIs<ContainerLifecycle>()
                .Use(c => c.GetInstance<Database>().GetTransaction());
            this.Scan(sc =>
            {
                sc.AssemblyContainingType<ExtensibleStandardMapper>();
                //sc.AddAllTypesOf(typeof(GenericExtensibleStandardMapper<>));
                // TODO: how to register?  Probably best to fetch all of them and register as named IMappers
                // Also need to registed with PetaPoco
                //Mappers.Register(null as Type, null as IMapper);
            });
        }
    }
}