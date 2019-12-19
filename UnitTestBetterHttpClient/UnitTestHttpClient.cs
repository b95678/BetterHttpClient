using System;
using System.Collections.Specialized;
using System.Text;
using BetterHttpClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace UnitTestBetterHttpClient
{
    [TestClass]
    public class UnitTestHttpClient
    {
        private const string HttpsProxy = "210.245.25.229:3128";
        private const string Socksproxy = "124.207.126.18:1080";
        [TestMethod]
        public void TestGet()
        {
            HttpClient client = new HttpClient
            {
                UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0"
            };

            string page = client.Get("http://www.google.com");
            Assert.IsTrue(page.Contains("<title>Google</title>"));
        }

        [TestMethod]
        public void TestPost()
        {
            HttpClient client = new HttpClient
            {
                UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0",
                ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                
            };
            string page = client.Post("http://www.newrank.cn/xdnphb/detail/getAccountArticle", new NameValueCollection
            {
                { "flag","true"},
                {"uuid","DCFCC585F745FF5401FD33213B1178EB" },
                { "nonce","0f3ac361b" },
                {"xyz","4d86efaf832909233403482879de5cdd" }
            });
            Assert.IsTrue(!string.IsNullOrEmpty(page));
            //Form root = JsonConvert.DeserializeObject<RootObject>(page).form;

            //Assert.AreEqual(root.custname, customerName);
            //Assert.AreEqual(root.custtel, phone);
            //Assert.AreEqual(root.custemail, email);
            //Assert.AreEqual(root.size, size);
            //Assert.AreEqual(root.topping, topping);
            //Assert.AreEqual(root.delivery, delivery);
            //Assert.AreEqual(root.comments, comments);

        }

        [TestMethod]
        public void TestUserAgent()
        {
            HttpClient client = new HttpClient
            {
                UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0"
            };

            string page = client.Get("https://httpbin.org/user-agent");
            Assert.IsTrue(page.Contains(client.UserAgent));
        }

        [TestMethod]
        public void TestGzipDecodingAndReferer()
        {
            HttpClient client = new HttpClient
            {
                UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0",
                Referer = "https://httpbin.org/"
            };

            string page = client.Get("https://httpbin.org/gzip");
            Assert.IsTrue(page.Contains(client.UserAgent));
            // check for referer
            Assert.IsTrue(page.Contains("https://httpbin.org/"));
        }

        [TestMethod]
        public void TestHttpProxy()
        {
            HttpClient client = new HttpClient(new Proxy(HttpsProxy))
            {
                UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0",
                Encoding = Encoding.GetEncoding("iso-8859-2"),
                AcceptEncoding = "gzip"
            };

            string page = client.Get("http://darkwarez.pl");
            Assert.IsTrue(page.Contains("Polskie Forum Warez! Najnowsze linki"));
        }
        [TestMethod]
        public void TestHttpsProxy()
        {
            HttpClient client = new HttpClient(new Proxy(HttpsProxy))
            {
                UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0"
            };

            string page = client.Get("https://httpbin.org/get");
            Assert.IsTrue(page.Contains(client.UserAgent));
        }
        [TestMethod]
        public void TestSocks5Proxy()
        {
            Proxy proxy = new Proxy("47.94.19.105", 3001, ProxyTypeEnum.Socks);
            HttpClient client = new HttpClient(proxy)
            {
                UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0",
                Encoding = Encoding.GetEncoding("iso-8859-2"),
                AcceptEncoding = "deflate"
            };

            string page = client.Get("https://httpbin.org/get");
            Assert.IsTrue(page.Contains("darkwarez.pl - Gry, Muzyka, Filmy, Download"));
        }
        [TestMethod]
        public void TestSocks4Proxy()
        {
            string ip = "61.142.72.150";
            Proxy proxy = new Proxy(ip, 33235, ProxyTypeEnum.Socks4);
            
            HttpClient client = new HttpClient(proxy)
            {
                UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0",
                Encoding = Encoding.GetEncoding("utf-8"),
                //AcceptEncoding = "deflate"
            };

            string page = client.Get("http://httpbin.org/get");
            Assert.IsTrue(page.Contains(ip));
        }
        [TestMethod]
        public void TestSocks5ProxyGetIPFromProxyServer()
        {
            Proxy proxy = new Proxy("92.222.196.248", 1080, ProxyTypeEnum.Socks);
            HttpClient client = new HttpClient(proxy, Encoding.UTF8, true)
            {
                UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0",
                Encoding = Encoding.GetEncoding("utf-8"),
                //AcceptEncoding = "deflate"
            };
            string page = client.Get("https://www.youtube.com");
            Assert.IsTrue(page.Contains("<title>YouTube</title>"));
        }
        [TestMethod]
        public void TestSocksHttpsProxyDeflateEncoding()
        {
            HttpClient client = new HttpClient(new Proxy(Socksproxy))
            {
                UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0",
                AcceptEncoding = "deflate"
            };

            string page = client.Get("https://httpbin.org/get");
            Assert.IsTrue(page.Contains(client.UserAgent));
        }
        [TestMethod]
        public void TestSocksHttpsProxyGzipEndcoding()
        {
            HttpClient client = new HttpClient(new Proxy(Socksproxy))
            {
                UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0",
                AcceptEncoding = "gzip"
            };

            string page = client.Get("https://httpbin.org/get");
            Assert.IsTrue(page.Contains(client.UserAgent));
        }
        public class Form
        {
            public string comments { get; set; }
            public string custemail { get; set; }
            public string custname { get; set; }
            public string custtel { get; set; }
            public string delivery { get; set; }
            public string size { get; set; }
            public string topping { get; set; }
        }

        public class RootObject
        {
            public object args { get; set; }
            public string data { get; set; }
            public object files { get; set; }
            public Form form { get; set; }
            public object headers { get; set; }
            public object json { get; set; }
            public string origin { get; set; }
            public string url { get; set; }
        }
    }
}
