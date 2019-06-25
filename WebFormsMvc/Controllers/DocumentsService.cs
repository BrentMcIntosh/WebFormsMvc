using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace WebFormsMvc.Controllers
{
    internal class DocumentsService
    {
        public DocumentsService()
        {
        }

        public DocumentsService(object repository)
        {
        }

        internal FileStreamResult GetFileFromDisk(int requiredDocumentID) => new FileStreamResult(new FileStream("test.txt", FileMode.Create), "");

        internal bool UploadRequiredDocument(HttpPostedFileBase file, int requiredDocumentID, object siteID, out string uploadMessage, out string fileUrl)
        {
            uploadMessage = "";

            fileUrl = "";

            return true;
        }

        public int SaveRequiredDocumentation(int memberID, int requiredDocumentationID, string name, string description, out string error)
        {
            error = "";

            return 1;
        }
    }
}