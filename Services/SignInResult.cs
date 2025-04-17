namespace Inventario360.Web.Services
{
    public class SignInResult
    {
        public bool Succeeded { get; set; }

        public static SignInResult Success => new SignInResult { Succeeded = true };
        public static SignInResult Failed => new SignInResult { Succeeded = false };
    }
}
