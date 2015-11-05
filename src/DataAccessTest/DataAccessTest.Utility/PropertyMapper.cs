namespace DataAccessTest.Utility
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Utility class for getting property information on types.
    /// </summary>
    /// <typeparam name="T">The type on which to reflect.</typeparam>
    public static class PropertyMapper<T>
    {
        /// <summary>
        /// Get the <see cref="PropertyInfo"/> for the property in the <paramref name="expression"/>.
        /// </summary>
        /// <typeparam name="U">The type on which to reflect on a property.</typeparam>
        /// <param name="expression">An expression pointing to a property in the <typeparamref name="U"/> class.</param>
        /// <returns>The <see cref="PropertyInfo"/> describing the property.</returns>
        public static PropertyInfo PropertyInfo<U>(Expression<Func<T, U>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member != null && member.Member is PropertyInfo)
            {
                return member.Member as PropertyInfo;
            }

            throw new ArgumentException("Expression is not a Property", "expression");
        }

        /// <summary>
        /// Get the <see cref="PropertyInfo.Name"/> for the property in the <paramref name="expression"/>.
        /// </summary>
        /// <typeparam name="U">The type on which to reflect on a property.</typeparam>
        /// <param name="expression">An expression pointing to a property in the <typeparamref name="U"/> class.</param>
        /// <returns>The name of the property.</returns>
        public static string PropertyName<U>(Expression<Func<T, U>> expression)
        {
            return PropertyInfo<U>(expression).Name;
        }

        /// <summary>
        /// Get the <see cref="Type"/> for the property in the <paramref name="expression"/>.
        /// </summary>
        /// <typeparam name="U">The type on which to reflect on a property.</typeparam>
        /// <param name="expression">An expression pointing to a property in the <typeparamref name="U"/> class.</param>
        /// <returns>The <see cref="Type"/> of the property.</returns>
        public static Type PropertyType<U>(Expression<Func<T, U>> expression)
        {
            return PropertyInfo<U>(expression).PropertyType;
        }
    }
}