using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoReservation.BusinessLayer
{
    public class AutoManager
        : ManagerBase
    {
        public async Task<List<Auto>> GetAll()
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Autos.ToListAsync();
        }

        public async Task<Auto> GetById(int i)
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Autos.SingleOrDefaultAsync(a => a.Id == i);
        }

        public async Task<Auto> Insert(Auto auto)
        {
            using AutoReservationContext context = new AutoReservationContext();
            context.Entry(auto).State = EntityState.Added;
            await context.SaveChangesAsync();
            return auto;
        }

        public async Task<Auto> Update(Auto auto)
        {
            throw new NotImplementedException("Test not implemented.");
        }

        public async Task Delete(Auto auto)
        {
            using AutoReservationContext context = new AutoReservationContext();
            context.Entry(auto).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }
    }
}