using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Newtonsoft;
using System.Collections.Generic;

[assembly: OwinStartup(typeof(AWSRedisDemo.Startup))]

namespace AWSRedisDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute("DefaultAPI",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            app.UseWebApi(config);
        }
    }

    public class Dummy
    {
        public string Id { get; set; }
        public string Message { get; set; }
    }

    public class GeoService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ICacheClient _client;

        public GeoService(string host)
        {
            _redis = ConnectionMultiplexer.Connect(host);

            var serializer = new NewtonsoftSerializer();
            _client = new StackExchangeRedisCacheClient(_redis, serializer);

            Seed();
        }

        public void Seed()
        {
            var dummy = new Dummy() { Id = "bob", Message = "Writes all the data here." };
           _client.Add<Dummy>(dummy.Id, dummy);
        }

        public List<string> GetValues()
        {
            var toReturn = new List<string>();
            var result = _client.Get<Dummy>("bob");

            toReturn.Add(result.Message);

            return toReturn;
        }
    }
}
