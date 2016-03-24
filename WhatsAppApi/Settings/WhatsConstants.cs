using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace WhatsAppApi.Settings
{
    /// <summary>
    /// Holds constant information used to connect to whatsapp server
    /// </summary>
    public class WhatsConstants
    {
        #region ServerConstants

        /// <summary>
        /// The whatsapp host
        /// </summary>
        public const string WhatsAppHost = "c3.whatsapp.net";

        /// <summary>
        /// The whatsapp XMPP realm
        /// </summary>
        public const string WhatsAppRealm = "s.whatsapp.net";

        /// <summary>
        /// The whatsapp server
        /// </summary>
        public const string WhatsAppServer = "s.whatsapp.net";

        /// <summary>
        /// The whatsapp CheckHost
        /// </summary>
        public const string WhatsAppCheckHost = "v.whatsapp.net/v2/exist";

        /// <summary>
        /// The whatsapp RegisterHost
        /// </summary>
        public const string WhatsAppRegisterHost = "v.whatsapp.net/v2/register";

        /// <summary>
        /// The whatsapp RequerstHost
        /// </summary>
        public const string WhatsAppRequestHost = "v.whatsapp.net/v2/code";

        /// <summary>
        /// The whatsapp group chat server
        /// </summary>
        public const string WhatsGroupChat = "g.us";

        /// <summary>
        /// The port that needs to be connected to
        /// </summary>
        public const int WhatsPort = 443;

        /// <summary>
        /// Device
        /// </summary>
        public const string Device = "armani";

        /// <summary>
        /// The whatsapp version the client complies to
        /// </summary>
        public const string WhatsAppVer = "2.12.440";

        /// <summary>
        /// OS Version
        /// </summary>
        public const string OsVersion = "4.3";

        /// <summary>
        /// Manufacturer
        /// </summary>
        public const string Manufacturer = "Xiaomi";

        /// <summary>
        /// BuildVersion
        /// </summary>
        public const string BuildVersion = "JLS36C";
        
        /// <summary>
        /// Platform
        /// </summary>
        public const string Platform = "Android";

        /// <summary>
        /// The useragent used for http requests
        /// </summary>
		public const string UserAgent = "WhatsApp/2.12.440 Android/4.3 Device/Xiaomi-HM_1SW";

        #endregion

        #region ParserConstants
        /// <summary>
        /// The number style used
        /// </summary>
        public static NumberStyles WhatsAppNumberStyle = (NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);

        /// <summary>
        /// Unix epoch DateTime
        /// </summary>
        public static DateTime UnixEpoch = new DateTime(0x7b2, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        #endregion
    }
}
