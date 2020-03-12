using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DayOff.Data;
using DaysOff.Objects;
using DaysOff.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace DaysOff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportExcelController : ControllerBase
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly DayOffContext _context;
        private int rowHC = 3;
        private int rowDB = 3;
        private int rowGR = 3;
        private int rowOJ = 3;
        private int rowOF = 3;
        private int rowKI = 3;
        Dictionary<string, string> workStrings = new Dictionary<string, string>();
        public ExportExcelController(DayOffContext context,IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;

        }

        [HttpGet("Export/{checkDateStr}")]
        public IActionResult Export([FromRoute] string checkDateStr)
        {
            DateTime checkDate;
            try { checkDate= DateTime.Parse(checkDateStr).Date; }
            catch (Exception e) {
                checkDate = DateTime.Now.Date;
            }
            
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
               
                //First add the headers
                worksheet.Cells[1, 1].Value = " DATE";
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

               
                List<IUserBase> usersBases = DataBaseHelper.getActiveUsers(checkDate,checkDate,_context);
                List<UserRota> users = new List<UserRota>();
                foreach (UserBase userBase in usersBases)
                {
                    UserRota user = new UserRota();
                    user.FirstName = userBase.FirstName;
                    user.ID = userBase.ID;
                    int count=_context.Holidays.Where(h => h.UserID == userBase.ID && (h.Duration == 0) && h.HolDate==checkDate).Count();
                    if (count > 0) { user.IsAmOff = true; }
                    else { user.IsAmOff = false; }
                    count = _context.Holidays.Where(h => h.UserID == userBase.ID && h.Duration == (Durations)1 && h.HolDate == checkDate).Count();
                    if (count > 0) { user.IsPmOff = true; }
                    else { user.IsPmOff = false; }
                    
                    
                    users.Add(user);
                }

                int durationOffset = 16;
                int startRowAm = 3;
                int startRowPm = 3+durationOffset;
                Dictionary<string, int> rowDic = new Dictionary<string, int>();
                rowDic.Add("rowAmC", startRowAm);
                rowDic.Add("rowPmC" , startRowPm);
                rowDic.Add("rowAmD" , startRowAm);
                rowDic.Add("rowPmD" , startRowPm);
                rowDic.Add("rowAmE" , startRowAm);
                rowDic.Add("rowPmE" , startRowPm);
                rowDic.Add("rowAmF" , startRowAm);
                rowDic.Add("rowPmF" ,startRowPm);
                rowDic.Add("rowAmG" ,startRowAm);
                rowDic.Add("rowPmG" ,startRowPm);
                rowDic.Add("rowAmH" , startRowAm);
                rowDic.Add("rowPmH" , startRowPm);

                string cellLoc = "";
                string keyString = "";
                string valueString = "";
                foreach (WorkBase workBase in workBases)
                {
                    keyString = "row" + workBase.DurationString() + workBase.ExcelCol();
                    cellLoc = workBase.ExcelCol() + rowDic[keyString].ToString();
                    rowDic[keyString] = rowDic[keyString] + 1;
                    valueString = workBase.UserName;
                    if (workBase.ExcelCol() == "F" || workBase.WorkType == 0) { valueString = workBase.UserName + " - " + workBase.WorkType.ToString(); }
                    worksheet.Cells[cellLoc].Value =valueString;
                }

                //Add day off on
                int col = 1;
                int rowStart = startRowAm;
                int rowLeft = rowStart;
                int rowRight = rowStart;
                foreach (UserRota user in users) {
                    if (user.IsAmOff && user.IsPmOff) {
                        col = 1;
                        worksheet.Cells[rowLeft, col].Value = user.FirstName;
                        rowLeft++;
                        continue ;
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
                        worksheet.Cells[rowLeft, col].Value = user.FirstName+ " AM";
                        rowLeft++;
                        col = 2;
                        worksheet.Cells[rowRight, col].Value = user.FirstName + " PM";
                        rowRight++;
                    }
                    if (user.IsPmOff) {
                        col = 1;
                        worksheet.Cells[rowLeft, col].Value = user.FirstName+ " PM";
                        rowLeft++;
                        col = 2;
                         worksheet.Cells[rowRight, col].Value = user.FirstName + " AM";
                        rowRight++;
                    }

                  
                }



                worksheet.Cells.AutoFitColumns(0);

                worksheet.View.PageLayoutView = true;
                worksheet.PrinterSettings.Orientation = eOrientation.Landscape;

                package.Save(); //Save the workbook.
            }
            var result = PhysicalFile(Path.Combine(sWebRootFolder, sFileName), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            Response.Headers["Content-Disposition"] = new ContentDispositionHeaderValue("attachment")
            {
                FileName = file.Name
            }.ToString();

            return result;
        }

    }

}
