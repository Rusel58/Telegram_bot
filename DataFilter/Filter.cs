using CultObjectProcessing;

namespace DataFilter
{
    /// <summary>
    /// Provides methods for filtering a collection of cultural objects based on specific criteria.
    /// </summary>
    public static class Filter
    {
        /// <summary>
        /// Filters a collection of cultural objects based on the specified field name and value.
        /// </summary>
        /// <param name="cultObjects">The collection of cultural objects to filter.</param>
        /// <param name="fieldName">The name of the field to filter by (SecurityStatus, Category, or ObjectType).</param>
        /// <param name="value">The value to filter the specified field by.</param>
        /// <returns>A new collection containing only the cultural objects where the specified field matches the given value.</returns>
        public static IEnumerable<CultObject> FilterByField(IEnumerable<CultObject> cultObjects, string fieldName, string value)
        {
            switch (fieldName)
            {
                case "SecurityStatus":
                    return cultObjects.Where(obj => obj.SecurityStatus == value);
                case "Category":
                    return cultObjects.Where(obj => obj.Category == value);
                case "ObjectType":
                    return cultObjects.Where(obj => obj.ObjectType == value);
                default:
                    return cultObjects;
            }
        }
    }
}