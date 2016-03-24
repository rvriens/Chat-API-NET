using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using WhatsAppApi.Parser;
using WhatsAppApi.Settings;

namespace WhatsAppApi.Register
{
    public static class WhatsRegisterV2
    {
        public static string GenerateIdentity(string phoneNumber, string salt = "")
        {
            return (phoneNumber + salt).Reverse().ToSHAString().ToLower();
        }

        public static string GetToken(string number, string platform)
        {
            return WaToken.GenerateToken(number, WhatsConstants.Platform );
        }

        public static bool RequestCode(string phoneNumber, out string password, string method = "sms", string id = null)
        {
            string response = string.Empty;
            return RequestCode(phoneNumber, out password, out response, method, id);
        }

        public static bool RequestCode(string phoneNumber, out string password, out string response, string method = "sms", string id = null)
        {
            string request = string.Empty;
            return RequestCode(phoneNumber, out password, out request, out response, method, id);
        }

        public static bool RequestCode(string phoneNumber, out string password, out string request, out string response, string method = "sms", string id = null)
        {
            response = null;
            password = null;
            request = null;
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    //auto-generate
                    id = GenerateIdentity(phoneNumber);
                }
                PhoneNumber pn = new PhoneNumber(phoneNumber);
                string token = System.Uri.EscapeDataString(WhatsRegisterV2.GetToken(pn.Number, WhatsConstants.Platform ));

                Dictionary<string, string> query = new Dictionary<string, string>();

                query.Add("cc", pn.CC);
                query.Add("in", pn.Number);
                query.Add("lg", pn.ISO639);
                query.Add("lc", pn.ISO3166);
                query.Add("id", id);
                query.Add("token", token);
                query.Add("mistyped", "6");
                query.Add("network_radio_type", "1");
                query.Add("simnum", "1");
                query.Add("s", "");
                query.Add("copiedrc", "1");
                query.Add("hasinrc", "1");
                query.Add("rcmatch", "1");
                query.Add("pid", ((new Random()).Next(9899) + 100).ToString());
                query.Add("rchash", GetRandom(20).ToSHA256String());
                query.Add("anhash", GetRandom(20).ToMD5String());
                query.Add("extexist", "1");
                query.Add("extstate", "1");
                query.Add("mcc", pn.MCC);
                query.Add("mnc", pn.MNC);
                query.Add("sim_mcc", pn.MCC);
                query.Add("sim_mnc", pn.MNC);
                query.Add("method", method);

