using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysOff.ExtensionMethods
{
    

        public static class EntityExtensions
        {
            public static void Clear<T>(this DbSet<T> dbSet) where T : class
            {
                dbSet.RemoveRange(dbSet);
            }
        }
    
}
