using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DayOff.Data;
using DaysOff.Models;
using Microsoft.AspNetCore.Hosting;
using DaysOff.Objects;
using System.Net.Http.Headers;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using DaysOff.Utils;

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


            string sWebRootFolder = _hostingEnvironment.ContentRootPath;
            string sFileName = @"demo.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Rota");

                worksheet.Column(1).Width = 16;
                worksheet.Column(2).Width = 16;
                worksheet.Column(3).Width = 16;
                worksheet.Column(4).Width = 16;
                worksheet.Column(5).Width = 15;
                worksheet.Column(6).Width = 16;
                worksheet.Column(7).Width = 15;
                worksheet.Column(8).Width = 15;

                //First add the headers
                worksheet.Cells[1, 1].Value = "  " + checkDate.DayOfWeek;
                worksheet.Cells[1, 2].Value = checkDate.ToShortDateString();

                worksheet.Cells[1, 3].Value = " OVER ROTA";
                worksheet.Cells[1, 6].Value = " LOCK UP";
                worksheet.Cells[2, 1].Value = "  OFF  ";
                worksheet.Cells[2, 2].Value = "  ON  ";
                worksheet.Cells[2, 3].Value = "  HOUSECARE  ";
                worksheet.Cells[2, 4].Value = "  DEBOP  ";
                worksheet.Cells[2, 5].Value = "  GROUNDS  ";
                worksheet.Cells[2, 6].Value = "OWN JOBS";
                //worksheet.Cells[2, 6].Style.WrapText = true;
                worksheet.Cells[2, 7].Value = "  OFFICE  ";
                worksheet.Cells[2, 8].Value = "  KITCHEN  ";
                worksheet.Cells["A1:H1"].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                worksheet.Cells["A1:H1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;

                worksheet.Cells["A2:H2"].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                worksheet.Cells["A2:H2"].Style.Font.Bold = true;
                worksheet.Cells["A1:H1"].Style.Font.Bold = true;
                worksheet.Cells["A2:H2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A2:H2"].Style.Font.Size = 14;
                worksheet.Cells["A1:H1"].Style.Font.Size = 14;

                worksheet.Cells["A1:A34"].Style.Border.Left.Style = ExcelBorderStyle.Thick;

                worksheet.Cells["A2:A34"].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                worksheet.Cells["B1:B34"].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                worksheet.Cells["C2:C34"].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                worksheet.Cells["D2:D34"].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                worksheet.Cells["E1:E34"].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                worksheet.Cells["F2:F34"].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                worksheet.Cells["G2:G34"].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                worksheet.Cells["H1:H34"].Style.Border.Right.Style = ExcelBorderStyle.Thick;

                worksheet.Cells["C19:H19"].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                worksheet.Cells["A34:H34"].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;

                List<WorkBase> workBases = DataBaseHelper.getWorkDay(checkDate, checkDate, _context);


                List<IUserBase> usersBases = DataBaseHelper.getActiveUsers(checkDate, checkDate, _context);
                List<UserRota> users = new List<UserRota>();
                foreach (UserBase userBase in usersBases)
                {
                    UserRota user = new UserRota();
                    user.FirstName = userBase.FirstName;
                    user.ID = userBase.ID;
                    int count = _context.Holidays.Where(h => h.UserID == userBase.ID && (h.Duration == 0) && h.HolDate == checkDate).Count();
                    if (count > 0) { user.IsAmOff = true; }
                    else { user.IsAmOff = false; }
                    count = _context.Holidays.Where(h => h.UserID == userBase.ID && h.Duration == (Durations)1 && h.HolDate == checkDate).Count();
                    if (count > 0) { user.IsPmOff = true; }
                    else { user.IsPmOff = false; }


                    users.Add(user);
                }

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
                foreach (WorkBase workBase in workBases)
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
                foreach (UserRota user in users)
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
                // Quick fix
                if (rowLeft == rowStart)
                {
                    worksheet.Cells[rowLeft, 1].Value = "___________";

                }




                worksheet.View.PageLayoutView = true;
                worksheet.PrinterSettings.Orientation = eOrientation.Landscape;

                package.Save(); //Save the workbook.
            }
            var result = PhysicalFile(Path.Combine(sWebRootFolder, sFileName), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            /*PdfDocument pdfdocument = new PdfDocument();
            pdfdocument.LoadFromFile("C:\\temp\\test.pdf");
            pdfdocument.PrintSettings.PrinterName = "Xerox VersaLink C405 (64:92:59)";
            pdfdocument.PrintSettings.Copies = 1;
            pdfdocument.Print();
            pdfdocument.Dispose();*/
            Response.Headers["Content-Disposition"] = new ContentDispositionHeaderValue("attachment")
            {
                FileName = file.Name
            }.ToString();

            return result;
        }
    }
    
}
