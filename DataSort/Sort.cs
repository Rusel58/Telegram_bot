using CultObjectProcessing;

namespace DataSort
{
    /// <summary>
    /// Provides methods for sorting collections of cultural objects.
    /// </summary>
    public static class Sort
    {
        /// <summary>
        /// Sorts a collection of cultural objects by their ObjectNameOnDoc property in either ascending or descending order.
        /// </summary>
        /// <param name="cultObjects">The collection of cultural objects to sort.</param>
        /// <param name="ascending">A boolean value indicating whether to sort in ascending order (true) or descending order (false).</param>
        /// <returns>A new collection of cultural objects sorted by their ObjectNameOnDoc property in the specified order.</returns>
        public static IEnumerable<CultObject> SortByObjectNameOnDoc(IEnumerable<CultObject> cultObjects, bool ascending)
        {
            return ascending
                ? cultObjects.OrderBy(obj => obj.ObjectNameOnDoc)
                : cultObjects.OrderByDescending(obj => obj.ObjectNameOnDoc);
        }
    }
}