using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace WhatsAppApi.Register
{
    class WaToken
    {
        public static string GenerateToken(string number, string platform)
        {
            string token = "";

            if (platform == "Android")
            {
                byte[] signature = Convert.FromBase64String("MIIDMjCCAvCgAwIBAgIETCU2pDALBgcqhkjOOAQDBQAwfDELMAkGA1UEBhMCVVMxEzARBgNVBAgTCkNhbGlmb3JuaWExFDASBgNVBAcTC1NhbnRhIENsYXJhMRYwFAYDVQQKEw1XaGF0c0FwcCBJbmMuMRQwEgYDVQQLEwtFbmdpbmVlcmluZzEUMBIGA1UEAxMLQnJpYW4gQWN0b24wHhcNMTAwNjI1MjMwNzE2WhcNNDQwMjE1MjMwNzE2WjB8MQswCQYDVQQGEwJVUzETMBEGA1UECBMKQ2FsaWZvcm5pYTEUMBIGA1UEBxMLU2FudGEgQ2xhcmExFjAUBgNVBAoTDVdoYXRzQXBwIEluYy4xFDASBgNVBAsTC0VuZ2luZWVyaW5nMRQwEgYDVQQDEwtCcmlhbiBBY3RvbjCCAbgwggEsBgcqhkjOOAQBMIIBHwKBgQD9f1OBHXUSKVLfSpwu7OTn9hG3UjzvRADDHj+AtlEmaUVdQCJR+1k9jVj6v8X1ujD2y5tVbNeBO4AdNG/yZmC3a5lQpaSfn+gEexAiwk+7qdf+t8Yb+DtX58aophUPBPuD9tPFHsMCNVQTWhaRMvZ1864rYdcq7/IiAxmd0UgBxwIVAJdgUI8VIwvMspK5gqLrhAvwWBz1AoGBAPfhoIXWmz3ey7yrXDa4V7l5lK+7+jrqgvlXTAs9B4JnUVlXjrrUWU/mcQcQgYC0SRZxI+hMKBYTt88JMozIpuE8FnqLVHyNKOCjrh4rs6Z1kW6jfwv6ITVi8ftiegEkO8yk8b6oUZCJqIPf4VrlnwaSi2ZegHtVJWQBTDv+z0kqA4GFAAKBgQDRGYtLgWh7zyRtQainJfCpiaUbzjJuhMgo4fVWZIvXHaSHBU1t5w//S0lDK2hiqkj8KpMWGywVov9eZxZy37V26dEqr/c2m5qZ0E+ynSu7sqUD7kGx/zeIcGT0H+KAVgkGNQCo5Uc0koLRWYHNtYoIvt5R3X6YZylbPftF/8ayWTALBgcqhkjOOAQDBQADLwAwLAIUAKYCp0d6z4QQdyN74JDfQ2WCyi8CFDUM4CaNB+ceVXdKtOrNTQcc0e+t");
                byte[] classesMd5 = Convert.FromBase64String("7UDPOXwpiLBvEjT8uNwsuA=="); // PERSVxyRE03RRwC3TrED+g==  // 2.12.291
                byte[] key2 = Convert.FromBase64String("eQV5aq/Cg63Gsq1sshN9T3gh+UUp0wIw0xgHYT1bnCjEqOJQKCRrWxdAe2yvsDeCJL+Y4G3PRD2HUF7oUgiGo8vGlNJOaux26k+A2F3hj8A="); //  /UIGKU1FVQa+ATM2A0za7G2KI9S/CwPYjgAbc67v7ep42eO/WeTLx1lb1cHwxpsEgF4+PmYpLd2YpGUdX/A2JQitsHzDwgcdBpUf7psX1BU=");
                byte[] opad = WhatsApp.SYSEncoding.GetBytes(new string('\x5c', 64));
                byte[] ipad = WhatsApp.SYSEncoding.GetBytes(new string('\x36', 64));
                for (int i = 0; i < 64; i++)
                {
                    opad[i] = (byte)(opad[i] ^ key2[i]);
                    ipad[i] = (byte)(ipad[i] ^ key2[i]);
                }

                List<byte> d1 = new List<byte>();
                d1.AddRange(ipad);
                d1.AddRange(signature);
                d1.AddRange(classesMd5);
                d1.AddRange(WhatsApp.SYSEncoding.GetBytes(number));

                byte[] t1 = SHA1.Create().ComputeHash(d1.ToArray());

                List<byte> d2 = new List<byte>();
                d2.AddRange(opad);
                d2.AddRange(t1);

                byte[] t2 = SHA1.Create().ComputeHash(d2.ToArray());

                token = Convert.ToBase64String(t2);

            }

            return token;

        }
         
        private static List<byte> GetFilledList(byte item, int length)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < length; i++)
            {
                result.Add(item);
            }
            return result;
        }

        public static string Base64Encode(string plainText)
        {
            //PHP is using ISO Latin 1, which is code page 28591:
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData, Encoding enc)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            //PHP is using ISO Latin 1, which is code page 28591:
            return enc.GetString(base64EncodedBytes);
        }

        static byte[] Hash(string input)
        {
            return SHA1.Create().ComputeHash(Encoding.GetEncoding(28591).GetBytes(input));
        }
    }
}
