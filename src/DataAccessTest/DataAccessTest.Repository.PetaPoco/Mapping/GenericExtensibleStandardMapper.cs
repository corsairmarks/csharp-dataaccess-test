namespace DataAccessTest.Repository.PetaPoco.Mapping
{
    using System;
    using System.Reflection;
    using global::PetaPoco;

    /// <summary>
    /// A generic POCO mapper designed to work only on a specific <typeparamref name="TEntity"/> class.
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="Type"/> for which this <see cref="IMapper"/> can supply metadata and conversion functions.</typeparam>
    public abstract class GenericExtensibleStandardMapper<TEntity> : ExtensibleStandardMapper
        where TEntity : class
    {
        /// <summary>
        /// Constructs a <see cref="TableInfo"/> for a POCO by reading its attribute data.
        /// </summary>
        /// <param name="pocoType">The POCO <see cref="Type"/>.</param>
        /// <returns>A <see cref="TableInfo"/> metadata object describing the table for <paramref name="pocoType"/>.</returns>
        public override sealed TableInfo GetTableInfo(Type pocoType)
        {
            if (pocoType == typeof(TEntity))
            {
                return this.GetTableInfoCore();
            }

            throw new ArgumentException(string.Format("pocoType must be {0}", typeof(TEntity).FullName), "pocoType");
        }

        /// <summary>
        /// Constructs a <see cref="ColumnInfo"/> for a POCO property by reading its attribute data.
        /// </summary>
        /// <param name="pocoProperty">Information on the POCO <see cref="PropertyInfo"/> to map to a column.</param>
        /// <returns>A <see cref="ColumnInfo"/> metadata object describing the column for <paramref name="pocoProperty"/>.</returns>
        public override sealed ColumnInfo GetColumnInfo(PropertyInfo pocoProperty)
        {
            if (pocoProperty.DeclaringType == typeof(TEntity))
            {
                return this.GetColumnInfoCore(pocoProperty);
            }

            throw new ArgumentException(string.Format("pocoProperty must belong to {0}", typeof(TEntity).FullName), "pocoProperty");
        }

        /// <summary>
        /// Get a function that can convert into <paramref name="targetProperty"/> from the <paramref name="sourceType"/>.
        /// </summary>
        /// <remarks>
        /// A return value of <c>null</c> use the default conversion for PetaPoco.
        /// </remarks>
        /// <param name="targetProperty">Metadata about the target property on a POCO into which the database data will be mapped.</param>
        /// <param name="sourceType">The <see cref="Type"/> of the source column in the database.</param>
        /// <returns>A function that converts from <paramref name="sourceType"/> into <paramref name="targetProperty"/>.</returns>
        public override sealed Func<object, object> GetFromDbConverter(PropertyInfo targetProperty, Type sourceType)
        {
            if (targetProperty.DeclaringType == typeof(TEntity))
            {
                return this.GetFromDbConverterCore(targetProperty, sourceType);
            }

            throw new ArgumentException(string.Format("targetProperty must belong to {0}", typeof(TEntity).FullName), "targetProperty");
        }

        /// <summary>
        /// Get a function that can convert from <paramref name="sourceProperty"/> into the appropriate database type.
        /// </summary>
        /// <remarks>
        /// A return value of <c>null</c> use the default conversion for PetaPoco.
        /// </remarks>
        /// <param name="sourceProperty">Metadata about the source property on a POCO from which the data will be mapped.</param>
        /// <returns>A function that converts from <paramref name="sourceProperty"/> into the correct, implementation-defined database type.</returns>
        public override sealed Func<object, object> GetToDbConverter(PropertyInfo sourceProperty)
        {
            if (sourceProperty.DeclaringType == typeof(TEntity))
            {
                return this.GetToDbConverterCore(sourceProperty);
            }

            throw new ArgumentException(string.Format("sourceProperty must belong to {0}", typeof(TEntity).FullName), "sourceProperty");
        }

        /// <summary>
        /// Constructs a <see cref="TableInfo"/> for the <typeparamref name="TEntity"/> POCO by reading its attribute data.
        /// </summary>
        /// <returns>A <see cref="TableInfo"/> metadata object describing the table for <typeparamref name="TEntity"/>.</returns>
        protected abstract TableInfo GetTableInfoCore();

        /// <summary>
        /// Constructs a <see cref="ColumnInfo"/> for a POCO property by reading its attribute data.
        /// </summary>
        /// <param name="pocoProperty">Information on the POCO <see cref="PropertyInfo"/> to map to a column.</param>
        /// <returns>A <see cref="ColumnInfo"/> metadata object describing the column for <paramref name="pocoProperty"/>.</returns>
        protected virtual ColumnInfo GetColumnInfoCore(PropertyInfo pocoProperty)
        {
            return base.GetColumnInfo(pocoProperty);
        }

        /// <summary>
        /// Get a function that can convert into <paramref name="targetProperty"/> from the <paramref name="sourceType"/>.
        /// </summary>
        /// <remarks>
        /// A return value of <c>null</c> use the default conversion for PetaPoco.
        /// </remarks>
        /// <param name="targetProperty">Metadata about the target property on a POCO into which the database data will be mapped.</param>
        /// <param name="sourceType">The <see cref="Type"/> of the source column in the database.</param>
        /// <returns>A function that converts from <paramref name="sourceType"/> into <paramref name="targetProperty"/>.</returns>
        protected virtual Func<object, object> GetFromDbConverterCore(PropertyInfo targetProperty, Type sourceType)
        {
            return base.GetFromDbConverter(targetProperty, sourceType);
        }

        /// <summary>
        /// Get a function that can convert from <paramref name="sourceProperty"/> into the appropriate database type.
        /// </summary>
        /// <remarks>
        /// A return value of <c>null</c> use the default conversion for PetaPoco.
        /// </remarks>
        /// <param name="sourceProperty">Metadata about the source property on a POCO from which the data will be mapped.</param>
        /// <returns>A function that converts from <paramref name="sourceProperty"/> into the correct, implementation-defined database type.</returns>
        protected virtual Func<object, object> GetToDbConverterCore(PropertyInfo sourceProperty)
        {
            return base.GetToDbConverter(sourceProperty);
        }
    }
}