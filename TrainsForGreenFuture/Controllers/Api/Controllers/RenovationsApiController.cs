namespace TrainsForGreenFuture.Controllers.Api.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Api.Renovations;

    [ApiController]
    [Route("api/renovations")]
    public class RenovationsApiController : ControllerBase
    {
        private IRenovationService service;
        private IMapper mapper;
        public RenovationsApiController(IRenovationService service,
           IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("all")]
        public ActionResult<IEnumerable<RenovationResponseModel>> Renovations()
            => mapper.Map<List<RenovationResponseModel>>(service.ApiRenovations());
    }
}
