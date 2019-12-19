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
            return await context.Reservationen.Include(r => r.Auto).Include(r => r.Kunde).ToListAsync();
        }

        public async Task<Reservation> GetById(int i)
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Reservationen.Include(r => r.Auto).Include(r => r.Kunde).SingleOrDefaultAsync(r => r.ReservationsNr == i);
        }

        public async Task<Reservation> Insert(Reservation reservation)
        {
            using AutoReservationContext context = new AutoReservationContext();
            try
            {
                if (!await IsReservationValid(reservation))
                {
                    throw new InvalidDateRangeException();
                }
                if (HasCollision(reservation))
                {
                    throw new AutoUnavailableException();
                }

                context.Entry(reservation).State = EntityState.Added;
                await context.SaveChangesAsync();
                return await context.Reservationen.Include(r => r.Auto).Include(r => r.Kunde).SingleOrDefaultAsync(r => r.ReservationsNr == reservation.ReservationsNr);
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
                if (!await IsReservationValid(reservation))
                {
                    throw new InvalidDateRangeException();
                }
                if (HasCollision(reservation))
                {
                    throw new AutoUnavailableException();
                }

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

        //public durch test requirements --> bessere Lösung?
        public async Task<bool> IsReservationValid(Reservation reservation)
        {
            return (reservation.Bis - reservation.Von).TotalHours >= 24 && reservation.Von < reservation.Bis;
        }

        public bool HasCollision(Reservation reservation)
        {
            using var context = new AutoReservationContext();
            return context.Reservationen.Where(r => r.ReservationsNr != reservation.ReservationsNr).Any(r => r.AutoId == reservation.AutoId &&
            ((r.Von < reservation.Von && r.Bis > reservation.Von)
            || (r.Von < reservation.Bis && r.Bis > reservation.Bis)
            || (r.Von == reservation.Von)));
        }
    }
}