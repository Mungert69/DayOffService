using DayOff.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DayOff.Models;
using Microsoft.AspNetCore.Hosting;
using DaysOff.Objects;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using Spire.Pdf;


namespace ASPNETCoreScheduler.Scheduler
{
    public class ScheduleTask : ScheduledProcessor
    {
        private  IHostingEnvironment _hostingEnvironment;
        private  DayOffContext _context;

        public ScheduleTask(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        protected override string Schedule => "0 12 * * *";

        public override Task ProcessInScope(IServiceProvider serviceProvider)
        {
            _context =serviceProvider.GetService<DayOffContext>();
            _hostingEnvironment = serviceProvider.GetService<IHostingEnvironment>();
            RotaDay rotaDay = new RotaDay();
            rotaDay.RotaDate = DateTime.Now.AddDays(1).Date;
            Export(rotaDay);
            Console.WriteLine("Processing starts here");
            return Task.CompletedTask;
        }

        public void Export(RotaDay rotaDay)
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

            Spire.Pdf.PdfDocument pdfdocument = new Spire.Pdf.PdfDocument();
            // PaperSize paper = new PaperSize("A4", (int)PdfPageSize.A4.Width, (int)PdfPageSize.A4.Height);
            // paper.RawKind = (int)PaperKind.A4;

            pdfdocument.LoadFromFile(Path.Combine(sWebRootFolder, pFileName));
            pdfdocument.PrintSettings.PrinterName = "Xerox VersaLink C405 (64:92:59)";
            //pdfdocument.PrintSettings.PaperSize = paper;
            pdfdocument.PageScaling = PdfPrintPageScaling.FitSize;
            pdfdocument.Print();
            pdfdocument.Dispose();
         
        }
    }
}
