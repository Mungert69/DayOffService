using DayOff.Data;
using DayOff.Models;
using DaysOff.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

namespace DaysOff.Objects
{
    public class RotaCreator
    {
        private List<WorkBase> workBases;
        private List<IUserBase> usersBases;
        private List<UserRota> users;
        private DayOffContext _context;

        public RotaCreator(DayOffContext context)
        {
            _context = context;

        }
        private List<WorkBase> WorkBases { get => workBases; set => workBases = value; }
        private List<UserRota> Users { get => users; set => users = value; }

        private string getRotaMeetingUsers(DateTime rotaDate)
        {

            List<DayOff.Models.RotaDay> rotaMeetingDays = _context.RotaDays.Where(d => d.RotaDate == rotaDate).ToList();
            _context.RemoveRange(rotaMeetingDays);
            _context.SaveChanges();

            List<UserRota> rotas = new List<UserRota>();

            foreach (UserRota user in users.Where(u => u.UserType == UserBase.UserTypes.TLP).ToList())
            {

                user.DishCount = _context.RotaDays.Where(d => d.UserID == user.ID && d.RotaDate > rotaDate.AddDays(-14)).Count();
                try
                {
                    List<RotaDay> countRotas = _context.RotaDays.Where(d => d.UserID == user.ID).OrderByDescending(d => d.RotaDate).ToList();

                    user.LastDate = countRotas[0].RotaDate;
                    try
                    {
                        user.LastButOneDate = countRotas[1].RotaDate;
                    }
                    catch (Exception e)
                    {
                        // just continue as this user has not done the dishes twice
                    }
                    try
                    {
                        user.LastButTwoDate = countRotas[2].RotaDate;
                    }
                    catch (Exception e)
                    {
                        // just continue as this user has not done the dishes twice
                    }
                    try
                    {
                        user.LastButThreeDate = countRotas[3].RotaDate;
                    }
                    catch (Exception e)
                    {
                        // just continue as this user has not done the dishes twice
                    }


                }
                catch (Exception e)
                {
                    // just continue as this user has not done the dishes before
                }
                rotas.Add(user);
            }

            List<UserRota> sortedList = rotas.OrderBy(d => d.LastDate).ThenBy(d => d.LastButOneDate).ThenBy(d => d.LastButTwoDate).ThenBy(d => d.LastButThreeDate).ThenBy(d => d.DishCount).ThenByDescending(d => d.ID).ToList();
            RotaDay rotaDay;
            string result = "";
            int count = 3;

            // First add OHC morning user
            foreach (UserRota user in sortedList)
            {
                if (!user.IsAmOff && user.AmWorkType == WorkTypes.OHC)
                {
                    result += user.FirstName + " ";
                    rotaDay = new RotaDay();
                    rotaDay.UserID = user.ID;
                    rotaDay.RotaDate = rotaDate;
                    _context.Add(rotaDay);
                    _context.SaveChanges();
                    sortedList.Remove(user);
                    count--;
                    break;
                }
            }

            //Then add other users
            foreach (UserRota user in sortedList)
            {
                if (!user.IsAmOff)
                {
                    result += user.FirstName + " ";
                    rotaDay = new RotaDay();
                    rotaDay.UserID = user.ID;
                    rotaDay.RotaDate = rotaDate;
                    _context.Add(rotaDay);
                    _context.SaveChanges();
                    count--;
                    if (count == 0) break;
                }
            }

            return result;
        }

