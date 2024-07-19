using CultObjectProcessing;

namespace TelegramBotProcessing
{
    /// <summary>
    /// Represents data related to a file containing cultural objects.
    /// </summary>
    public class FileData
    {
        /// <summary>
        /// Gets or sets the file type (JSON or CSV) of the cultural objects data.
        /// </summary>
        public FileType? FileType { get; set; }

        /// <summary>
        /// Gets or sets the list of cultural objects contained in the file.
        /// </summary>
        public List<CultObject>? Records { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current operation is in filter mode.
        /// </summary>
        public bool FilterMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current operation is in sort mode.
        /// </summary>
        public bool SortMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a file download is requested.
        /// </summary>
        public bool FileDownload { get; set; }

        /// <summary>
        /// Gets or sets the name of the field to filter the cultural objects by.
        /// </summary>
        public string? FilterField { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the filtering operation involves two fields.
        /// </summary>
        public bool IsTwoFields { get; set; }

        /// <summary>
        /// Gets or sets the first filter value for filtering cultural objects.
        /// </summary>
        public string? FirstFilterVar { get; set; }

        /// <summary>
        /// Gets or sets the second filter value for filtering cultural objects.
        /// </summary>
        public string? SecondFilterVar { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a file upload is requested.
        /// </summary>
        public bool FileUpload { get; set; }

        /// <summary>
        /// Resets the properties of the FileData instance to their default values.
        /// </summary>
        public void UpdateUserInfo()
        {
            FileType = null;
            Records = null;
            FilterMode = false;
            SortMode = false;
            FileDownload = false;
            FilterField = null;
            IsTwoFields = false;
            FirstFilterVar = null;
            SecondFilterVar = null;
            FileUpload = false;
        }

    }
}
