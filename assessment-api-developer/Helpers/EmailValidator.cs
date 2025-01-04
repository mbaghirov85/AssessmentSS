using System.Text.RegularExpressions;

namespace assessment_platform_developer.Helpers {

    public class EmailValidator {

        public static bool IsValidEmail(string email) {
            if (string.IsNullOrEmpty(email))
                return false;

            string emailPattern = @"^[a-zA-Z0-9._%\-]+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(emailPattern);

            return regex.IsMatch(email);
        }
    }
}