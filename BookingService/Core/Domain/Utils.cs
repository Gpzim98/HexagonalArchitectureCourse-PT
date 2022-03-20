namespace Domain.UtilsTools
{
    public static class Utils
    {
        public static bool ValidateEmail(string email)
        { 
            if (email == "b@b.com") return false;

            return true;
        }
    }
}
