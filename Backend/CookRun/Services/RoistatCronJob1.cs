using CookRun.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RoistatClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CookRun.Services
{
    public class RoistatCronJob1 : CronJobService
    {
        private readonly ILogger<RoistatCronJob1> _logger;
        private readonly IServiceProvider _serviceProvider;

        public RoistatCronJob1(IScheduleConfig<RoistatCronJob1> config, ILogger<RoistatCronJob1> logger, IServiceProvider serviceProvider)
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
            var roistatProxyService = scope.ServiceProvider.GetRequiredService<RoistatProxyService>();

            var log1 = new Log("Begin RoistatMeanReport");
            applicationContext.Logs.Add(log1);
            applicationContext.SaveChanges();

            try
            {
                var result = "";

                var jobReport = new JobReport
                {
                    Id = Guid.NewGuid(),
                    DateTime = DateTime.UtcNow,
                    Name = "RoistatMeanReport"
                };
                applicationContext.JobReports.Add(jobReport);

                var projects = roistatProxyService.GetProjects();
                foreach (var project in projects)
                {
                    var roistatMeanReport = new RoistatMeanReport();
                    roistatMeanReport.JobReportId = jobReport.Id;
                    roistatMeanReport.ProjectName = project.name;
                    applicationContext.RoistatMeanReports.Add(roistatMeanReport);

                    Console.WriteLine("=======    " + project.name);
                    result += "\n" + project.name;

                    var analyticsData = roistatProxyService.GetAnalyticsData(project.id, DateTime.Now.AddDays(-90), DateTime.Now);
                    foreach (var metric in analyticsData.mean.metrics)
                    {
                        if (metric.metric_name == "visits")
                        {
                            roistatMeanReport.Visits = metric.value;
                        }
                        if (metric.metric_name == "visitCount")
                        {
                            roistatMeanReport.ConversionVisitsToLeads = metric.value;
                        }
                        if (metric.metric_name == "leads")
                        {
                            roistatMeanReport.Leads = metric.value;
                        }
                        if (metric.metric_name == "mc_leads")
                        {
                            roistatMeanReport.McLeads = metric.value;
                        }
                        if (metric.metric_name == "conversion_leads_to_sales")
                        {
                            roistatMeanReport.ConversionLeadsToSales = metric.value;
                        }
                        if (metric.metric_name == "sales")
                        {
                            roistatMeanReport.Sales = metric.value;
                        }
                        if (metric.metric_name == "revenue")
                        {
                            roistatMeanReport.Revenue = metric.value;
                        }
                        if (metric.metric_name == "average_sale")
                        {
                            roistatMeanReport.AverageSale = metric.value;
                        }
                        if (metric.metric_name == "profit")
                        {
                            roistatMeanReport.Profit = metric.value;
                        }
                        if (metric.metric_name == "marketing_cost")
                        {
                            roistatMeanReport.MarketingCost = metric.value;
                        }
                        if (metric.metric_name == "roi")
                        {
                            roistatMeanReport.Roi = metric.value;
                        }
                        if (metric.metric_name == "net_cost")
                        {
                            roistatMeanReport.NetCost = metric.value;
                        }

                        Console.WriteLine(metric.metric_name + " " + metric.value);
                        result += " " + metric.metric_name + " " + metric.value;
                    }
                }

                applicationContext.SaveChanges();

                var log3 = new Log("End RoistatMeanReport");
                applicationContext.Logs.Add(log3);
                applicationContext.SaveChanges();
            }
            catch(Exception e)
            {
                var log2 = new Log(e.Message);
                applicationContext.Logs.Add(log2);
                applicationContext.SaveChanges();
            }

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}