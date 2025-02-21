using Microsoft.EntityFrameworkCore;
using SmartHome.Common;
using SmartHome.Common.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Database.ApiContext
{
    public class LogContext
    {
        private readonly SmartHomeContext _dbContext;

        public LogContext(SmartHomeContext dbContext)
        {
            _dbContext = dbContext;
        }

       
        public async Task CreateLog(string Action, string Result)
        {
            

        }

    }
}
