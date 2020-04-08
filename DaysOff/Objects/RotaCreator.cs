using DayOff.Data;
using DayOff.Models;
using DaysOff.Models;
using DaysOff.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public List<WorkBase> WorkBases { get => workBases; set => workBases = value; }
        public List<UserRota> Users { get => users; set => users = value; }

        public string getRotaMeetingUsers(DateTime rotaDate)
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
            foreach (UserRota user in sortedList)
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

            return result;
        }

        public string getDishesSupervisors(DateTime dishDate)
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
    }
}
