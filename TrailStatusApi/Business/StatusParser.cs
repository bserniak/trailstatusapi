using System.Linq;
using System.Net;
using HtmlAgilityPack;
using TrailStatusApi.Models;

namespace TrailStatusApi.Business
{
    public interface IStatusParser
    {
        Status EvaluateStatus(HtmlNode listItem);
        string ExtractArea(HtmlNode listItem);
        string ExtractDirectUrl(HtmlNode listItem);
        string ExtractName(HtmlNode listItem);
    }

    public class StatusParser : IStatusParser
    {
        public string ExtractName(HtmlNode listItem)
        {
            return WebUtility.HtmlDecode(listItem.ChildNodes.Last(x => x.Name == "a").InnerText);
        }

        public string ExtractArea(HtmlNode listItem)
        {
            return listItem.ParentNode.ParentNode.ChildNodes.Single(x => x.Name == "h3").InnerText;
        }

        public string ExtractDirectUrl(HtmlNode listItem)
        {
            return listItem.ChildNodes.Last(x => x.Name == "a").Attributes.Select(y => y.Value).Single();
        }

        public Status EvaluateStatus(HtmlNode listItem)
        {
            var image = listItem.ChildNodes.First(x => x.Name == "a").FirstChild.Attributes.Single().Value;
            if (image.Contains("green")) return Status.Good;
            if (image.Contains("yellow")) return Status.Caution;
            return image.Contains("red") ? Status.Bad : Status.Unknown;
        }
    }
}