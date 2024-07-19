using CultObjectProcessing;

namespace CSVFormat
{
    /// <summary>
    /// Provides utility methods for reading and processing cultural objects in CSV format.
    /// </summary>
    public static class CSVReadProcessing
    {
        /// <summary>
        /// Checks if the provided array of fields matches the expected header format for cultural objects in CSV format.
        /// </summary>
        /// <param name="fields">The array of field names to check.</param>
        /// <returns>True if the provided fields match the expected header format, false otherwise.</returns>
        public static bool CheckData(string[] fields)
        {
            string[] header = new string[] { "AISID", "USRCHONumber", "ObjectNameOnDoc", "EnsembleNameOnDoc",
                "SecurityStatus", "Category", "ObjectType", "global_id"};
            if (fields.Length >= 8)
            {
                for (int i = 0; i < header.Length; i++)
                {
                    if (header[i] != ElementFormating(fields[i]))
                        return false;
                }
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Creates a new CultObject instance from the provided array of fields in CSV format.
        /// </summary>
        /// <param name="fields">The array of field values representing a cultural object.</param>
        /// <returns>A new CultObject instance created from the provided field values.</returns>
        public static CultObject MakeObject(string[] fields)
        {
            CultObject obj = new CultObject(ElementFormating(fields[0]), ElementFormating(fields[1]), ElementFormating(fields[2]),
                ElementFormating(fields[3]), ElementFormating(fields[4]), ElementFormating(fields[5]), ElementFormating(fields[6]),
                ElementFormating(fields[7]));
            return obj;
        }

        /// <summary>
        /// Formats a string by removing the leading and trailing double quotes, if present.
        /// </summary>
        /// <param name="s">The string to format.</param>
        /// <returns>The formatted string without leading and trailing double quotes, or an empty string if the input was null.</returns>
        public static string ElementFormating(string s)
        {
            if (s.Length >= 2)
                s = s[1..^1];
            return s ?? string.Empty;
        }
    }
}
