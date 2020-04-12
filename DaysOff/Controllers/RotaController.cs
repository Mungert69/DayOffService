using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DayOff.Data;
using DayOff.Models;
using Microsoft.AspNetCore.Hosting;
using DaysOff.Objects;
using System.Net.Http.Headers;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using Spire.Pdf;

namespace DaysOff.Controllers
{
    public class RotaController : Controller
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly DayOffContext _context;

        public RotaController(DayOffContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;

        }

        // GET: RotaDays
        public async Task<IActionResult> Index()
        {
            RotaDay rotaDay = new RotaDay();
            rotaDay.RotaDate = DateTime.Now.AddDays(1).Date;
            return View(rotaDay);
        }

        // POST: RotaDays/Print/03-02-2020
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Print([Bind("RotaID,UserID,RotaDate")] RotaDay rotaDay)
        {

            var result = Export(rotaDay);
            return result;
        }



        public IActionResult Export(RotaDay rotaDay)
        {
            DateTime checkDate = rotaDay.RotaDate.Date;
            RotaCreator rotaCreator = new RotaCreator(_context);
            rotaCreator.init(checkDate);
           

            string sWebRootFolder = _hostingEnvironment.ContentRootPath;
            string sFileName = @"Rota-" + checkDate.ToLongDateString() + ".xlsx";
            string pFileName = @"Rota-" + checkDate.ToLongDateString() + ".pdf";
            //string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, pFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            FileInfo pfile = new FileInfo(Path.Combine(sWebRootFolder, pFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            rotaCreator.createXLSX(file, checkDate);

            Spire.Xls.Workbook workbook = new Spire.Xls.Workbook();
            workbook.LoadFromFile(Path.Combine(sWebRootFolder, sFileName), true);
            workbook.SaveToFile(Path.Combine(sWebRootFolder, pFileName), Spire.Xls.FileFormat.PDF);

            var result = PhysicalFile(Path.Combine(sWebRootFolder, pFileName), "application/pdf");

            Spire.Pdf.PdfDocument pdfdocument = new Spire.Pdf.PdfDocument();
           // PaperSize paper = new PaperSize("A4", (int)PdfPageSize.A4.Width, (int)PdfPageSize.A4.Height);
           // paper.RawKind = (int)PaperKind.A4;
           
            pdfdocument.LoadFromFile(Path.Combine(sWebRootFolder, pFileName));
            pdfdocument.PrintSettings.PrinterName = "Xerox VersaLink C405 (64:92:59)";
            //pdfdocument.PrintSettings.PaperSize = paper;
            pdfdocument.PageScaling = PdfPrintPageScaling.FitSize;
            pdfdocument.Print();
            pdfdocument.Dispose();
            // initialize PrintDocument object
            /*PrintDocument doc = new PrintDocument()
            {
                PrinterSettings = new PrinterSettings()
                {
                    // set the printer to 'Microsoft Print to PDF'
                    PrinterName = "Xerox VersaLink C405 (64:92:59)",


                    // set the filename to whatever you like (full path)
                    PrintFileName = (Path.Combine(sWebRootFolder, pFileName))
                }
            };
            doc.Print();*/
            Response.Headers["Content-Disposition"] = new ContentDispositionHeaderValue("attachment")
            {
                FileName = pfile.Name
            }.ToString();

            return result;
        }
    }

}
