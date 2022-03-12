using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace AmoClient
{
    public class AmoParcer
    {
        public static List<Lead> ParceLeads(string json)
        {
            var leads = new List<Lead>();

            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            var response = root.GetProperty("response");
            var items = response.GetProperty("items");

            foreach (var parentItem in items.EnumerateObject())
            {
                foreach (var item in parentItem.Value.EnumerateObject())
                {
                    var lead = new Lead();
                    lead.ParentName = parentItem.Name;
                    leads.Add(lead);

                    lead.Id = item.Value.GetProperty("id").GetInt32();

                    var name = item.Value.GetProperty("name");
                    lead.NameText = name.GetProperty("text").GetString();
                    lead.NameUrl = name.GetProperty("url").GetString();

                    lead.Status = item.Value.GetProperty("status").GetInt32();
                    lead.Budget = item.Value.GetProperty("budget").GetInt32();

                    var budget_formatted = item.Value.GetProperty("budget_formatted");

                    lead.CanEdit = item.Value.GetProperty("can_edit").GetBoolean();

                    var main_contact = item.Value.GetProperty("main_contact");
                    if (main_contact.ValueKind == JsonValueKind.Object)
                    {
                        lead.main_contact_id = main_contact.GetProperty("id").GetInt32();
                        lead.main_contact_name = main_contact.GetProperty("name").GetString();
                        lead.main_contact_url = main_contact.GetProperty("url").GetString();
                    }

                    var company_name = item.Value.GetProperty("company_name");
                    if (company_name.ValueKind == JsonValueKind.Object)
                    {
                        lead.company_name_id = company_name.GetProperty("id").GetInt32();
                        lead.company_name_name = company_name.GetProperty("name").GetString();
                        lead.company_name_url = company_name.GetProperty("url").GetString();
                    }

                    var source_data = item.Value.GetProperty("source_data");
                    if (source_data.ValueKind == JsonValueKind.Object)
                    {
                        var count = source_data.EnumerateObject().ToList().Count();
                        lead.source_data_name = source_data.GetProperty("name").GetString();
                    }

                    var tags = item.Value.GetProperty("tags");
                    if (tags.ValueKind == JsonValueKind.Object)
                    {
                        var tags_items = tags.GetProperty("items");
                        if (tags_items.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var tags_item in tags_items.EnumerateArray())
                            {
                                var dealTag = new LeadTag();
                                dealTag.Id = tags_item.GetProperty("id").GetInt32();
                                dealTag.Label = tags_item.GetProperty("label").GetString();
                                lead.Tags.Add(dealTag);
                            }
                        }
                    }

                    var date_create = item.Value.GetProperty("date_create").GetInt32();
                    lead.date_create = new DateTime(1970, 1, 1).AddSeconds(date_create);

                    lead.lead_mark = item.Value.GetProperty("lead_mark").GetString();

                    var loss_reason = item.Value.GetProperty("loss_reason");

                    var last_event_at = item.Value.GetProperty("last_event_at");

                    var last_message_at = item.Value.GetProperty("last_message_at");
                }
            }

            return leads;
        }
    }
}