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
    public class RoistatController : ControllerBase
    {
        private readonly ILogger<RoistatController> _logger;
        private readonly ApplicationContext _applicationContext;

        public RoistatController(ILogger<RoistatController> logger, ApplicationContext applicationContext)
        {
            _logger = logger;
            _applicationContext = applicationContext;
        }

        [HttpGet("GetMeanForAllProjects")]
        public List<RoistatMeanReport> GetMeanForAllProjects(DateTime startDateTime, DateTime endDateTime)
        {
            var jobReport = _applicationContext.JobReports
                .AsNoTracking()
                .Where(x => x.Name == "RoistatMeanReport")
                .OrderByDescending(x => x.DateTime)
                .FirstOrDefault();

            if(jobReport != null)
            {
                var roistatMeanReports = _applicationContext.RoistatMeanReports
                    .AsNoTracking()
                    .Where(x => x.JobReportId == jobReport.Id)
                    .ToList();

                return roistatMeanReports;
            }

            return null;
        }
    }
}