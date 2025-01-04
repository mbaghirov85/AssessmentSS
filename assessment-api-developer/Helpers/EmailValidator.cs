using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AssessmentPlatformDeveloper.Helpers {

    public class EmailValidator {

        public static bool IsValidEmail(string email) {
            System.Diagnostics.Debug.WriteLine(email);
            if (string.IsNullOrEmpty(email))
                return false;

            string emailPattern = @"^[a-zA-Z0-9._%\-]+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(emailPattern);
            System.Diagnostics.Debug.WriteLine(regex.IsMatch(email));

            return regex.IsMatch(email);
        }
    }
}