using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Text.RegularExpressions;

namespace Genshine_Gacha_Recorder_Win.Utils
{
    class Requests
    {
        private const string DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受   
        }

        private static bool IsHttps(string url)
        {
            Regex re = new Regex(@"https://*");
            return re.IsMatch(url);
        }

        public static HttpWebResponse Get(string url, Encoding charset, string UserAgent = DefaultUserAgent)
        {
            HttpWebRequest request = null;
            CookieContainer cookie = new CookieContainer();
            if (IsHttps(url))
            {
                //HTTPSQ请求
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            }
            request = WebRequest.Create(url) as HttpWebRequest;
            request.CookieContainer = cookie;
            request.ProtocolVersion = HttpVersion.Version11;
            request.Method = "GET";
            request.UserAgent = UserAgent;
            request.KeepAlive = true;
            request.Headers["Accept-Language"] = "zh-CN,zh;q=0.9,en-GB;q=0.8,en;q=0.7";

            return (HttpWebResponse)request.GetResponse();
        }

        public static HttpWebResponse Post(string url, IDictionary<string, string> parameters, Encoding charset, string UserAgent = DefaultUserAgent)
        {
            HttpWebRequest request = null;
            CookieContainer cookie = new CookieContainer();
            if (IsHttps(url))
            {
                //HTTPSQ请求
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            }
            request = WebRequest.Create(url) as HttpWebRequest;
            request.CookieContainer = cookie;
            request.ProtocolVersion = HttpVersion.Version11;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = UserAgent;
            request.KeepAlive = true;
            request.Headers["Accept-Language"] = "zh-CN,zh;q=0.9";
            //request.Headers["Cookie"] = "username=aaaaaa; Language=zh_CN";
            //如果需要POST数据   
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, WebUtility.UrlEncode(parameters[key]));
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, WebUtility.UrlEncode(parameters[key]));
                    }
                    i++;
                }
                byte[] data = charset.GetBytes(buffer.ToString());
                using Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
            }

            return (HttpWebResponse)request.GetResponse();
        }
    }
}
