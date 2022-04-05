namespace TrainsForGreenFuture.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Locomotives;

    public class LocomotivesController : Controller
    {
        private const string LocomotivesCacheKey = "locomotivesall";
        private ILocomotiveService service;
        private IMemoryCache cache;
        public LocomotivesController(ILocomotiveService service,
            IMemoryCache cache)
        {
            this.service = service;
            this.cache = cache;
        }
        public IActionResult All()
        {
            var locomotives = cache.Get<IEnumerable<LocomotiveViewModel>>(LocomotivesCacheKey);

            if (locomotives == null)
            {
                locomotives = service.AllLocomotives();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromDays(5));

                cache.Set(LocomotivesCacheKey, locomotives, cacheOptions);
            }

            return View(locomotives);
        }


        public IActionResult Details(int id)
        {
            var locomotive = service.Details(id);

            if (locomotive == null)
            {
                return Redirect("/Locomotives/All");
            }

            return View(locomotive);
        }
    }
}
