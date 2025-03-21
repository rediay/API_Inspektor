namespace Inspektor_API_REST.Models.Auth
{
    public class AuthResult<T>
    {
        private AuthResult(T token)
        {
            Succeeded = true;
            IsModelValid = true;
            IsBlockUser = false;
            Data = token;
            Message = new string[] { };
        }

        private AuthResult(bool isSucceeded, bool isModelValid)
        {
            Succeeded = isSucceeded;
            IsModelValid = isModelValid;
        }

        private AuthResult(string message, bool isBlockUser)
        {
            Message = new string[]{
                message
            };
            IsBlockUser = isBlockUser;
        }

        private AuthResult(bool isSucceeded)
        {
            Succeeded = isSucceeded;
            IsModelValid = isSucceeded;
        }

        public bool Succeeded { get; }
        public T Data { get; }
        public bool IsModelValid { get; }
        public bool IsBlockUser { get; }
        public string[] Message { get; }

        public static AuthResult<T> UnvalidatedResult => new AuthResult<T>(false);
        public static AuthResult<T> UnauthorizedResult => new AuthResult<T>(false, true);
        public static AuthResult<T> BlokedUserResult => new AuthResult<T>("El usuario se encuentra bloqueado.", true);
        public static AuthResult<T> BlokedCompanyResult => new AuthResult<T>("La empresa se encuentra bloqueada.", true);
        public static AuthResult<T> SucceededResult => new AuthResult<T>(true);
        public static AuthResult<T> TokenResult(T data)
        {
            return new AuthResult<T>(data);
        }
    }
}
