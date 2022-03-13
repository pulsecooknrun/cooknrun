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

        /*
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
                if (monthReport != null)
                {
                    amoSummReport.OverdueTrend = todayReport.Overdue - yesterdayReport.Overdue;
                    if (amoSummReport.OverdueTrend != 0 && amoSummReport.TodayOverdue != 0)
                        amoSummReport.OverdueTrendPercents = (int)((decimal)amoSummReport.OverdueTrend / (decimal)amoSummReport.TodayOverdue * (decimal)100);
                    amoSummReport.LeadsWithoutTasksTrend = todayReport.LeadsWithoutTasks - yesterdayReport.LeadsWithoutTasks;
                    if (amoSummReport.LeadsWithoutTasksTrend != 0 && amoSummReport.TodayLeadsWithoutTasks != 0)
                        amoSummReport.LeadsWithoutTasksTrendPercents = (int)((decimal)amoSummReport.LeadsWithoutTasksTrend / (decimal)amoSummReport.TodayLeadsWithoutTasks * (decimal)100);
                }

                if (amoSummReport.MonthCorrectLeads != 0) 
                    amoSummReport.Conversion = 100 * (decimal)amoSummReport.MonthSales / (decimal)amoSummReport.MonthCorrectLeads;

                amoSummReports.Add(amoSummReport);
            }
            return amoSummReports;
        }
        */

        [HttpGet("GetAmoFullReports")]
        public List<AmoFullReport> GetAmoFullReports()
        {
            var todayReports = _applicationContext.AmoDayReports
                .AsNoTracking()
                .Where(x => x.Date.Date == DateTime.Today.AddDays(-1))
                .ToList();

            var monthDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var monthReports = _applicationContext.AmoMonthReports
                .AsNoTracking()
                .Where(x => x.Date.Date == monthDateTime)
                .ToList();

            var amoFullReports = new List<AmoFullReport>();
            foreach (var todayReport in todayReports)
            {
                var amoFullReport = new AmoFullReport();
                amoFullReport.Project = todayReport.ProjectName;
                amoFullReport.TodayLeads = todayReport.Leads;
                amoFullReport.TodayOverdue = todayReport.Overdue;
                amoFullReport.TodayLeadsWithoutTasks = todayReport.LeadsWithoutTasks;
                amoFullReport.TodayForehead = todayReport.Forehead;
                amoFullReport.TodaySales = todayReport.Sales;

                var monthReport = monthReports.FirstOrDefault(x => x.ProjectName == todayReport.ProjectName);

                if (monthReport != null)
                {
                    amoFullReport.MonthLeads = monthReport.Leads;
                    amoFullReport.MonthSales = monthReport.Sales;
                    amoFullReport.MonthCorrectLeads = monthReport.CorrectLeads;
                    amoFullReport.GamesSoldThisMonth = monthReport.GamesSoldThisMonth;
                    amoFullReport.AmountOfSalesThisMonth = monthReport.AmountOfSalesThisMonth;
                    amoFullReport.GameCompletedThisMonth = monthReport.GameCompletedThisMonth;
                    amoFullReport.GamesSoldNextMonth = monthReport.GamesSoldNextMonth;
                    amoFullReport.AmountOfSalesNextMonth = monthReport.AmountOfSalesNextMonth;
                }

                if (amoFullReport.MonthCorrectLeads != 0)
                    amoFullReport.Conversion = Math.Round(100 * (decimal)amoFullReport.MonthSales / (decimal)amoFullReport.MonthCorrectLeads, 2);

                amoFullReports.Add(amoFullReport);
            }
            return amoFullReports;
        }

        [HttpGet("GetAmoSalesReports")]
        public List<AmoSalesReport> GetAmoSalesReports()
        {
            var todayReports = _applicationContext.AmoDayReports
                .AsNoTracking()
                .Where(x => x.Date.Date == DateTime.Today.AddDays(-1))
                .ToList();

            var monthDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var monthReports = _applicationContext.AmoMonthReports
                .AsNoTracking()
                .Where(x => x.Date.Date == monthDateTime)
                .ToList();

            var amoSalesReports = new List<AmoSalesReport>();
            foreach (var todayReport in todayReports)
            {
                var amoSalesReport = new AmoSalesReport();
                amoSalesReport.Project = todayReport.ProjectName;
                amoSalesReport.TodayForehead = todayReport.Forehead;
                amoSalesReport.TodaySales = todayReport.Sales;

                var monthReport = monthReports.FirstOrDefault(x => x.ProjectName == todayReport.ProjectName);

                if (monthReport != null)
                {
                    amoSalesReport.MonthSales = monthReport.Sales;
                    amoSalesReport.MonthCorrectLeads = monthReport.CorrectLeads;
                }

                if (amoSalesReport.MonthCorrectLeads != 0)
                    amoSalesReport.Conversion = Math.Round(100 * (decimal)amoSalesReport.MonthSales / (decimal)amoSalesReport.MonthCorrectLeads, 2);

                amoSalesReports.Add(amoSalesReport);
            }
            return amoSalesReports;
        }

        [HttpGet("GetAmoConversionCharts")]
        public List<AmoConversionChart> GetAmoConversionCharts()
        {
            var todayReports = _applicationContext.AmoDayReports
                .AsNoTracking()
                .Where(x => x.Date.Date == DateTime.Today.AddDays(-1))
                .ToList();

            var monthDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var monthReports = _applicationContext.AmoMonthReports
                .AsNoTracking()
                .Where(x => x.Date.Date == monthDateTime)
                .ToList();

            var amoConversionCharts = new List<AmoConversionChart>();

            foreach (var todayReport in todayReports)
            {
                int monthSales = 0;
                int monthCorrectLeads = 0;
                decimal conversion = 0;
                var amoConversionChart = new AmoConversionChart();

                amoConversionChart.Project = todayReport.ProjectName;

                var monthReport = monthReports.FirstOrDefault(x => x.ProjectName == todayReport.ProjectName);
                if (monthReport != null)
                {
                    monthSales = monthReport.Sales;
                    monthCorrectLeads = monthReport.CorrectLeads;
                }

                if (monthCorrectLeads != 0)
                    conversion = Math.Round(100 * (decimal)monthSales / (decimal)monthCorrectLeads, 2);

                amoConversionChart.Conversion = conversion;

                amoConversionCharts.Add(amoConversionChart);
            }

            return amoConversionCharts;
        }

        [HttpGet("GetAmoMonthSalesCharts")]
        public List<AmoMonthSalesChart> GetAmoMonthSalesCharts()
        {
            var todayReports = _applicationContext.AmoDayReports
                .AsNoTracking()
                .Where(x => x.Date.Date == DateTime.Today.AddDays(-1))
                .ToList();

            var monthDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var monthReports = _applicationContext.AmoMonthReports
                .AsNoTracking()
                .Where(x => x.Date.Date == monthDateTime)
                .ToList();

            var amoMonthSalesCharts = new List<AmoMonthSalesChart>();

            foreach (var todayReport in todayReports)
            {
                var amoMonthSalesChart = new AmoMonthSalesChart();
                amoMonthSalesChart.Project = todayReport.ProjectName;

                var monthReport = monthReports.FirstOrDefault(x => x.ProjectName == todayReport.ProjectName);
                if (monthReport != null)
                    amoMonthSalesChart.MonthSales = monthReport.Sales;

                amoMonthSalesCharts.Add(amoMonthSalesChart);
            }

            return amoMonthSalesCharts;
        }

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
        public int GamesSoldThisMonth { get; set; }
        public int AmountOfSalesThisMonth { get; set; }
        public int GameCompletedThisMonth { get; set; }
        public int GamesSoldNextMonth { get; set; }
        public int AmountOfSalesNextMonth { get; set; }
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

    public class AmoConversionChart
	{
        public string Project { get; set; }
        public decimal Conversion { get; set; }
    }
    public class AmoMonthSalesChart
    {
        public string Project { get; set; }
        public int MonthSales { get; set; }
    }
}