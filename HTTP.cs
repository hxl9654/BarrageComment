﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
// *   This program is free software: you can redistribute it and/or modify
// *   it under the terms of the GNU General Public License as published by
// *   the Free Software Foundation, either version 3 of the License, or
// *   (at your option) any later version.
// *
// *   This program is distributed in the hope that it will be useful,
// *   but WITHOUT ANY WARRANTY; without even the implied warranty of
// *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// *   GNU General Public License for more details.
// *
// *   You should have received a copy of the GNU General Public License
// *   along with this program.  If not, see <http://www.gnu.org/licenses/>.
// *
// * @author     Xianglong He
// * @copyright  Copyright (c) 2015 Xianglong He. (http://tec.hxlxz.com)
// * @license    http://www.gnu.org/licenses/     GPL v3
// * @version    1.0
// * @discribe   RuiRuiQQRobot服务端
// * 本软件作者是何相龙，使用GPL v3许可证进行授权。
namespace SmartQQ
{

    public static class HTTP
    {
        //网络通信相关
        public static CookieContainer cookies = new CookieContainer();
        static CookieCollection CookieCollection = new CookieCollection();
        static CookieContainer CookieContainer = new CookieContainer();

        static public string HeartPackdata;
        static int AmountOfRunningPosting = 0;
        public static string HttpGet(string url, int timeout = 100000, Encoding encode = null, string referer = "http://d.web2.qq.com/proxy.html?v=20130916001&callback=1&id=2")
        {
            string dat;
            HttpWebResponse res = null;
            HttpWebRequest req;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(url);
                req.CookieContainer = cookies;
                req.Timeout = timeout;
                req.Referer = referer;
                res = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException)
            {
                return "";
            }
            StreamReader reader;
            if (encode != null)
                reader = new StreamReader(res.GetResponseStream(), encode);
            else
                reader = new StreamReader(res.GetResponseStream());
            dat = reader.ReadToEnd();
            res.Close();
            req.Abort();

            return dat;
        }
        //http://www.itokit.com/2012/0721/74607.html
        public static string HttpPost(string url, string Referer, string data, Encoding encode, bool SaveCookie, int timeout = 100000)
        {
            string dat = "";
            HttpWebRequest req;
            if (AmountOfRunningPosting == 0)
                System.GC.Collect();
            AmountOfRunningPosting++;
            try
            {
                req = WebRequest.Create(url) as HttpWebRequest;
                req.CookieContainer = cookies;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Method = "POST";
                req.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:30.0) Gecko/20100101 Firefox/30.0";
                req.Proxy = null;
                req.Timeout = timeout;
                req.ProtocolVersion = HttpVersion.Version10;
                if (!string.IsNullOrEmpty(Referer))
                    req.Referer = Referer;
                byte[] mybyte = Encoding.Default.GetBytes(data);
                req.ContentLength = mybyte.Length;

                Stream stream = req.GetRequestStream();
                stream.Write(mybyte, 0, mybyte.Length);


                HttpWebResponse hwr = req.GetResponse() as HttpWebResponse;
                stream.Close();
                if (SaveCookie)
                {
                    CookieCollection = hwr.Cookies;
                    cookies.GetCookies(req.RequestUri);
                }
                StreamReader SR = new StreamReader(hwr.GetResponseStream(), encode);
                dat = SR.ReadToEnd();
                hwr.Close();
                req.Abort();
            }

            catch (WebException)
            {
                AmountOfRunningPosting--;
                return "";
            }
            AmountOfRunningPosting--;
            return dat;
        }

    }
}