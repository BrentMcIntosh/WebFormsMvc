using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace WebFormsMvc.Models
{
    public class UploaderViewModel
    {
        #region UserSelectionProperties
        [Description("File Selection - Maximum File Size in Bytes Allowed")]
        public decimal SelectionMaxSize { get; set; }

        [Description("File Selection - Comma Delimited List of Acceptable Mime Types or File Extensions")]
        public string SelectionTypeFilter { get; set; }

        [Description("File Selection - Target Url for a Save Request via Ajax")]
        public string SelectionSaveUrl { get; set; }

        [Description("File Selection - Target Url for a Save Request via a Form Post Submit")]
        public string SelectionSaveUrlLegacy { get; set; }
        #endregion

        #region StoredFileProperties
        [Description("Stored File - Target Url for a Delete Request to the Server")]
        public string StoredDeleteUrl { get; set; }

        [Description("Stored File - Whether or not to Allow the Delete Request")]
        public bool StoredDeleteAllowed { get; set; }

        [Description("Stored File - Url Where the File is Stored")]
        public string StoredUrl { get; set; }

        [Description("Stored File - Flag Whether or not there is a Server Stored File")]
        public bool HasStored { get; set; }
        #endregion

        #region ConfigurationProperties
        [Description("Configuration - Element ID for the Uploader Container")]
        public string ContainerId { get; set; }

        [Description("Configuration - Key Uniquely Identifying the File")]
        public int Key { get; set; }

        [Description("Configuration - Include Image Previews in the Uploader")]
        [DefaultValue(false)]
        public bool IsImage { get; set; }

        [Description("Configuration - Uploader Heading Text")]
        public string Heading { get; set; }

        [Description("Configuration - Whether to Display the Heading")]
        [DefaultValue(true)]
        public bool HeaderVisible { get; set; }

        [Description("Configuration - Message on Detection of a Legacy Browser")]
        public string LegacyBrowserMessage { get; set; }

        [Description("Configuration - Instructions for Legacy Browsers")]
        public string LegacyBrowserInstruction { get; set; }

        [Description("Configuration - Message for Selections Exceeding Max Filesize")]
        public string FileSizeExceededMessage { get; set; }

        [Description("Configuration - Message for Selections of Invalid File Types")]
        public string FileTypeInvalidMessage { get; set; }

        [Description("Configuration - Message for Server Response Errors")]
        public string ServerResponseErrorMessage { get; set; }

        [Description("Configuration - Whether or not to Hide the Delete Action")]
        [DefaultValue(false)]
        public bool StoredDeleteHidden { get; set; }

        [Description("Configuration - Message for Selections Exceeding Allowed File Count")]
        public string FileCountExceededMessage { get; set; }

        [Description("Configuration - Dictionary of Optional Parameters [name, value] Sent with All Server Requests")]
        public Dictionary<string, object> RequestParameters { get; set; }
        #endregion

        #region constructors

        public UploaderViewModel()
        {
            HeaderVisible = true;
            IsImage = false;
            StoredDeleteHidden = false;
            RequestParameters = new Dictionary<string, object>();
        }
        #endregion

        public string ToJson()
        {
            return Json.Encode(this);
        }

    }
}