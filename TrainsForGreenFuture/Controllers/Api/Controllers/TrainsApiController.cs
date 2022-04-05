namespace TrainsForGreenFuture.Controllers.Api.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Api.Trains;

    [ApiController]
    [Route("api/trains")]
    public class TrainsApiController : ControllerBase
    {
        private ITrainService service;
        private IMapper mapper;
        public TrainsApiController(ITrainService service,
            IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("all")]
        public ActionResult<IEnumerable<TrainResponseModel>> Trains()
            => mapper.Map<List<TrainResponseModel>>(service.AllTrains());
    }
}
