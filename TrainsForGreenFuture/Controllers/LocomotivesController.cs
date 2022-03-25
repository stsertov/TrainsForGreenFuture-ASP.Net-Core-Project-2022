namespace TrainsForGreenFuture.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;
    using TrainsForGreenFuture.Models.Locomotives;

    public class LocomotivesController : Controller
    {
        private TrainsDbContext context;
        private IMapper mapper;
        public LocomotivesController(TrainsDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public IActionResult All()
        {
            var dbTrains = context.Locomotives
                .Where(l => !l.IsForRenovation);

            var trains = mapper.Map<IEnumerable<LocomotiveViewModel>>(dbTrains.ToList());

            return View(trains);
        }

        public IActionResult Add()
            => View();

        [HttpPost]
        public IActionResult Add(LocomotiveFormModel locomotive)
        {
            if (!Enum.TryParse<EngineType>(locomotive.EngineType, out EngineType parsedEngineType))
            {
                ModelState.AddModelError(locomotive.EngineType, "We do not offer this engine type.");
            }


            if (!ModelState.IsValid)
            {
                return View(locomotive);
            }

            var dbLocomotive = new Locomotive
            {
                Model = locomotive.Model,
                Year = locomotive.Year,
                Series = locomotive.Series,
                EngineType = parsedEngineType,
                InterrailId = 2,
                TopSpeed = locomotive.TopSpeed,
                Picture = locomotive.Picture,
                Description = locomotive.Description,
                Price = locomotive.Price
            };

            context.Locomotives.Add(dbLocomotive);
            context.SaveChanges();

            return Redirect("/Locomotives/All");
        }


        public IActionResult Details(int id)
        {
            var dbLocomotive = context.Locomotives
                .FirstOrDefault(l => !l.IsForRenovation && l.Id == id);

            var locomotive = mapper.Map<LocomotiveViewModel>(dbLocomotive);

            if (locomotive == null)
            {
                return Redirect("/Locomotives/All");
            }

            return View(locomotive);
        }

        public IActionResult Order(int id)
        {
            var dbLocomotive = context.Locomotives
                .FirstOrDefault(l => !l.IsForRenovation && l.Id == id);

            var locomotive = mapper.Map<LocomotiveViewModel>(dbLocomotive);

            if (locomotive == null)
            {
                return Redirect("/Locomotives/All");
            }

            return View(locomotive);
        }

        [HttpPost]
        public IActionResult Order()
        {

            return Redirect("/Home/Trains");
        }
    }
}
