using CultObjectProcessing;

namespace DataFilter
{
    /// <summary>
    /// Provides methods for processing and filtering cultural objects.
    /// </summary>
    public static class FilterProcessing
    {
        /// <summary>
        /// Gets an array of unique values for the specified field from a list of cultural objects.
        /// </summary>
        /// <param name="field">The name of the field to get values for (SecurityStatus, ObjectType, or Category).</param>
        /// <param name="objects">The list of cultural objects to process.</param>
        /// <returns>An array of unique values for the specified field from the given list of cultural objects.</returns>
        public static string[] GetVariants(string field, List<CultObject> objects)
        {
            SortedSet<string> variants = new SortedSet<string>();
            foreach (CultObject obj in objects)
            {
                switch (field)
                {
                    case "SecurityStatus":
                        if (obj.SecurityStatus != null)
                            variants.Add(obj.SecurityStatus);
                        break;
                    case "ObjectType":
                        if (obj.ObjectType != null)
                            variants.Add(obj.ObjectType);
                        break;
                    case "Category":
                        if (obj.Category != null)
                            variants.Add(obj.Category);
                        break;
                }
            }
            return variants.ToArray();
        }
    }
}
