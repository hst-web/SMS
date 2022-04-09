using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace HST.Art.Web
{
    [Serializable]
    public class PlatformAgent
    {
        private PlatformAgent() { }
        private static PlatformAgent _instance;
        private static readonly object _locker = new object();
        public static PlatformAgent Get()
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        _instance = new PlatformAgent();
                    }
                }
            }

            return _instance;
        }
        public static void Set(string jsonStr)
        {
            _instance = JsonConvert.DeserializeObject<PlatformAgent>(jsonStr);
        }
        public string UserAgent { get; set; }
        public string Platform { get; set; }
        public string Version { get; set; }
        public string BrowserName { get; set; }
        public string BrowserType { get; set; }
        public string IPAddress { get; set; }


    }
}