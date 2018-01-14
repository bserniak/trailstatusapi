using System.Linq;
using System.Web.Http;
using TrailStatusApi.Business;
using TrailStatusApi.Models;
using WebApi.OutputCache.V2;

namespace TrailStatusApi.Controllers
{
    public class StatusController : ApiController
    {
        private readonly IStatusParser _statusParser;
        private readonly IPageRetriever _pageRetriever;

        public StatusController(IStatusParser statusParser, IPageRetriever pageRetriever)
        {
            _statusParser = statusParser;
            _pageRetriever = pageRetriever;
        }

        // Obtain all trail information
        [CacheOutput(ServerTimeSpan = 500)]
        public TrailResponse Get()
        {
            var document = _pageRetriever.GetPage("https://gorctrails.com/trails");

            var trails = document.DocumentNode.SelectNodes("//li")
                .Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("views-row"))
                .Select(li => new Trail(_statusParser.ExtractName(li), _statusParser.ExtractArea(li), _statusParser.ExtractDirectUrl(li), _statusParser.EvaluateStatus(li)));
            return new TrailResponse { Trails = trails.ToList() };
        }
    }
}
