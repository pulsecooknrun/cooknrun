using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;

namespace AmoClient
{
    public class AmoProxyService
    {
        //string _login = "franchise@infernocook.ru";
        //string _password = "Kjc-e6z-LSY-DdL";

        string _login = "anna.admarketing@yandex.ru";
        string _password = "sD9-94w-mwD-ucw";

        public AmoProxyService()
        {

        }

        private void EnsureSuccessStatusCode(IRestResponse response)
        {
            if (response.IsSuccessful)
                return;

            var json = JObject.Parse(response.Content);

            throw new Exception(
                $"Error. Status description: '{response.StatusDescription}', Content: '{response.Content}'");
        }

        public AuthorizeResponce Authorize()
        {
            var client = new RestClient("https://infernocook.amocrm.ru/oauth2/authorize");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");

            var autorizeRequest = new AutorizeRequest
            {
                csrf_token = "def502004e004b5e43bfa4381d611d68458f0ce485a2ab423e899b16cfbd300b12591cb367312afb3e383ef0b31e9dc5297be8feb61ce2b0f60d0d466f50f99993996985ca49c1c21e64964bbe261251dc113858063becf9b308cb0068fc4789cfcc02ce85f0c46c5741366daf0c6f77334b1f0c8db16b0fde5b12bf81c1754a1d2093daea4accc66f0041399ba0b67b8825bf5a72ed64c58f1dd23e6f9d70d1aa6f55cdd248b1cd6d041d7c6cd0bef3a5cbd43abe930deb829d0794e1656fa7a8d40b3d092056cdb8dc69f22ef5c922562495160f24dea8672c3c1603ac01e7fd757747ff3a9fc8c960f9293e2005f9bbf97b99c91101ca0359f7261f6c840398f881caf22695ba6afecf",
                temporary_auth = "N",
                username = _login,
                password = _password
            };
            request.AddJsonBody(autorizeRequest);

            var response = client.Execute<AuthorizeResponce>(request);
            EnsureSuccessStatusCode(response);
            AuthorizeResponce authorizeResponce = response.Data;
            return authorizeResponce;
        }

        public void GetUsers()
        {
            var authorizeResponce = Authorize();
            var token = "access_token=" + authorizeResponce.access_token + "; refresh_token=" + authorizeResponce.refresh_token + ";";

            var client = new RestClient("https://infernocook.amocrm.ru/api/v4/users?page=2");
            var request = new RestRequest(Method.GET);

            request.AddHeader("Cookie", token);

            var response = client.Execute<GetUsersResult>(request);
            foreach (var user in response.Data._embedded.users)
            {
                if (user.rights.leads.view == "M")
                    Console.WriteLine(user.name + " " + user.rights.leads.view);
            }
            EnsureSuccessStatusCode(response);
        }

        public List<Lead> GetLeads()
        {
            var authorizeResponce = Authorize();

            var client = new RestClient("https://infernocook.amocrm.ru/ajax/leads/pipeline/4406956/");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddQueryParameter("filter%5Btags_logic%5D", "or");
            request.AddQueryParameter("filter%5Bdate_preset%5D", "week");
            request.AddQueryParameter("useFilter", "y");
            request.AddHeader("Cookie", "access_token=" + authorizeResponce.access_token + "; refresh_token=" + authorizeResponce.refresh_token + ";");

            request.AddHeader("sec-ch-ua", "'Google Chrome';v='95', 'Chromium';v='95', '; Not A Brand';v='99'");
            request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36");
            request.AddHeader("sec-ch-ua-platform", "'Windows'");
            request.AddHeader("Origin", "https://infernocook.amocrm.ru");
            request.AddHeader("Sec-Fetch-Site", "same-origin");
            request.AddHeader("Sec-Fetch-Mode", "cors");
            request.AddHeader("Sec-Fetch-Dest", "empty");
            //request.AddHeader("Referer", "https://infernocook.amocrm.ru/leads/pipeline/4406956/?filter%5Btags_logic%5D=or&filter%5Bdate_preset%5D=week&useFilter=y");
            request.AddHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");

            var response = client.Execute<string>(request);
            EnsureSuccessStatusCode(response);

            var leads = AmoParcer.ParceLeads(response.Data);

            return leads;
        }

