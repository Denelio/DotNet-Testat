using AutoReservation.BusinessLayer;
using AutoReservation.Dal.Entities;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoReservation.Service.Grpc.Services
{
    internal class ReservationService : Grpc.ReservationService.ReservationServiceBase
    {
        private readonly ILogger<ReservationService> _logger;

        public ReservationService(ILogger<ReservationService> logger)
        {
            _logger = logger;
        }

        public override async Task<ReservationDto> GetById(GetReservationByIdRequest request, ServerCallContext context)
        {
            ReservationManager manager = new ReservationManager();
            Reservation reservation = await manager.GetById(request.Id);
            ReservationDto result = reservation.ConvertToDto();
            return result;
        }

        public override async Task<AllReservations> GetAll(Empty request, ServerCallContext context)
        {
            ReservationManager manager = new ReservationManager();
            List<Reservation> reservationList = await manager.GetAll();
            AllReservations allReservationDto = new AllReservations();
            foreach (Reservation k in reservationList)
            {
                allReservationDto.Reservation.Add(k.ConvertToDto());
            }
            return allReservationDto;
        }

        public override async Task<ReservationDto> Insert(ReservationDto request, ServerCallContext context)
        {
            try
            {
                ReservationManager manager = new ReservationManager();
                Reservation reservation = await manager.Insert(request.ConvertToEntity());
                ReservationDto result = reservation.ConvertToDto();
                return result;
            }
            catch(Exception e)
            {
                throw new RpcException(new Status(StatusCode.Aborted, e.Message));
            }
        }

        public override async Task<ReservationDto> Update(ReservationDto request, ServerCallContext context)
        {
            try
            {
                ReservationManager manager = new ReservationManager();
                Reservation reservation = await manager.Update(request.ConvertToEntity());
                ReservationDto result = reservation.ConvertToDto();
                return result;
            }
            catch (Exception e)
            {
                throw new RpcException(new Status(StatusCode.Aborted, e.Message));
            }
        }

        public override async Task<Empty> Delete(ReservationDto request, ServerCallContext context)
        {
            try
            {
                ReservationManager manager = new ReservationManager();
                await manager.Delete(request.ConvertToEntity());
                return new Empty();
            }
            catch (Exception e)
            {
                throw new RpcException(new Status(StatusCode.Aborted, e.Message));
            }
        }
    }
}
