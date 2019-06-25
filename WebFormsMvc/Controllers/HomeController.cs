using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using WebFormsMvc.Models;

namespace WebFormsMvc.Controllers
{
    public class HomeController : Controller
    {
        public object Repository { get; private set; }
        public object SiteCookie { get; private set; }

        public ActionResult Index()
        {
            decimal singleFileMax = 8 * 1024 * 1024;

            var uploaderModel = new UploaderViewModel()
            {
                Key = 1,
                SelectionMaxSize = 5 * singleFileMax,
                SelectionTypeFilter = string.Join(",", "pdf", "tiff", "tif"),
                SelectionSaveUrl = string.Format("..{0}", Url.Action("UploadRequiredDocumentAjax", "Home")),
                SelectionSaveUrlLegacy = String.Format("..{0}", Url.Action("UploadRequiredDocument", "Home")),
                HasStored = true,
                StoredDeleteAllowed = false,
                StoredDeleteHidden = true,
                StoredUrl = "",
                Heading = "The Heading File",
                FileCountExceededMessage = "Uploader Message File Count Exceeded",
                FileSizeExceededMessage = "Uploader Message File Size Exceeeded",
                FileTypeInvalidMessage = "Uploader Message File Type Invalid",
                LegacyBrowserInstruction = "Uploader Instruction LegacyBrowser",
                LegacyBrowserMessage = "Uploader Message LegacyBrowser",
                ServerResponseErrorMessage = "Uploader Message Server Response Error",
                RequestParameters = new Dictionary<string, object>()
                {
                    { "requiredDocumentID", 1 }
                }
            };

            return View(uploaderModel);
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase fileInfo)
        {
            if (fileInfo != null && fileInfo.ContentLength > 0)
            {
                var fileName = Path.GetFileName(fileInfo.FileName);

                var path = Path.Combine(Server.MapPath("~/Files"), fileName);

                fileInfo.SaveAs(path);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetUploader(RequiredDocumentModel document)
        {
            DocumentsService documentsService = new DocumentsService();
            // var fileLimits = documentsService.GetFileLimitations();
            decimal singleFileMax = 8 * 1024 * 1024;

            var uploaderModel = new UploaderViewModel()
            {
                Key = document.RequiredDocumentID,
                SelectionMaxSize = 5 * singleFileMax,
                SelectionTypeFilter = string.Join(",", "pdf", "tiff", "tif"),
                SelectionSaveUrl = string.Format("..{0}", Url.Action("UploadRequiredDocumentAjax", "Home")),
                SelectionSaveUrlLegacy = String.Format("..{0}", Url.Action("UploadRequiredDocument", "Home")),
                HasStored = document.HasFile,
                StoredDeleteAllowed = false,
                StoredDeleteHidden = true,
                StoredUrl = (document.HasFile ? document.FileURL : null),
                Heading = "The Heading File",
                FileCountExceededMessage = "Uploader Message File Count Exceeded",
                FileSizeExceededMessage = "Uploader Message File Size Exceeeded",
                FileTypeInvalidMessage = "Uploader Message File Type Invalid",
                LegacyBrowserInstruction = "Uploader Instruction LegacyBrowser",
                LegacyBrowserMessage = "Uploader Message LegacyBrowser",
                ServerResponseErrorMessage = "Uploader Message Server Response Error",
                RequestParameters = new Dictionary<string, object>()
                {
                    { "requiredDocumentID", document.RequiredDocumentID }
                }
            };

            return PartialView("_Uploader", uploaderModel);
        }

        [HttpPost]
        public FileStreamResult DownloadRequiredDocument(int requiredDocumentID)
        {
            DocumentsService documentService = new DocumentsService();
            return documentService.GetFileFromDisk(requiredDocumentID);
        }

        [HttpPost]
        public ActionResult UploadRequiredDocumentAjax(HttpPostedFileBase file, int requiredDocumentID)
        {
            if (requiredDocumentID < 1)
                throw new ArgumentException(" Failed to upload the file: incoming Required Document ID is not valid: got: " + requiredDocumentID);

            if (file == null)
                throw new ArgumentNullException("file", "null HttpPostedFileBase received at server");

            DocumentsService documentsService = new DocumentsService(Repository);

            string uploadMessage = null;
            
            string fileUrl = null;


            bool uploadSuccess = documentsService.UploadRequiredDocument(file, requiredDocumentID, 1, out uploadMessage, out fileUrl);

            var result = Json(new UploaderResponseModel()
            {
                Key = requiredDocumentID,

                FileUrl = ((fileUrl == null) ? String.Empty : string.Format("..{0}", fileUrl)),

                ResponseMessage = uploadMessage,

                Success = uploadSuccess
            });

            return result;
        }

        [HttpPost]
        public ActionResult UploadRequiredDocument(HttpPostedFileBase file, int requiredDocumentID)
        {

            if (requiredDocumentID < 1)
                throw new ArgumentException(" Failed to upload the file: incoming Required Document ID is not valid: got: " + requiredDocumentID);

            if (file == null)
                throw new ArgumentNullException("file", "null HttpPostedFileBase received at server");

            DocumentsService documentsService = new DocumentsService(Repository);

            String uploadMessage = null;

            String fileUrl = null;

            // RequiredDocumentModel result = 
            bool uploadSuccess = documentsService.UploadRequiredDocument(file, requiredDocumentID, 1, out uploadMessage, out fileUrl);

            if (uploadSuccess)
            {
                // reload the entire Required Docs page - will hide the Upload button, show the View button
                // may need to just reload the required docs div if it gets too heavy

                // will this work when called from the modal? Will it cancel the modal?
                return RedirectToAction("PendingBenefits", "Benefits");
            }
            else
            {
                // need to show this message in the upload modal
                throw new Exception(" Failed to upload: " + uploadMessage);
            }
        }

        [HttpGet]
        public ActionResult RequestAddRequiredDocument()
        {
            int memberID = 1;

            var addRequiredDocModel = new RequiredDocumentModel()
            {
                MemberID = memberID
            };

            return PartialView("_AddRequiredDoc", addRequiredDocModel);
        }

        [HttpPost]
        public ActionResult AddRequiredDocument(int memberID, string name, string description)
        {
            DocumentsService documentsService = new DocumentsService(Repository);

            string error = null;

            var status = documentsService.SaveRequiredDocumentation(memberID, 0, name, description, out error);

            if (status != 0)
                return RedirectToAction("PendingBenefits", "Benefits");

            throw new Exception("Failed to add request: " + error);
        }
    }
}