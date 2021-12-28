using System.Collections.Generic;

namespace RoistatClient
{
    public class Project
    {
        public int id { get; set; }
        public string name { get; set; }
        public object profit { get; set; }
        public string creation_date { get; set; }
        public string currency { get; set; }
        public object timeZone { get; set; }
        public int is_owner { get; set; }
    }

    public class GetProjectResponce
    {
        public List<Project> projects { get; set; }
        public string status { get; set; }
    }
}