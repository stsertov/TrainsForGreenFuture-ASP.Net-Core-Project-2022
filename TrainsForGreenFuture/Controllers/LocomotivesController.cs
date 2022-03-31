namespace TrainsForGreenFuture.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Locomotives;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    using static Areas.RolesConstants;
    public class LocomotivesController : Controller
    {
        private IMapper mapper;
        private ILocomotiveService service;
        public LocomotivesController(IMapper mapper,
            ILocomotiveService service)
        {
            this.mapper = mapper;
            this.service = service;
        }
        public IActionResult All()
            => View(service.AllLocomotives());

        public IActionResult Add()
            => View(new LocomotiveFormModel { Interrails = service.AllInterrails() });

        [HttpPost]
        public IActionResult Add(LocomotiveFormModel locomotive)
        {
            if (!Enum.TryParse(locomotive.EngineType, out EngineType parsedEngineType))
            {
                ModelState.AddModelError(locomotive.EngineType, "We do not offer this engine type.");
            }

            if(!service.AllInterrails().Any(i => i.Id == locomotive.InterrailId))
            {
                ModelState.AddModelError("Invalid Interrail", "Interrail is invalid.");
            }

            if (!ModelState.IsValid)
            { 
                locomotive.Interrails = service.AllInterrails();
                return View(locomotive);
            }

            var locomotiveId = service.Create(
                locomotive.Model,
                locomotive.Year.Value,
                locomotive.Series.Value,
                parsedEngineType,
                locomotive.InterrailId.Value,
                locomotive.TopSpeed.Value,
                locomotive.Picture,
                locomotive.Description,
                locomotive.Price.Value);
            
            TempData["GlobalMessage"] = $"Locomotive {locomotive.Model} / {locomotive.Series} was added!";

            return Redirect("/Locomotives/All");
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


        [Authorize(Roles = $"{AdministratorRole}, {EngineerRole}")]
        public IActionResult Edit(int id)
        {

            var dbLocomotive = service.Details(id);

            if (dbLocomotive == null)
            {
                return Redirect("/Locomotives/All");
            }

            var locomotive = mapper.Map<LocomotiveFormModel>(dbLocomotive);

            locomotive.Interrails = service.AllInterrails();

            return View(locomotive);
        }

        [HttpPost]
        [Authorize(Roles = $"{AdministratorRole}, {EngineerRole}")]
        public IActionResult Edit(int id, LocomotiveFormModel locomotive)
        {
            if (!Enum.TryParse(locomotive.EngineType, out EngineType parsedEngineType))
            {
                ModelState.AddModelError(locomotive.EngineType, "We do not offer this engine type.");
            }

            if (!ModelState.IsValid)
            {
                locomotive.Interrails = service.AllInterrails();
                return View(locomotive);
            }

            var isEdited = service.Edit(
                id,
                locomotive.Model,
                locomotive.Year.Value,
                locomotive.Series.Value,
                parsedEngineType,
                locomotive.InterrailId.Value,
                locomotive.TopSpeed.Value,
                locomotive.Picture,
                locomotive.Description,
                locomotive.Price.Value
                );

            if(!isEdited)
            {
                locomotive.Interrails = service.AllInterrails();
                return View(locomotive);
            }

            return Redirect("/Locomotives/All");
        }

        

        [Authorize(Roles = AdministratorRole)]
        public IActionResult Delete(int id)
        {

            var isRemoved = service.Remove(id);

            if (isRemoved)
            {
                return Redirect("/Home/Trains");
            }

            return Redirect("/Locomotive/All");
        }
    }
}
