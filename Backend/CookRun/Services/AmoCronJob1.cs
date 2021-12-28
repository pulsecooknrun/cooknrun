using AmoClient;
using CookRun.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CookRun.Services
{
    public class AmoCronJob1 : CronJobService
    {
        private readonly ILogger<AmoCronJob1> _logger;
        private readonly IServiceProvider _serviceProvider;

        public AmoCronJob1(IScheduleConfig<AmoCronJob1> config, ILogger<AmoCronJob1> logger, IServiceProvider serviceProvider)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob 1 is working.");

            using var scope = _serviceProvider.CreateScope();
            var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            var amoProxyService = scope.ServiceProvider.GetRequiredService<AmoProxyService>();

            var jobReport = new JobReport
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.UtcNow,
                Name = "AmoLeadReport"
            };
            applicationContext.JobReports.Add(jobReport);

            var leads = amoProxyService.GetLeads();

            foreach (var lead in leads)
            {
                var amoLeadReport = new AmoLeadReport()
                {
                    JobReportId = jobReport.Id
                };
                applicationContext.AmoLeadReports.Add(amoLeadReport);

                amoLeadReport.ParentName = lead.ParentName;
                amoLeadReport.LeadId = lead.Id;
                amoLeadReport.NameText = lead.NameText;
                amoLeadReport.NameUrl = lead.NameUrl;
                amoLeadReport.Status = lead.Status;
                amoLeadReport.Budget = lead.Budget;
                amoLeadReport.CanEdit = lead.CanEdit;
                amoLeadReport.MainContactId = lead.main_contact_id;
                amoLeadReport.MainContactName = lead.main_contact_name;
                amoLeadReport.MainContactUrl = lead.main_contact_url;
                amoLeadReport.CompanyNameId = lead.company_name_id;
                amoLeadReport.CompanyNameName = lead.company_name_name;
                amoLeadReport.CompanyNameUrl = lead.company_name_url;
                amoLeadReport.SourceDataName = lead.source_data_name;
                amoLeadReport.Tags = lead.Tags.Select(x => new AmoLeadReportTag { TagId = x.Id, Label = x.Label }).ToList();
                amoLeadReport.DateCreate = lead.date_create;
                amoLeadReport.LeadMark = lead.lead_mark;
            }

            applicationContext.SaveChanges();

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}