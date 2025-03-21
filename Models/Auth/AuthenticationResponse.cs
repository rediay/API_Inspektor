namespace Inspektor_API_REST.Models.Auth
{
    public class AuthenticationResponse
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public Usuarios? DataUser { get; set; }
    }
}
