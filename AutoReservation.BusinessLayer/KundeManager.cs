using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoReservation.BusinessLayer
{
    public class KundeManager
        : ManagerBase
    {
        public async Task<List<Kunde>> GetAll()
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Kunden.ToListAsync();
        }

        public async Task<Kunde> GetById(int i)
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Kunden.SingleOrDefaultAsync(k => k.Id == i);
        }

        public async Task<Kunde> Insert(Kunde kunde)
        {
            using AutoReservationContext context = new AutoReservationContext();
            try
            {
                context.Entry(kunde).State = EntityState.Added;
                await context.SaveChangesAsync();
                return kunde;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw CreateOptimisticConcurrencyException(context, kunde);
            }

        }

        public async Task<Kunde> Update(Kunde kunde)
        {
            using AutoReservationContext context = new AutoReservationContext();
            try
            {
                context.Entry(kunde).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return kunde;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw CreateOptimisticConcurrencyException(context, kunde);
            }
        }

        public async Task Delete(Kunde kunde)
        {
            using AutoReservationContext context = new AutoReservationContext();
            try
            {
                context.Entry(kunde).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw CreateOptimisticConcurrencyException(context, kunde);
            }

        }

    }
}