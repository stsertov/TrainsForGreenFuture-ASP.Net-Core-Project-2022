namespace TrainsForGreenFuture.Controllers.Api.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Api.Locomotives;

    [ApiController]
    [Route("api/locomotives")]
    public class LocomotivesApiController : ControllerBase
    {
        private ILocomotiveService service;
        private IMapper mapper;
        public LocomotivesApiController(ILocomotiveService service,
            IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("all")]
        public ActionResult<IEnumerable<LocomotiveResponseModel>> Locomotives()
            => mapper.Map<List<LocomotiveResponseModel>>(service.AllLocomotives());
        
    }
}
