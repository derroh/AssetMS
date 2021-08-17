using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetMS
{
    public static class TestPassword
    {
        public static bool ValidatePassword(string password)
        {
            int validConditions = 0;

            foreach(char c in password)
            {
                if (c >='a' && c <='z')
                {
                    validConditions++;
                    break;
                }
            }
            foreach (char c in password)
            {
                if (c >= 'A' && c <= 'Z')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 0) return false;
            foreach (char c in password)
            {
                if (c >= '0' && c <= '9')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 1) return false;

            if (validConditions == 2)
            {
                char[] special = { '@', '#', '$', '%', '^', '&', '*', '+', '=', '-', '!', '(', ')' };

                if (password.IndexOfAny(special) == -1) return false;
            }

            return true;
        }
    }
}
