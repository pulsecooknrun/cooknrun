namespace AmoClient
{
    public class AutorizeRequest
    {
        public string csrf_token { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string temporary_auth { get; set; }
    }
}