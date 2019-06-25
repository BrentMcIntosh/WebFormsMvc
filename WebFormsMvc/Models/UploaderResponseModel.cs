
using System.ComponentModel;

namespace WebFormsMvc.Models
{
    public class UploaderResponseModel
    {
        [Description("Key Uniquely Identifying the File")]
        public int Key { get; set; }

        [Description("Url Where the File is Stored")]
        public string FileUrl { get; set; }

        public object ResponseObject { get; set; }

        [Description("Message Containing Information about the Storage State of the File")]
        public string ResponseMessage { get; set; }

        [Description("Whether the Upload was Successful")]
        public bool Success { get; set; }
    }
}