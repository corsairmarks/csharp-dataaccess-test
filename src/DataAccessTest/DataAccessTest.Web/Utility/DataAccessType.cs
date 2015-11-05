namespace DataAccessTest.Web.Utility
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Enumeration of data access types available for testing.
    /// </summary>
    public enum DataAccessType
    {
        /// <summary>
        /// Entity Framework ORM from Microsoft
        /// </summary>
        EntityFramework = 1,

        /// <summary>
        /// PetaPoco micro-ORM from Topten Software
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Proper names.")]
        PetaPoco,

        /// <summary>
        /// NHibernate ORM, based Hibernate for Java
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Proper name.")]
        NHibernate,
    }
}