using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AWSRedisDemo.Controllers
{
    public class GeoPerformancesController : ApiController
    {
        // GET: api/GeoPerformances
        public IEnumerable<string> Get()
        {
            var host = ConfigurationManager.AppSettings["RedisHost"];
            var service = new GeoService(host);
            var toReturn = service.GetValues();

            return toReturn;
        }
    }
}
