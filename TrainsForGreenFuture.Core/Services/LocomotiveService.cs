namespace TrainsForGreenFuture.Core.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Interrails;
    using TrainsForGreenFuture.Core.Models.Locomotives;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public class LocomotiveService : ILocomotiveService
    {
        private TrainsDbContext context;
        private IMapper mapper;

        public LocomotiveService(IMapper mapper, TrainsDbContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IEnumerable<LocomotiveViewModel> AllLocomotives()
        {
            var dbTrains = context.Locomotives
                .Include(l => l.Interrail)
                .Where(l => !l.IsForRenovation)
                .ToList();

            var trains = mapper.Map<IEnumerable<LocomotiveViewModel>>(dbTrains);

            return trains;
        }

        public IEnumerable<InterrailServiceModel> AllInterrails()
        {
            var interrails = context.Interrails.ToList();

            return mapper.Map<List<InterrailServiceModel>>(interrails);
        }

        public int Create(
            string model, 
            int year, 
            int series, 
            EngineType engineType, 
            int interrailId, 
            int topSpeed, 
            string picture, 
            string description, 
            decimal price)
        {
            var locomotive = new Locomotive
            {
                Model = model,
                Year = year,
                Series = series,
                EngineType = engineType,
                InterrailId = interrailId,
                TopSpeed = topSpeed,
                Picture = picture,
                Description = description,
                Price = price
            };

            context.Locomotives.Add(locomotive);
            context.SaveChanges();

            return locomotive.Id;
        }

        public bool Edit(
            int id,
            string model, 
            int year, 
            int series, 
            EngineType engineType, 
            int interrailId, 
            int topSpeed, 
            string picture, 
            string description, 
            decimal price)
        {
            var locomotive = GetLocomotive(id);

            if (locomotive == null)
            {
                return false;
            }

            locomotive.Model = model;
            locomotive.Year = year;
            locomotive.Series = series;
            locomotive.EngineType = engineType;
            locomotive.InterrailId = interrailId;
            locomotive.TopSpeed = topSpeed;
            locomotive.Picture = picture;
            locomotive.Description = description;
            locomotive.Price = price;

            context.SaveChanges();

            return true;
        }

        public LocomotiveViewModel Details(int id)
        {
            var dbLocomotive = GetLocomotive(id);

            if(dbLocomotive == null)
                return null;

            var locomotive = mapper.Map<LocomotiveViewModel>(dbLocomotive);

            return locomotive;
        }

        public bool Remove(int id)
        {
            var locomotive = GetLocomotive(id);

            if(locomotive == null)
            {
                return false;
            }

            context.Locomotives.Remove(locomotive);
            context.SaveChanges();

            return true;
        }

        private Locomotive GetLocomotive(int id)
            => context.Locomotives
                .Include(l => l.Interrail)
                .FirstOrDefault(l => !l.IsForRenovation && l.Id == id);

    }
}