        // лиды
        public int GetLeads(DateTime startDateTime, DateTime endDateTime, string token, string userId)
        {
            var parametersSale = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("filter[pipe][4406956][]", "142"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "143"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928542"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928545"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928548"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928551"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928584"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928587"),
                };
            var leadsSale = GetCount(token, "4406956", parametersSale, startDateTime, endDateTime, userId);

            var parametersGame = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("filter[pipe][4406959][]", "142"),
                    //new Tuple<string, string>("filter[pipe][4406959][]", "143"),
                    new Tuple<string, string>("filter[pipe][4406959][]", "40928554"),
                    new Tuple<string, string>("filter[pipe][4406959][]", "40928557"),
                };
            var leadsGame = GetCount(token, "4406959", parametersGame, startDateTime, endDateTime, userId);

            return leadsSale + leadsGame;
        }

        // закрытые
        public int GetClosed(DateTime startDateTime, DateTime endDateTime, string token, string userId)
        {
            var parametersSale = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("filter[pipe][4406956][]", "143"),
                };
            var closedSale = GetCount(token, "4406956", parametersSale, null, null, userId);

            var parametersGame = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("filter[pipe][4406959][]", "143"),
                };
            var closedGame = GetCount(token, "4406959", parametersGame, null, null, userId);

            return closedSale + closedGame;
        }

        // лиды без задач
        public int GetLeadsWithoutTasks(DateTime startDateTime, DateTime endDateTime, string token, string userId)
        {
            var parameters = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("filter[pipe][4406956][]", "142"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "143"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928542"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928545"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928548"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928551"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928584"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928587"),
                    new Tuple<string, string>("filter[tasks][]", "no_tasks"),
                    new Tuple<string, string>("filter[loss_reason_id][]", "0"),
                    new Tuple<string, string>("filter[loss_reason_id][]", "7656397"),
                    new Tuple<string, string>("filter[loss_reason_id][]", "7656400"),
                    new Tuple<string, string>("filter[loss_reason_id][]", "8661985"),
                };
            var leadsWithoutTasks = GetCount(token, "4406956", parameters, null, null, userId);
            return leadsWithoutTasks;
        }

        // просроченные
        public int GetOverdue(DateTime startDateTime, DateTime endDateTime, string token, string userId)
        {
            var parameters = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("filter[pipe][4406956][]", "142"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "143"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928542"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928545"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928548"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928551"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928584"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928587"),
                    new Tuple<string, string>("filter[tasks][]", "failed_tasks"),
                };
            var overdue = GetCount(token, "4406956", parameters, null, null, userId);
            return overdue;
        }

        // предбронь
        public int GetForehead(DateTime startDateTime, DateTime endDateTime, string token, string userId)
        {
            var parametersSale = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("filter[pipe][4406956][]", "142"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "143"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928542"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928545"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928548"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928551"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928584"),
                    new Tuple<string, string>("filter[pipe][4406956][]", "40928587"),
                    new Tuple<string, string>("filter[cf][1130257][from]", startDateTime.ToString("dd.MM.yyyy")),
                    new Tuple<string, string>("filter[cf][1130257][to]", endDateTime.ToString("dd.MM.yyyy")),
                };
            var foreheadSale = GetCount(token, "4406956", parametersSale, null, null, userId);

            var parametersGame = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("filter[pipe][4406959][]", "142"),
                    new Tuple<string, string>("filter[pipe][4406959][]", "143"),
                    new Tuple<string, string>("filter[pipe][4406959][]", "40928554"),
                    new Tuple<string, string>("filter[pipe][4406959][]", "40928557"),
                    new Tuple<string, string>("filter[cf][1130257][from]", startDateTime.ToString("dd.MM.yyyy")),
                    new Tuple<string, string>("filter[cf][1130257][to]", endDateTime.ToString("dd.MM.yyyy")),
                };
            var foreheadGame = GetCount(token, "4406959", parametersGame, null, null, userId);

            return foreheadSale + foreheadGame;
        }

        public int GetSales(DateTime startDateTime, DateTime endDateTime, string token, string userId)
        {
            var parameters = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("filter[pipe][4406959][]", "142"),
                  //new Tuple<string, string>("filter[pipe][4406959][]", "143"),
                    new Tuple<string, string>("filter[pipe][4406959][]", "40928554"),
                    new Tuple<string, string>("filter[pipe][4406959][]", "40928557"),
                    new Tuple<string, string>("filter[cf][608242][from]", startDateTime.ToString("dd.MM.yyyy")),
                    new Tuple<string, string>("filter[cf][608242][to]", endDateTime.ToString("dd.MM.yyyy")),
                };
            var sales = GetCount(token, "4406959", parameters, null, null, userId);
            return sales;
        }

        public int GetCorrectLeads(DateTime startDateTime, DateTime endDateTime, string token, string userId)
          {
              var parametersSale = new List<Tuple<string, string>>
                  {
                      new Tuple<string, string>("filter[pipe][4406956][]", "142"),
                      new Tuple<string, string>("filter[pipe][4406956][]", "143"),
                      new Tuple<string, string>("filter[pipe][4406956][]", "40928542"),
                      new Tuple<string, string>("filter[pipe][4406956][]", "40928545"),
                      new Tuple<string, string>("filter[pipe][4406956][]", "40928548"),
                      new Tuple<string, string>("filter[pipe][4406956][]", "40928551"),
                      new Tuple<string, string>("filter[pipe][4406956][]", "40928584"),
                      new Tuple<string, string>("filter[pipe][4406956][]", "40928587"),
                      new Tuple<string, string>("filter[loss_reason_id][]", "0"),
                      new Tuple<string, string>("filter[loss_reason_id][]", "7656397"),
                      new Tuple<string, string>("filter[loss_reason_id][]", "7656400"),
                      new Tuple<string, string>("filter[loss_reason_id][]", "8661985"),
                  };
              var correctLeads = GetCountForCorrectLeads(token, "4406956", parametersSale, startDateTime, endDateTime, userId);

              return correctLeads;
          }

        public int GetGamesSoldThisMonth(DateTime startDateTime, DateTime endDateTime, string token, string userId)
		{
            var parameters = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("filter[pipe][4406959][]", "142"),
                    new Tuple<string, string>("filter[pipe][4406959][]", "40928554"),
                    new Tuple<string, string>("filter[pipe][4406959][]", "40928557"),
                    new Tuple<string, string>("filter[cf][1137099][from]", startDateTime.ToString("dd.MM.yyyy")),
                    new Tuple<string, string>("filter[cf][1137099][to]", endDateTime.ToString("dd.MM.yyyy")),
                };
            var gamesSold = GetCount(token, "4406959", parameters, null, null, userId);

            return gamesSold;
		}

        public int GetAmountOfSalesThisMonth(DateTime startDateTime, DateTime endDateTime, string token, string userId)
		{
            var parameters = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("filter[pipe][4406959][]", "142"),
                    new Tuple<string, string>("filter[pipe][4406959][]", "40928554"),
                    new Tuple<string, string>("filter[pipe][4406959][]", "40928557"),
                    new Tuple<string, string>("filter[cf][1137099][from]", startDateTime.ToString("dd.MM.yyyy")),
                    new Tuple<string, string>("filter[cf][1137099][to]", endDateTime.ToString("dd.MM.yyyy")),
                };
            var amountOfSales = GetCountOfPrice(token, "4406959", parameters, null, null, userId);

            return amountOfSales;
        }

        public int GetGameCompletedThisMonth(DateTime startDateTime, DateTime endDateTime, string token, string userId)
		{
            var parameters = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("filter[pipe][4406959][]", "142"),
                    new Tuple<string, string>("filter[pipe][4406959][]", "40928554"),
                    new Tuple<string, string>("filter[pipe][4406959][]", "40928557"),
                    new Tuple<string, string>("filter[cf][1137099][from]", startDateTime.ToString("dd.MM.yyyy")),
                    new Tuple<string, string>("filter[cf][1137099][to]", endDateTime.ToString("dd.MM.yyyy")),
                };
            var gameCompleted = GetCount(token, "4406959", parameters, null, null, userId);

            return gameCompleted;
		}

        public int GetGamesSoldNextMonth(DateTime startDateTime, DateTime endDateTime, string token, string userId)
		{
            var parameters = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("filter[pipe][4406959][]", "142"),
                    new Tuple<string, string>("filter[pipe][4406959][]", "40928554"),
                    new Tuple<string, string>("filter[pipe][4406959][]", "40928557"),
                    new Tuple<string, string>("filter[cf][1137099][from]", startDateTime.ToString("dd.MM.yyyy")),
                    new Tuple<string, string>("filter[cf][1137099][to]", endDateTime.ToString("dd.MM.yyyy")),
                };
            var gamesSold = GetCount(token, "4406959", parameters, null, null, userId);

            return gamesSold;
        }

        public int GetAmountOfSalesNextMonth(DateTime startDateTime, DateTime endDateTime, string token, string userId)
		{
            var parameters = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("filter[pipe][4406959][]", "142"),
                    new Tuple<string, string>("filter[pipe][4406959][]", "40928554"),
                    new Tuple<string, string>("filter[pipe][4406959][]", "40928557"),
                    new Tuple<string, string>("filter[cf][1137099][from]", startDateTime.ToString("dd.MM.yyyy")),
                    new Tuple<string, string>("filter[cf][1137099][to]", endDateTime.ToString("dd.MM.yyyy")),
                };
            var amountOfSales = GetCountOfPrice(token, "4406959", parameters, null, null, userId);

            return amountOfSales;
        }

        public int GetCount(string token, string dealType, List<Tuple<string, string>> roistatFilterItemNos, DateTime? startDate, DateTime? endDate, string user)
        {
            var client = new RestClient("https://infernocook.amocrm.ru/ajax/leads/sum/" + dealType);
            var request = new RestRequest(Method.POST);

            foreach (var roistatFilterItemNo in roistatFilterItemNos)
            {
                request.AddQueryParameter(roistatFilterItemNo.Item1, roistatFilterItemNo.Item2);
            }

            request.AddQueryParameter("filter[tags_logic]", "or");
            request.AddQueryParameter("filter[main_user][]", user);
            request.AddQueryParameter("useFilter", "y");

            if (startDate != null)
                request.AddQueryParameter("filter_date_from", startDate.Value.ToString("dd.MM.yyyy"));
            if (endDate != null)
                request.AddQueryParameter("filter_date_to", endDate.Value.ToString("dd.MM.yyyy"));

            request.AddHeader("Cookie", token);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("sec-ch-ua", "'Google Chrome';v='95', 'Chromium';v='95', '; Not A Brand';v='99'");
            request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36");
            request.AddHeader("sec-ch-ua-platform", "'Windows'");
            request.AddHeader("Origin", "https://infernocook.amocrm.ru");
            request.AddHeader("Sec-Fetch-Site", "same-origin");
            request.AddHeader("Sec-Fetch-Mode", "cors");
            request.AddHeader("Sec-Fetch-Dest", "empty");
            request.AddHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");

            request.AddJsonBody(new AmoSumRequest());

            var response = client.Execute<AmoSumResponce>(request);
            EnsureSuccessStatusCode(response);

            return response.Data.all_count;
        }

        public int GetCountOfPrice(string token, string dealType, List<Tuple<string, string>> roistatFilterItemNos, DateTime? startDate, DateTime? endDate, string user)
        {
            var client = new RestClient("https://infernocook.amocrm.ru/ajax/leads/sum/" + dealType);
            var request = new RestRequest(Method.POST);

            foreach (var roistatFilterItemNo in roistatFilterItemNos)
            {
                request.AddQueryParameter(roistatFilterItemNo.Item1, roistatFilterItemNo.Item2);
            }

            request.AddQueryParameter("filter[tags_logic]", "or");
            request.AddQueryParameter("filter[main_user][]", user);
            request.AddQueryParameter("useFilter", "y");

            if (startDate != null)
                request.AddQueryParameter("filter_date_from", startDate.Value.ToString("dd.MM.yyyy"));
            if (endDate != null)
                request.AddQueryParameter("filter_date_to", endDate.Value.ToString("dd.MM.yyyy"));

            request.AddHeader("Cookie", token);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("sec-ch-ua", "'Google Chrome';v='95', 'Chromium';v='95', '; Not A Brand';v='99'");
            request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36");
            request.AddHeader("sec-ch-ua-platform", "'Windows'");
            request.AddHeader("Origin", "https://infernocook.amocrm.ru");
            request.AddHeader("Sec-Fetch-Site", "same-origin");
            request.AddHeader("Sec-Fetch-Mode", "cors");
            request.AddHeader("Sec-Fetch-Dest", "empty");
            request.AddHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");

            request.AddJsonBody(new AmoSumRequest());

            var response = client.Execute<AmoSumResponce>(request);
            EnsureSuccessStatusCode(response);

            int countOfPrice = 0;
            foreach(var item in response.Data.leads_by_status)
			{
                countOfPrice += item.total_price;
			}

            return countOfPrice;
        }

        public int GetCountForCorrectLeads(string token, string dealType, List<Tuple<string, string>> roistatFilterItemNos, DateTime? startDate, DateTime? endDate, string user)
        {
            var client = new RestClient("https://infernocook.amocrm.ru/ajax/leads/sum/" + dealType);
            var request = new RestRequest(Method.POST);

            foreach (var roistatFilterItemNo in roistatFilterItemNos)
            {
                request.AddQueryParameter(roistatFilterItemNo.Item1, roistatFilterItemNo.Item2);
            }

            request.AddQueryParameter("filter[tags_logic]", "or");
            request.AddQueryParameter("filter[main_user][]", user);
            request.AddQueryParameter("useFilter", "y");

            if (startDate != null)
                request.AddQueryParameter("filter_date_from", startDate.Value.ToString("dd.MM.yyyy"));
            if (endDate != null)
                request.AddQueryParameter("filter_date_to", endDate.Value.ToString("dd.MM.yyyy"));

            request.AddHeader("Cookie", token);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("sec-ch-ua", "'Google Chrome';v='95', 'Chromium';v='95', '; Not A Brand';v='99'");
            request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36");
            request.AddHeader("sec-ch-ua-platform", "'Windows'");
            request.AddHeader("Origin", "https://infernocook.amocrm.ru");
            request.AddHeader("Sec-Fetch-Site", "same-origin");
            request.AddHeader("Sec-Fetch-Mode", "cors");
            request.AddHeader("Sec-Fetch-Dest", "empty");
            request.AddHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");

            request.AddJsonBody(new AmoSumRequest());

            var response = client.Execute<AmoSumResponce>(request);
            EnsureSuccessStatusCode(response);

            int countOfLeads = 0;
            foreach (var item in response.Data.leads_by_status)
            {
                countOfLeads += item.count;
            }

            return countOfLeads;
        }
    }

    public class AmoSumRequest
    {
        public string leads_by_status { get; set; } = "Y";
    }

    public class AmoManager
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
}