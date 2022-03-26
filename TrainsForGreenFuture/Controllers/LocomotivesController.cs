namespace TrainsForGreenFuture.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using TrainsForGreenFuture.Areas.Identity.Pages.Account;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;
    using TrainsForGreenFuture.Models.Locomotives;

    using static Areas.RolesConstants;
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
                .Where(l => !l.IsForRenovation)
                .ToList();

            var trains = mapper.Map<IEnumerable<LocomotiveViewModel>>(dbTrains);

            return View(trains);
        }

        public IActionResult Add()
            => View();

        [HttpPost]
        public IActionResult Add(LocomotiveFormModel locomotive)
        {
            if (!Enum.TryParse(locomotive.EngineType, out EngineType parsedEngineType))
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


        [Authorize(Roles = $"{AdministratorRole}, {EngineerRole}")]
        public IActionResult Edit(int id)
        {
            var dbLocomotive = context.Locomotives
                 .FirstOrDefault(l => l.Id == id && !l.IsForRenovation);

            if (dbLocomotive == null)
                return Redirect("/Locomotives/All");

            var locomotive = mapper.Map<LocomotiveFormModel>(dbLocomotive);

            return View(locomotive);
        }

        [HttpPost]
        [Authorize(Roles = $"{AdministratorRole}, {EngineerRole}")]
        public IActionResult Edit(int id, LocomotiveFormModel newLocomotive)
        {
            if (!Enum.TryParse(newLocomotive.EngineType, out EngineType parsedEngineType))
            {
                ModelState.AddModelError(newLocomotive.EngineType, "We do not offer this engine type.");
            }

            if (!ModelState.IsValid)
            {
                return View(newLocomotive);
            }

            var locomotive = context.Locomotives.FirstOrDefault(l => l.Id == id && !l.IsForRenovation);

            if (locomotive == null)
                return View(newLocomotive);

            locomotive.Model = newLocomotive.Model;
            locomotive.Year = newLocomotive.Year;
            locomotive.Series = newLocomotive.Series;
            locomotive.EngineType = parsedEngineType;
            locomotive.InterrailId = 2;
            locomotive.TopSpeed = newLocomotive.TopSpeed;
            locomotive.Picture = newLocomotive.Picture;
            locomotive.Description = newLocomotive.Description;
            locomotive.Price = newLocomotive.Price;

            context.SaveChanges();

            return Redirect("/Locomotives/All");
        }

        public IActionResult Order(int id)
        {
            if(!User.Identity.IsAuthenticated)
            {
                return Redirect($"/Identity/Account/Login/?returnUrl=/Locomotives/Order/{id}");
            }

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
        [Authorize]
        public IActionResult Order()
        {

            return Redirect("/Home/Trains");
        }
        
        [Authorize(Roles = AdministratorRole)]
        public IActionResult Delete(int id)
        {

            var locomotive = context.Locomotives
                .FirstOrDefault(l => l.Id == id);

            if (locomotive != null)
            {
                context.Locomotives.Remove(locomotive);
                context.SaveChanges();
                return Redirect("/Home/Trains");
            }

            return Redirect("/Locomotive/All");
        }
    }
}
