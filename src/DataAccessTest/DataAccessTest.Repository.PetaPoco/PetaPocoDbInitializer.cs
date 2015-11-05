namespace DataAccessTest.Repository.PetaPoco
{
    using System;
    using System.Collections.Generic;
    using DataAccessTest.Repository.PetaPoco.Mapping;
    using global::PetaPoco;

    /// <summary>
    /// Perform lifetime setup for Entity Framework data access.
    /// </summary>
    public class PetaPocoDbInitializer : IDataAccessInitializer
    {
        #region Fields

        /// <summary>
        /// A function to retrieve all the PetaPoco <see cref="ISingleTypeMapper"/> classes to initialize.
        /// </summary>
        private readonly Func<IEnumerable<ISingleTypeMapper>> getMappers;

        /// <summary>
        /// Whether the data access layer has been initialized.
        /// </summary>
        private bool isInitialized;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PetaPocoDbInitializer"/> class.
        /// </summary>
        /// <param name="getMappers">A function to retrieve all the PetaPoco <see cref="ISingleTypeMapper"/> classes to initialize.</param>
        public PetaPocoDbInitializer(Func<IEnumerable<ISingleTypeMapper>> getMappers)
        {
            if (getMappers == null)
            {
                throw new ArgumentNullException("getMappers");
            }

            this.getMappers = getMappers;
        }

        #endregion

        #region IDataAccessInitializer Members

        /// <summary>
        /// Perform any initialization required for the data access layer.  This should be called
        /// once when the application domain is being initialized.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when this method is called and the data access layer has already been initialized.
        /// </exception>
        public void Initialize()
        {
            if (this.isInitialized)
            {
                throw new InvalidOperationException("Initialize() can only be called once per application.");
            }

            foreach (var mapper in this.getMappers())
            {
                Mappers.Register(mapper.MappedType, mapper);
            }

            this.isInitialized = true;
        }

        #endregion
    }
}