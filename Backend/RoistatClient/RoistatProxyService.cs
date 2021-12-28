using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoistatClient
{
    public class RoistatProxyService
    {
        string _key = "979e1130318368a499acd883a76c0777";
        int _spbProjectId = 199133;

        public Datum GetAnalyticsData(int projectId, DateTime startDateTime, DateTime endDateTime)
        {
            var client = new RestClient("https://cloud.roistat.com/api/v1/project/analytics/data");
            var request = new RestRequest(Method.POST);
            request.AddParameter("key", _key, ParameterType.QueryString);
            request.AddParameter("project", projectId, ParameterType.QueryString);
            request.AddHeader("content-type", "application/json");
            //request.AddParameter("application/json", "{\"dimensions\":[\"marker_level_1\"],\"period\":{\"from\":\"2021-10-01T21:00:00.000Z\",\"to\":\"2021-11-08T20:59:59.999Z\"},\"filters\":[],\"next_dimensions\":[\"marker_level_2\"],\"metrics\":[{\"metric\":\"visits\",\"attribution\":\"default\"},{\"metric\":\"visits\",\"attribution\":\"default\"},{\"metric\":\"conversion_visits_to_leads\",\"attribution\":\"default\"},{\"metric\":\"leads\",\"attribution\":\"default\"},{\"metric\":\"mc_leads\",\"attribution\":\"default\"},{\"metric\":\"conversion_leads_to_sales\",\"attribution\":\"default\"},{\"metric\":\"sales\",\"attribution\":\"default\"},{\"metric\":\"revenue\",\"attribution\":\"default\"},{\"metric\":\"average_sale\",\"attribution\":\"default\"},{\"metric\":\"profit\",\"attribution\":\"default\"},{\"metric\":\"marketing_cost\",\"attribution\":\"default\"},{\"metric\":\"roi\",\"attribution\":\"default\"},{\"metric\":\"net_cost\",\"attribution\":\"default\"}]}", ParameterType.RequestBody);

            var analyticsDataRequest = new AnalyticsDataRequest
            {
                dimensions = new List<string>() { "marker_level_1" },
                period = new Period
                {
                    from = startDateTime,
                    to = endDateTime
                },
                filters = new List<object>(),
                nextDimensions = new List<string> { "marker_level_2" },
                metrics = new List<MetricItem>
                {
                    new MetricItem { metric = "visits", attribution = "default" },
                    new MetricItem { metric = "visits", attribution = "default" },
                    new MetricItem { metric = "conversion_visits_to_leads", attribution = "default" },
                    new MetricItem { metric = "leads", attribution = "default" },
                    new MetricItem { metric = "mc_leads", attribution = "default" },
                    new MetricItem { metric = "conversion_leads_to_sales", attribution = "default" },
                    new MetricItem { metric = "sales", attribution = "default" },
                    new MetricItem { metric = "revenue", attribution = "default" },
                    new MetricItem { metric = "average_sale", attribution = "default" },
                    new MetricItem { metric = "profit", attribution = "default" },
                    new MetricItem { metric = "marketing_cost", attribution = "default" },
                    new MetricItem { metric = "roi", attribution = "default" },
                    new MetricItem { metric = "net_cost", attribution = "default" },
                }
            };
            request.AddJsonBody(analyticsDataRequest);

            var response = client.Execute<AnalyticsDataResponce>(request);
            EnsureSuccessStatusCode(response);
            AnalyticsDataResponce analyticsDataResponce = response.Data;

            foreach (var datum in analyticsDataResponce.Data)
            {
                foreach (var item in datum.items)
                {
                    if (item.isHasChild > 0)
                    {
                        ;
                    }

                    //Console.WriteLine(item.dimensions.marker_level_1.title);

                    if (item.dimensions.marker_level_1.title == "Яндекс.Директ")
                    {
                        ;
                    }
                }
            }
            ;

            return analyticsDataResponce.Data[0];
        }

        public List<Project> GetProjects()
        {
            var client = new RestClient("https://cloud.roistat.com/api/v1/user/projects");
            var request = new RestRequest(Method.GET);
            request.AddParameter("key", _key, ParameterType.QueryString);
            request.AddHeader("content-type", "application/json");

            var response = client.Execute<GetProjectResponce>(request);
            EnsureSuccessStatusCode(response);
            GetProjectResponce getProjectResponce = response.Data;

            return getProjectResponce.projects.Where(x => x.name != "Test project name").ToList();
        }

        private void EnsureSuccessStatusCode(IRestResponse response)
        {
            if (response.IsSuccessful)
                return;

            var json = JObject.Parse(response.Content);

            throw new Exception(
                $"Error. Status description: '{response.StatusDescription}', Content: '{response.Content}'");
        }
    }
}