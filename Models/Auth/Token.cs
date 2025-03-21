namespace Inspektor_API_REST.Models.Auth
{
    public class Token
    {
        public double Expires_in {  get; set; }

        public string Access_token { get; set; }

        public string Refresh_token { get; set; }
    }
}
