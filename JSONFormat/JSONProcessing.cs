using System.Text.Encodings.Web;
using System.Text;
using System.Text.Json;
using CultObjectProcessing;

namespace JSONFormat
{
    /// <summary>
    /// Provides methods for reading and writing cultural objects in JSON format.
    /// </summary>
    public class JSONProcessing
    {
        /// <summary>
        /// Reads a list of cultural objects from a stream in JSON format.
        /// </summary>
        /// <param name="stream">The stream containing the JSON data.</param>
        /// <returns>A list of cultural objects deserialized from the JSON data.</returns>
        public static List<CultObject> Read(Stream stream)
        {
            var cultObjects = JsonSerializer.Deserialize<List<CultObject>>(stream);
            return cultObjects ?? new List<CultObject>();
        }

        /// <summary>
        /// Writes a list of cultural objects to a stream in JSON format.
        /// </summary>
        /// <param name="objects">The list of cultural objects to write.</param>
        /// <returns>A stream containing the JSON data for the given cultural objects.</returns>
        public static Stream Write(List<CultObject> objects)
        {
            var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            var json = JsonSerializer.Serialize(objects, options);
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);

            writer.Write(json);
            writer.Flush();
            stream.Position = 0;

            return stream;
        }
    }
}