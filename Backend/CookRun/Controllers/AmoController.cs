using CookRun.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoistatApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AmoController : ControllerBase
    {
        private readonly ILogger<AmoController> _logger;
        private readonly ApplicationContext _applicationContext;

        public AmoController(ILogger<AmoController> logger, ApplicationContext applicationContext)
        {
            _logger = logger;
            _applicationContext = applicationContext;
        }

        [HttpGet("GetLeads")]
        public List<AmoLeadReport> GetLeads()
        {
            var jobReport = _applicationContext.JobReports
                .AsNoTracking()
                .Where(x => x.Name == "AmoLeadReport")
                .OrderByDescending(x => x.DateTime)
                .FirstOrDefault();

            if (jobReport != null)
            {
                var amoLeadReports = _applicationContext.AmoLeadReports
                    .AsNoTracking()
                    .Include(x => x.Tags)
                    .Where(x => x.JobReportId == jobReport.Id)
                    .ToList();

                return amoLeadReports;
            }

            return null;
        }

        [HttpGet("GetAmoSummReports")]
        public List<AmoSummReport> GetAmoSummReports()
        {
            var todayReports = _applicationContext.AmoDayReports
                .AsNoTracking()
                .Where(x => x.Date.Date == DateTime.Today.AddDays(-1))
                .ToList();

            var yesterdayReports = _applicationContext.AmoDayReports
                .AsNoTracking()
                .Where(x => x.Date.Date == DateTime.Today.AddDays(-2))
                .ToList();

            var monthDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var monthReports = _applicationContext.AmoMonthReports
                .AsNoTracking()
                .Where(x => x.Date.Date == monthDateTime)
                .ToList();

            var amoSummReports = new List<AmoSummReport>();
            foreach (var todayReport in todayReports)
            {
                var amoSummReport = new AmoSummReport();
                amoSummReport.Project = todayReport.ProjectName;
                amoSummReport.TodayLeads = todayReport.Leads;
                //amoSummReport.TodayClosedLeads = todayReport.Closed;
                amoSummReport.TodayOverdue = todayReport.Overdue;
                amoSummReport.TodayLeadsWithoutTasks = todayReport.LeadsWithoutTasks;

                var monthReport = monthReports.FirstOrDefault(x => x.ProjectName == todayReport.ProjectName);
                if (monthReport != null)
                {
                    amoSummReport.MonthLeads = monthReport.Leads;
                    //amoSummReport.MonthClosedLeads = monthReport.Closed;
                    amoSummReport.MonthSales = monthReport.Sales;
                    amoSummReport.MonthCorrectLeads = monthReport.CorrectLeads;

                }

                var yesterdayReport = yesterdayReports.FirstOrDefault(x => x.ProjectName == todayReport.ProjectName);
                /*if (monthReport != null)
                {
                    amoSummReport.OverdueTrend = todayReport.Overdue - yesterdayReport.Overdue;
                    if (amoSummReport.OverdueTrend != 0 && amoSummReport.TodayOverdue != 0)
                        amoSummReport.OverdueTrendPercents = (int)((decimal)amoSummReport.OverdueTrend / (decimal)amoSummReport.TodayOverdue * (decimal)100);
                    amoSummReport.LeadsWithoutTasksTrend = todayReport.LeadsWithoutTasks - yesterdayReport.LeadsWithoutTasks;
                    if (amoSummReport.LeadsWithoutTasksTrend != 0 && amoSummReport.TodayLeadsWithoutTasks != 0)
                        amoSummReport.LeadsWithoutTasksTrendPercents = (int)((decimal)amoSummReport.LeadsWithoutTasksTrend / (decimal)amoSummReport.TodayLeadsWithoutTasks * (decimal)100);
                }*/
                if (amoSummReport.MonthCorrectLeads != 0) 
                    amoSummReport.Conversion = 100 * (decimal)amoSummReport.MonthSales / (decimal)amoSummReport.MonthCorrectLeads;

                amoSummReports.Add(amoSummReport);
            }
            return amoSummReports;
        }
    }

    public class AmoSummReport
    {
        public string Project { get; set; }
        public int TodayLeads { get; set; }
        public int MonthLeads { get; set; }
        public int MonthCorrectLeads { get; set; }
        public int TodayOverdue { get; set; }
        public int TodayLeadsWithoutTasks { get; set; }
        public int TodayForehead { get; set; }
        public int TodaySales { get; set; }
        public int MonthSales { get; set; }
        //public int TodayClosedLeads { get; set; }
        //public int MonthClosedLeads { get; set; }
        //public int OverdueTrend { get; set; }
        //public int OverdueTrendPercents { get; set; }
        //public int LeadsWithoutTasksTrend { get; set; }
        //public int LeadsWithoutTasksTrendPercents { get; set; }
        public decimal Conversion { get; set; }
    }

    public class AmoFullReport
	{
        public string Project { get; set; }
        public int TodayLeads { get; set; }
        public int MonthLeads { get; set; }
        public int MonthCorrectLeads { get; set; }
        public int TodayOverdue { get; set; }
        public int TodayLeadsWithoutTasks { get; set; }
        public int TodayForehead { get; set; }
        public int TodaySales { get; set; }
        public int MonthSales { get; set; }
        public decimal Conversion { get; set; }
    }

    public class AmoSalesReport
	{
        public string Project { get; set; }
        public int TodayForehead { get; set; }
        public int TodaySales { get; set; }
        public int MonthSales { get; set; }
        public int MonthCorrectLeads { get; set; }
        public decimal Conversion { get; set; }
    }
}