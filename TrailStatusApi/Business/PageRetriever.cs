using System.Net;
using HtmlAgilityPack;

namespace TrailStatusApi.Business
{
    public interface IPageRetriever
    {
        HtmlDocument GetPage(string url);
    }

    public class PageRetriever : IPageRetriever
    {
        private readonly HtmlWeb _retriever;
        public PageRetriever()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            _retriever = new HtmlWeb();
        }
        public HtmlDocument GetPage(string url)
        {
            return _retriever.Load(url);
        }
    }
}