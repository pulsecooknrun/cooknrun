using System.Collections.Generic;

namespace AmoClient
{
    public class Self
    {
        public string href { get; set; }
    }

    public class Next
    {
        public string href { get; set; }
    }

    public class Last
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
        public Next next { get; set; }
        public Last last { get; set; }
    }

    public class Leads
    {
        public string view { get; set; }
        public string edit { get; set; }
        public string add { get; set; }
        public string delete { get; set; }
        public string export { get; set; }
    }

    public class Contacts
    {
        public string view { get; set; }
        public string edit { get; set; }
        public string add { get; set; }
        public string delete { get; set; }
        public string export { get; set; }
    }

    public class Companies
    {
        public string view { get; set; }
        public string edit { get; set; }
        public string add { get; set; }
        public string delete { get; set; }
        public string export { get; set; }
    }

    public class Tasks
    {
        public string edit { get; set; }
        public string delete { get; set; }
    }

    public class Rights2
    {
        public string view { get; set; }
        public string edit { get; set; }
        public string delete { get; set; }
        public Leads leads { get; set; }
        public Contacts contacts { get; set; }
        public Companies companies { get; set; }
        public Tasks tasks { get; set; }
        public bool mail_access { get; set; }
        public bool catalog_access { get; set; }
        public List<StatusRight> status_rights { get; set; }
        public object catalog_rights { get; set; }
        public bool is_admin { get; set; }
        public bool is_free { get; set; }
        public bool is_active { get; set; }
        public object group_id { get; set; }
        public int? role_id { get; set; }
    }

    public class StatusRight
    {
        public string entity_type { get; set; }
        public int pipeline_id { get; set; }
        public int status_id { get; set; }
        public Rights2 rights { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string lang { get; set; }
        public Rights2 rights { get; set; }
        public Links _links { get; set; }
    }

    public class Embedded
    {
        public List<User> users { get; set; }
    }

    public class GetUsersResult
    {
        public int _total_items { get; set; }
        public int _page { get; set; }
        public int _page_count { get; set; }
        public Links _links { get; set; }
        public Embedded _embedded { get; set; }
    }
}