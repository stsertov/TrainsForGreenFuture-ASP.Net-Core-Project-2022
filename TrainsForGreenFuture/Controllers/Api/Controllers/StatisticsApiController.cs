namespace TrainsForGreenFuture.Controllers.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Api.Statistics;

    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : ControllerBase
    {
        private IStatisticsService service;

        public StatisticsApiController(IStatisticsService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("all")]
        public StatisticsServiceModel AllCounts()
            => service.GetInfo();
    }
}
