using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoReservation.BusinessLayer
{
    public class ReservationManager
        : ManagerBase
    {
        public async Task<List<Reservation>> GetAll()
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Reservationen.ToListAsync();
        }

        public async Task<Reservation> GetById(int i)
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Reservationen.SingleOrDefaultAsync(r => r.ReservationsNr == i);
        }

        public async Task<Reservation> Insert(Reservation reservation)
        {
            using AutoReservationContext context = new AutoReservationContext();
            try
            {
                if (!isReservationValid(reservation))
                {
                    throw new InvalidDateRangeException();
                }

                context.Entry(reservation).State = EntityState.Added;
                await context.SaveChangesAsync();
                return reservation;

            }
            catch (DbUpdateConcurrencyException)
            {
                throw CreateOptimisticConcurrencyException(context, reservation);
            }

        }

        public async Task<Reservation> Update(Reservation reservation)
        {
            using AutoReservationContext context = new AutoReservationContext();
            try
            {
                context.Entry(reservation).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return reservation;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw CreateOptimisticConcurrencyException(context, reservation);
            }
        }

        public async Task Delete(Reservation reservation)
        {
            using AutoReservationContext context = new AutoReservationContext();
            try
            {
                context.Entry(reservation).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw CreateOptimisticConcurrencyException(context, reservation);
            }

        }

        public bool isReservationValid(Reservation reservation)
        {
            return (reservation.Bis - reservation.Von).TotalHours >= 24 & reservation.Von < reservation.Bis;
        }

        public bool isAvailable(Reservation reservation)
        {
            return true;
        }
    }
}