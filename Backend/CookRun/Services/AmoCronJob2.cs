using AmoClient;
using CookRun.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CookRun.Services
{
    public class AmoCronJob2 : CronJobService
    {
        private readonly ILogger<AmoCronJob2> _logger;
        private readonly IServiceProvider _serviceProvider;

        public AmoCronJob2(IScheduleConfig<AmoCronJob2> config, ILogger<AmoCronJob2> logger, IServiceProvider serviceProvider)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 2 starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob 2 is working.");

            using var scope = _serviceProvider.CreateScope();
            var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            var amoProxyService = scope.ServiceProvider.GetRequiredService<AmoProxyService>();

            var jobReport = new JobReport
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.UtcNow,
                Name = "AmoSumReport"
            };
            applicationContext.JobReports.Add(jobReport);

            var users = new List<AmoManager>();
            users.Add(new AmoManager { Name = "а_Курская", Id = "7150507" });
            users.Add(new AmoManager { Name = "а_Владивосток", Id = "7183879" });
            users.Add(new AmoManager { Name = "а_Строгино", Id = "7183897" });
            users.Add(new AmoManager { Name = "а_РостовНаДону", Id = "7183906" });
            users.Add(new AmoManager { Name = "а_Казань", Id = "7222321" });
            users.Add(new AmoManager { Name = "а_Хабаровск", Id = "7237606" });
            users.Add(new AmoManager { Name = "а_Уфа", Id = "7249252" });
            users.Add(new AmoManager { Name = "а_Новосибирск", Id = "7249255" });
            users.Add(new AmoManager { Name = "а_Екатеринбург", Id = "7337203" });
            users.Add(new AmoManager { Name = "а_СанктПетербург", Id = "7337209" });
            users.Add(new AmoManager { Name = "а_Сочи", Id = "7507468" });
            users.Add(new AmoManager { Name = "а_Краснодар", Id = "7636588" });
            users.Add(new AmoManager { Name = "а_Балашиха", Id = "7830271" });

            var authorizeResponce = amoProxyService.Authorize();
            var token = "access_token=" + authorizeResponce.access_token + "; refresh_token=" + authorizeResponce.refresh_token + ";";

            foreach (var user in users)
            {
                var month = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                var leads = amoProxyService.GetLeads(month, DateTime.Today.AddDays(-1), token, user.Id);
                var closed = amoProxyService.GetClosed(month, DateTime.Today.AddDays(-1), token, user.Id);
                var sales = amoProxyService.GetSales(month, DateTime.Today.AddDays(-1), token, user.Id);

                var amoMonthReport = applicationContext.AmoMonthReports.FirstOrDefault(x => x.ProjectName == user.Name && x.Date == month);
                if (amoMonthReport == null)
                {
                    amoMonthReport = new AmoMonthReport()
                    {
                        ProjectName = user.Name,
                        Date = month,
                        Leads = leads,
                        Closed = closed,
                        Sales = sales
                    };
                    applicationContext.AmoMonthReports.Add(amoMonthReport);
                }
                else
                {
                    amoMonthReport.Leads = leads;
                    amoMonthReport.Closed = closed;
                    amoMonthReport.Sales = sales;
                }

                for (int i = 0; i < 10; i++)
                {
                    var date = DateTime.Now.Date.AddDays(-i);

                    leads = amoProxyService.GetLeads(date, date, token, user.Id);
                    closed = amoProxyService.GetClosed(date, date, token, user.Id);
                    var leadsWithoutTasks = amoProxyService.GetLeadsWithoutTasks(date, date, token, user.Id);
                    var overdue = amoProxyService.GetOverdue(date, date, token, user.Id);
                    var forehead = amoProxyService.GetForehead(date, date, token, user.Id);
                    sales = amoProxyService.GetSales(date, date, token, user.Id);

                    var amoDayReport = applicationContext.AmoDayReports.FirstOrDefault(x => x.ProjectName == user.Name && x.Date == date);
                    if (amoDayReport == null)
                    {
                        amoDayReport = new AmoDayReport()
                        {
                            ProjectName = user.Name,
                            Date = date,
                            Leads = leads,
                            Closed = closed,
                            LeadsWithoutTasks = leadsWithoutTasks,
                            Overdue = overdue,
                            Forehead = forehead,
                            Sales = sales,
                        };
                        applicationContext.AmoDayReports.Add(amoDayReport);
                    }
                    else
                    {
                        amoDayReport.Leads = leads;
                        amoDayReport.Closed = closed;
                        amoDayReport.LeadsWithoutTasks = leadsWithoutTasks;
                        amoDayReport.Overdue = overdue;
                        amoDayReport.Forehead = forehead;
                        amoDayReport.Sales = sales;
                    }
                }
            }

            applicationContext.SaveChanges();

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 2 is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