        private string getDishesSupervisors(DateTime dishDate)
        {

            DayOff.Models.DishDay dishDay;
            List<DayOff.Models.DishDay> dishDays = _context.DishDays.Where(d => d.DishDate == dishDate).ToList();
            _context.RemoveRange(dishDays);
            _context.SaveChanges();

            List<UserRota> rotas = new List<UserRota>();

            foreach (UserRota user in users.Where(u => u.UserType == UserBase.UserTypes.TLP).ToList())
            {

                user.DishCount = _context.DishDays.Where(d => d.UserID == user.ID && d.DishDate > dishDate.AddDays(-14)).Count();
                try
                {
                    List<DishDay> countDishes = _context.DishDays.Where(d => d.UserID == user.ID).OrderByDescending(d => d.DishDate).ToList();

                    user.LastDate = countDishes[0].DishDate;
                    try
                    {
                        user.LastButOneDate = countDishes[1].DishDate;
                    }
                    catch (Exception e)
                    {
                        // just continue as this user has not done the dishes twice
                    }
                    try
                    {
                        user.LastButTwoDate = countDishes[2].DishDate;
                    }
                    catch (Exception e)
                    {
                        // just continue as this user has not done the dishes twice
                    }
                    try
                    {
                        user.LastButThreeDate = countDishes[3].DishDate;
                    }
                    catch (Exception e)
                    {
                        // just continue as this user has not done the dishes twice
                    }


                }
                catch (Exception e)
                {
                    // just continue as this user has not done the dishes before
                }
                rotas.Add(user);
            }

            List<UserRota> sortedList = rotas.OrderBy(d => d.LastDate).ThenBy(d => d.LastButOneDate).ThenBy(d => d.LastButTwoDate).ThenBy(d => d.LastButThreeDate).ThenBy(d => d.DishCount).ThenByDescending(d => d.ID).ToList();



            string amResult = "AM : ";
            string pmResult = "PM : ";
            List<int> amUsers = new List<int>();
            int amCount = 2;
            int pmCount = 2;
            foreach (UserRota user in sortedList)
            {
                if (!user.IsAmOff && amCount > 0 && user.AmWorkType != WorkTypes.MC)
                {
                    amCount--;
                    amResult += user.FirstName + " ";
                    dishDay = new DishDay();
                    dishDay.UserID = user.ID;
                    dishDay.DishDate = dishDate;
                    dishDay.Duration = 0;
                    amUsers.Add(user.ID);

                    _context.Add(dishDay);
                    _context.SaveChanges();

                }
                if (!user.IsPmOff && pmCount > 0 && user.PmWorkType != WorkTypes.MC && !amUsers.Contains(user.ID))
                {
                    pmCount--;
                    pmResult += user.FirstName + " ";
                    dishDay = new DishDay();
                    dishDay.UserID = user.ID;
                    dishDay.DishDate = dishDate;
                    dishDay.Duration = (Durations)1;

                    _context.Add(dishDay);
                    _context.SaveChanges();

                }
            }

            return amResult + " - " + pmResult;
        }

        public void init(DateTime checkDate)
        {
            workBases = DataBaseHelper.getWorkDay(checkDate, checkDate, _context);

            // We only print TLPs for now.
            usersBases = DataBaseHelper.getActiveUsers(checkDate, checkDate, _context);
            users = new List<UserRota>();
            foreach (UserBase userBase in usersBases)
            {
                UserRota user = new UserRota();
                user.FirstName = userBase.FirstName;
                user.ID = userBase.ID;
                user.UserType = userBase.UserType;
                int count = _context.Holidays.Where(h => h.UserID == userBase.ID && (h.Duration == 0) && h.HolDate == checkDate).Count();
                if (count > 0) { user.IsAmOff = true; }
                else { user.IsAmOff = false; }
                count = _context.Holidays.Where(h => h.UserID == userBase.ID && h.Duration == (Durations)1 && h.HolDate == checkDate).Count();
                if (count > 0) { user.IsPmOff = true; }
                else { user.IsPmOff = false; }
                WorkBase amWorkBase = workBases.Where(w => w.Duration == 0 && w.UserID == user.ID).FirstOrDefault();
                WorkBase pmWorkBases = workBases.Where(w => w.Duration == (Durations)1 && w.UserID == user.ID).FirstOrDefault();
                if (amWorkBase != null) { user.AmWorkType = amWorkBase.WorkType; }
                if (pmWorkBases != null) { user.PmWorkType = pmWorkBases.WorkType; }



                users.Add(user);
            }

        }

        public void createXLSX(FileInfo file, DateTime checkDate)
        {
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
                worksheet.Cells["B35"].Value = getDishesSupervisors(checkDate);
                worksheet.Cells["E35"].Value = "Rota ";
                worksheet.Cells["F35"].Value = getRotaMeetingUsers(checkDate);


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
                foreach (WorkBase workBase in WorkBases)
                {
                    try
                    {
                        keyString = "row" + workBase.DurationString() + workBase.ExcelCol();
                        cellLoc = workBase.ExcelCol() + rowDic[keyString].ToString();
                        rowDic[keyString] = rowDic[keyString] + 1;
                        valueString = workBase.UserName;
                        if (workBase.ExcelCol() == "F" || workBase.WorkType == 0 || workBase.WorkType == (WorkTypes)2) { valueString = workBase.UserName + " - " + workBase.WorkType.ToString(); }
                        worksheet.Cells[cellLoc].Value = valueString;
                    }
                    catch (Exception e)
                    {
                        // Just skip if workType has no corresponding excel cell.
                    }

                    //Add day off on
                    int col = 1;
                    int rowStart = startRowAm;
                    int rowLeft = rowStart;
                    int rowRight = rowStart;
                    foreach (UserRota user in Users)
                    {
                        if (user.UserType == UserBase.UserTypes.Core) continue;
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

                }
            }
        }
    }
}
