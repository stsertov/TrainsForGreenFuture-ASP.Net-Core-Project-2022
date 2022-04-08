namespace TrainsForGreenFuture.Controllers.Api.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Api.TrainCars;

    [ApiController]
    [Route("api/traincars")]
    public class TrainCarsApiController : ControllerBase
    {
        private TrainDbContext service;
        private IMapper mapper;
        public TrainCarsApiController(TrainDbContext service,
            IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("all")]
        public ActionResult<IEnumerable<TrainCarResponseModel>> TrainCars()
            => mapper.Map<List<TrainCarResponseModel>>(service.AllTrainCars());
    }
}
