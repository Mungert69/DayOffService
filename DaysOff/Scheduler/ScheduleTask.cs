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
            string dishesSupervisors = rotaCreator.getDishesSupervisors(checkDate);
            string rotaMeetingUsers = rotaCreator.getRotaMeetingUsers(checkDate);

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
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Rota");

                worksheet.Column(1).Width = 17;
                worksheet.Column(2).Width = 17;
                worksheet.Column(3).Width = 17;
                worksheet.Column(4).Width = 17;
                worksheet.Column(5).Width = 17;
                worksheet.Column(6).Width = 17;
                worksheet.Column(7).Width = 17;
                worksheet.Column(8).Width = 17;

                //First add the headers
                worksheet.Cells[1, 1].Value = "  " + checkDate.DayOfWeek;
                worksheet.Cells[1, 2].Value = checkDate.ToLongDateString();

                worksheet.Cells[1, 3].Value = " OVER ROTA";
                worksheet.Cells[1, 6].Value = " LOCK UP";
                worksheet.Cells[2, 1].Value = "  OFF  ";
                worksheet.Cells[2, 2].Value = "  ON  ";
                worksheet.Cells[2, 3].Value = "  HOUSECARE  ";
                worksheet.Cells[2, 4].Value = "  DEBOP  ";
                worksheet.Cells[2, 5].Value = "  GROUNDS  ";
                worksheet.Cells[2, 6].Value = "OWN JOBS";
                worksheet.Cells[2, 7].Value = "  OFFICE  ";
                worksheet.Cells[2, 8].Value = "  KITCHEN  ";
                worksheet.Cells["A1:H1"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:H1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                worksheet.Cells["A2:H2"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A2:H2"].Style.Font.Bold = true;
                worksheet.Cells["A1:H1"].Style.Font.Bold = true;
                worksheet.Cells["A2:H2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A2:H2"].Style.Font.Size = 14;
                worksheet.Cells["A1:H1"].Style.Font.Size = 14;

                worksheet.Cells["A1:A34"].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                worksheet.Cells["A2:A34"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["B1:B34"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["C2:C34"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["D2:D34"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["E1:E34"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["F2:F34"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["G2:G34"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["H1:H34"].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                worksheet.Cells["C19:H19"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A34:H34"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A35"].Value = "Dishes ";
                worksheet.Cells["B35"].Value = dishesSupervisors;
                worksheet.Cells["E35"].Value = "Rota ";
                worksheet.Cells["F35"].Value = rotaMeetingUsers;


                int durationOffset = 16;
                int startRowAm = 3;
                int startRowPm = 3 + durationOffset;
                Dictionary<string, int> rowDic = new Dictionary<string, int>();
                rowDic.Add("rowAmC", startRowAm);
                rowDic.Add("rowPmC", startRowPm);
                rowDic.Add("rowAmD", startRowAm);
                rowDic.Add("rowPmD", startRowPm);
                rowDic.Add("rowAmE", startRowAm);
                rowDic.Add("rowPmE", startRowPm);
                rowDic.Add("rowAmF", startRowAm);
                rowDic.Add("rowPmF", startRowPm);
                rowDic.Add("rowAmG", startRowAm);
                rowDic.Add("rowPmG", startRowPm);
                rowDic.Add("rowAmH", startRowAm);
                rowDic.Add("rowPmH", startRowPm);

                string cellLoc = "";
                string keyString = "";
                string valueString = "";
                foreach (WorkBase workBase in rotaCreator.WorkBases)
                {
                    keyString = "row" + workBase.DurationString() + workBase.ExcelCol();
                    cellLoc = workBase.ExcelCol() + rowDic[keyString].ToString();
                    rowDic[keyString] = rowDic[keyString] + 1;
                    valueString = workBase.UserName;
                    if (workBase.ExcelCol() == "F" || workBase.WorkType == 0 || workBase.WorkType == (WorkTypes)2) { valueString = workBase.UserName + " - " + workBase.WorkType.ToString(); }
                    worksheet.Cells[cellLoc].Value = valueString;
                }

                //Add day off on
                int col = 1;
                int rowStart = startRowAm;
                int rowLeft = rowStart;
                int rowRight = rowStart;
                foreach (UserRota user in rotaCreator.Users)
                {
                    if (user.IsAmOff && user.IsPmOff)
                    {
                        col = 1;
                        worksheet.Cells[rowLeft, col].Value = user.FirstName;
                        rowLeft++;
                        continue;
                    }

                    if (!user.IsAmOff && !user.IsPmOff)
                    {
                        col = 2;
                        worksheet.Cells[rowRight, col].Value = user.FirstName;
                        rowRight++;
                        continue;
                    }

                    if (user.IsAmOff)
                    {
                        col = 1;
                        worksheet.Cells[rowLeft, col].Value = user.FirstName + " AM";
                        rowLeft++;
                        col = 2;
                        worksheet.Cells[rowRight, col].Value = user.FirstName + " PM";
                        rowRight++;
                    }
                    if (user.IsPmOff)
                    {
                        col = 1;
                        worksheet.Cells[rowLeft, col].Value = user.FirstName + " PM";
                        rowLeft++;
                        col = 2;
                        worksheet.Cells[rowRight, col].Value = user.FirstName + " AM";
                        rowRight++;
                    }


                }



                worksheet.PrinterSettings.FitToPage = true;
                worksheet.PrinterSettings.FitToHeight = 1;
                worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                worksheet.PrinterSettings.FooterMargin = 0.5M;
                worksheet.PrinterSettings.TopMargin = .5M;
                worksheet.PrinterSettings.LeftMargin = .5M;
                worksheet.PrinterSettings.RightMargin = .5M;
                worksheet.PrinterSettings.PaperSize = ePaperSize.A4;
                package.Save(); //Save the workbook.
                Spire.Xls.Workbook workbook = new Spire.Xls.Workbook();
                workbook.LoadFromFile(Path.Combine(sWebRootFolder, sFileName), true);
                workbook.SaveToFile(Path.Combine(sWebRootFolder, pFileName), Spire.Xls.FileFormat.PDF);

            }
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
