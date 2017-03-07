using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CsvUploaderApp.Models;
using CsvUploaderBusinessLayer;
using CsvUploaderBusinessLayer.Models;

namespace CsvUploaderApp.Controllers
{
    public class UploadCsvController : Controller
    {
        private readonly ICsvUploadManager _iCsvUploadManager;
      

        public UploadCsvController(ICsvUploadManager iCsvUploadManager)
        {
            this._iCsvUploadManager = iCsvUploadManager;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetCsvFileToDownLoad()
        {
            ViewBag.IsDownLoadAvailable = true;
            var model=_iCsvUploadManager.GetFiles().Select(file=> new CsvFileContentModel { Id=file.Id,FileName=file.FileName }).ToList();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FileUpload(HttpPostedFileBase fileBase)
        {
            try
            {
                ValidateFileName(fileBase);

                using (var reader = new BinaryReader(fileBase.InputStream))
                {
                   var content = reader.ReadBytes(fileBase.ContentLength);
                    _iCsvUploadManager.PersistUploadedCsvContent(fileBase.FileName,content);
                }

                return RedirectToAction("GetCsvFileToDownLoad");
            }
            catch (CsvContentException ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View("Index");
        }

        private static void ValidateFileName(HttpPostedFileBase fileBase)
        {
            //ToDo: To be refactored
            if (string.IsNullOrEmpty(fileBase?.FileName))
            {
                throw new CsvContentException("Please Select file to Upload");
            }
        }

        [HttpGet]
        public ActionResult DownLoad(int id)
        {
            var fileContent=_iCsvUploadManager.GetFile(id);
            return File(fileContent.Content, "text/csv", $"Report{fileContent.FileName}.csv");
        }
    }
}


