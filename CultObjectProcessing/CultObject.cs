using System.Text.Json.Serialization;

namespace CultObjectProcessing
{
    /// <summary>
    /// Represents a cultural object with various properties related to its identification and classification.
    /// </summary>
    [Serializable]
    public class CultObject
    {
        /// <summary>
        /// Gets or sets the Automated Information System ID (AISID) of the cultural object.
        /// </summary>
        [JsonPropertyName("AISID")]
        public string AISID { get; set; }

        /// <summary>
        /// Gets or sets the Unified State Register of Cultural Heritage Objects (USRCHONumber) number of the cultural object.
        /// </summary>
        [JsonPropertyName("USRCHONumber")]
        public string USRCHONumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the cultural object as listed on the documentation.
        /// </summary>
        [JsonPropertyName("ObjectNameOnDoc")]
        public string ObjectNameOnDoc { get; set; }

        /// <summary>
        /// Gets or sets the name of the ensemble to which the cultural object belongs, as listed on the documentation.
        /// </summary>
        [JsonPropertyName("EnsemblyNameOnDoc")]
        public string EnsembleNameOnDoc { get; set; }

        /// <summary>
        /// Gets or sets the security status of the cultural object.
        /// </summary>
        [JsonPropertyName("SecurityStatus")]
        public string SecurityStatus { get; set; }

        /// <summary>
        /// Gets or sets the category of the cultural object.
        /// </summary>
        [JsonPropertyName("Category")]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the type of the cultural object.
        /// </summary>
        [JsonPropertyName("ObjectType")]
        public string ObjectType { get; set; }

        /// <summary>
        /// Gets or sets the global identifier of the cultural object.
        /// </summary>
        [JsonPropertyName("global_id")]
        public string Global_id { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CultObject"/> class with all properties set to an empty string.
        /// </summary>
        public CultObject()
        {
            AISID = string.Empty;
            USRCHONumber = string.Empty;
            ObjectNameOnDoc = string.Empty;
            EnsembleNameOnDoc = string.Empty;
            SecurityStatus = string.Empty;
            Category = string.Empty;
            ObjectType = string.Empty;
            Global_id = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CultObject"/> class with the specified property values.
        /// </summary>
        /// <param name="aISID">The Automated Information System ID (AISID) of the cultural object.</param>
        /// <param name="uSRCHONumber">The Unified State Register of Cultural Heritage Objects (USRCHONumber) number of the cultural object.</param>
        /// <param name="objectNameOnDoc">The name of the cultural object as listed on the documentation.</param>
        /// <param name="ensembleNameOnDoc">The name of the ensemble to which the cultural object belongs, as listed on the documentation.</param>
        /// <param name="securityStatus">The security status of the cultural object.</param>
        /// <param name="category">The category of the cultural object.</param>
        /// <param name="objectType">The type of the cultural object.</param>
        /// <param name="global_id">The global identifier of the cultural object.</param>
        public CultObject(string aISID, string uSRCHONumber, string objectNameOnDoc, string ensembleNameOnDoc,
            string securityStatus, string category, string objectType, string global_id)
        {
            AISID = aISID;
            USRCHONumber = uSRCHONumber;
            ObjectNameOnDoc = objectNameOnDoc;
            EnsembleNameOnDoc = ensembleNameOnDoc;
            SecurityStatus = securityStatus;
            Category = category;
            ObjectType = objectType;
            Global_id = global_id;
        }
    }
}