                string uri = BuildUrl(WhatsConstants.WhatsAppRequestHost, query);
                Console.WriteLine(uri);
                response = GetResponse(uri);
                password = response.GetJsonValue("pw");
                if (!string.IsNullOrEmpty(password))
                {
                    return true;
                }
                return (response.GetJsonValue("status") == "sent");
            }
            catch (Exception e)
            {
                response = e.Message;
                return false;
            }
        }

        public static string RegisterCode(string phoneNumber, string code, string id = null)
        {
            string response = string.Empty;
            return WhatsRegisterV2.RegisterCode(phoneNumber, code, out response, id);
        }

        public static string RegisterCode(string phoneNumber, string code, out string response, string id = null)
        {
            response = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    //auto generate
                    id = GenerateIdentity(phoneNumber);
                }
                PhoneNumber pn = new PhoneNumber(phoneNumber);

                Dictionary<string, string> query = new Dictionary<string, string>();
                query.Add("cc", pn.CC);
                query.Add("in", pn.Number);
                query.Add("lg", pn.ISO639);
                query.Add("lc", pn.ISO3166);
                query.Add("id", id);
                query.Add("mistyped", "6");
                query.Add("network_radio_type", "1");
                query.Add("simnum", "1");
                query.Add("s", "");
                query.Add("copiedrc", "1");
                query.Add("hasinrc", "1");
                query.Add("rcmatch", "1");
                query.Add("pid", ((new Random()).Next(9899) + 100).ToString());
                query.Add("rchash", GetRandom(20).ToSHA256String());
                query.Add("anhash", GetRandom(20).ToMD5String());
                query.Add("extexist", "1");
                query.Add("extstate", "1");
                // query.Add("method", "sms");
                query.Add("code", code);

                string uri = BuildUrl(WhatsConstants.WhatsAppRegisterHost, query);
				response = GetResponse(uri);
                if (response.GetJsonValue("status") == "ok")
                {
                    return response.GetJsonValue("pw");
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static string RequestExist(string phoneNumber, string id = null)
        {
            string response = string.Empty;
            return RequestExist(phoneNumber, out response, id);
        }

        public static string RequestExist(string phoneNumber, out string response, string id = null)
        {
            response = string.Empty;
            try
            {
                if (String.IsNullOrEmpty(id))
                {
                    id = GenerateIdentity(phoneNumber);
                }
                PhoneNumber pn = new PhoneNumber(phoneNumber);

                Dictionary<string, string> query = new Dictionary<string, string>();
                query.Add("cc", pn.CC);
                query.Add("in", pn.Number);
                query.Add("lg", pn.ISO639);
                query.Add("lc", pn.ISO3166);
                query.Add("id", id);
                query.Add("mistyped", "6");
                query.Add("network_radio_type", "1");
                query.Add("simnum", "1");
                query.Add("s", "");
                query.Add("copiedrc", "1");
                query.Add("hasinrc", "1");
                query.Add("rcmatch", "1");
                query.Add("pid", ((new Random()).Next(9899) + 100).ToString());
                query.Add("extexist", "1");
                query.Add("extstate", "1");
                
                string uri = BuildUrl(WhatsConstants.WhatsAppCheckHost, query);
                response = GetResponse(uri);
                if (response.GetJsonValue("status") == "ok")
                {
                    return response.GetJsonValue("pw");
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        private static string BuildUrl(string host, Dictionary<string, string> query)
        {
            string querystr = String.Join("&", (from q in query select String.Format("{0}={1}", q.Key, q.Value)).ToArray());
            return string.Format("https://{0}?{1}", host, querystr);
        }

        private static string GetResponse(string uri)
        {
            HttpWebRequest request = HttpWebRequest.Create(new Uri(uri)) as HttpWebRequest;
            request.KeepAlive = false;
            request.UserAgent = WhatsConstants.UserAgent;
            request.Accept = "text/json";
            using (var reader = new System.IO.StreamReader(request.GetResponse().GetResponseStream()))
            {
                return reader.ReadLine();
            }
        }

        private static string ToSHAString(this IEnumerable<char> s)
        {
            return new string(s.ToArray()).ToSHAString();
        }

        public static string UrlEncode(string data)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in data.ToCharArray())
            {
                int i = (int)c;
                if (
                    (
                        i >= 0 && i <= 31
                    )
                    ||
                    (
                        i >= 32 && i <= 47
                    )
                    ||
                    (
                        i >= 58 && i <= 64
                    )
                    ||
                    (
                        i >= 91 && i <= 96
                    )
                    ||
                    (
                        i >= 123 && i <= 126
                    )
                    ||
                    i > 127
                )
                {
                    //encode 
                    sb.Append('%'); 
                    sb.AppendFormat("{0:x2}", (byte)c); 
                }
                else
                {
                    //do not encode
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        private static byte[] GetRandom(int len)
        {
            byte[] bytes = new byte[len];
            (new Random()).NextBytes(bytes);
            return bytes;
        }

        private static string ToSHAString(this string s)
        {
            byte[] data = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(s));
            string str = Encoding.GetEncoding("iso-8859-1").GetString(data);
            str = WhatsRegisterV2.UrlEncode(str).ToLower();
            return str;
        }

        private static string ToSHA256String(this byte[] bytes)
        {
            byte[] data = SHA256.Create().ComputeHash(bytes);
            return string.Join(string.Empty, data.Select(q => q.ToString("x2")).ToArray());
        }

        private static string ToMD5String(this byte[] bytes)
        {
            return string.Join(string.Empty, MD5.Create().ComputeHash(bytes).Select(item => item.ToString("x2")).ToArray());
        }

        private static string ToMD5String(this IEnumerable<char> s)
        {
            return new string(s.ToArray()).ToMD5String();
        }
 
        private static string ToMD5String(this string s)
        {
            return string.Join(string.Empty, MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(s)).Select(item => item.ToString("x2")).ToArray());
        }


        private static void GetLanguageAndLocale(this CultureInfo self, out string language, out string locale)
        {
            string name = self.Name;
            int n1 = name.IndexOf('-');
            if (n1 > 0)
            {
                int n2 = name.LastIndexOf('-');
                language = name.Substring(0, n1);
                locale = name.Substring(n2 + 1);
            }
            else
            {
                language = name;
                switch (language)
                {
                    case "cs":
                        locale = "CZ";
                        return;

                    case "da":
                        locale = "DK";
                        return;

                    case "el":
                        locale = "GR";
                        return;

                    case "ja":
                        locale = "JP";
                        return;

                    case "ko":
                        locale = "KR";
                        return;

                    case "sv":
                        locale = "SE";
                        return;

                    case "sr":
                        locale = "RS";
                        return;
                }
                locale = language.ToUpper();
            }
        }

        private static string GetJsonValue(this string s, string parameter)
        {
            Match match;
            if ((match = Regex.Match(s, string.Format("\"?{0}\"?:\"(?<Value>.+?)\"", parameter), RegexOptions.Singleline | RegexOptions.IgnoreCase)).Success)
            {
                return match.Groups["Value"].Value;
            }
            return null;
        }
    }
}
