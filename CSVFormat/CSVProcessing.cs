using CultObjectProcessing;
using System.Text;

namespace CSVFormat
{
    /// <summary>
    /// Provides methods for reading and writing cultural objects in CSV format.
    /// </summary>
    public class CSVProcessing
    {
        /// <summary>
        /// Reads cultural objects from a stream in CSV format.
        /// </summary>
        /// <param name="stream">The stream containing the CSV data.</param>
        /// <returns>A list of cultural objects read from the CSV data, or null if the data is invalid.</returns>
        public static List<CultObject>? Read(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            string[] lines = reader.ReadToEnd().Trim('\n').Split('\n');
            List<CultObject> objects = new List<CultObject>();
            if (!CSVReadProcessing.CheckData(lines[0].Split(';')) || lines.Length < 3)
                return null;
            for (int i = 2; i < lines.Count(); i++)
            {
                string[] fields = lines[i].Split(';');
                if (fields.Length >= 8)
                    objects.Add(CSVReadProcessing.MakeObject(fields));
            }
            reader.Close();
            return objects;
        }

        /// <summary>
        /// Writes a list of cultural objects to a stream in CSV format.
        /// </summary>
        /// <param name="objects">The list of cultural objects to write.</param>
        /// <returns>A stream containing the CSV data for the given cultural objects.</returns>
        public static Stream Write(List<CultObject> objects)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);

            writer.WriteLine(string.Join(";", new[]
            {
            "\"AISID\"", "\"USRCHONumber\"", "\"ObjectNameOnDoc\"", "\"EnsembleNameOnDoc\"", "\"SecurityStatus\"",
            "\"Category\"", "\"ObjectType\"", "\"global_id\""
            }) + ";");

            writer.WriteLine(string.Join(";", new[]
            {
            "\"Идентификатор в АИС Мосгорнаследия\"", "\"Номер ЕГРОКН\"", "\"Наименование объекта по документам\"",
            "\"Наименование ансамбля по документам\"", "\"Охранный статус\"", "\"Категория объекта\"",
            "\"Вид объекта недвижимости\"", "\"global_id\""
            }) + ";");

            foreach (var cultObject in objects)
            {
                writer.WriteLine(string.Join(";", new[]
                {
                $"\"{cultObject.AISID}\"",
                $"\"{ cultObject.USRCHONumber}\"",
                $"\"{cultObject.ObjectNameOnDoc}\"",
                $"\"{cultObject.EnsembleNameOnDoc}\"",
                $"\"{cultObject.SecurityStatus}\"",
                $"\"{cultObject.Category}\"",
                $"\"{cultObject.ObjectType}\"",
                $"\"{cultObject.Global_id}\""
                }
                ) + ";");
            };
            writer.Flush();
            stream.Position = 0;

            return stream;
        }
    }
}