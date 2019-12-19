using AutoReservation.BusinessLayer;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal.Entities;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoReservation.Service.Grpc.Services
{
    internal class AutoService : Grpc.AutoService.AutoServiceBase
    {
        private readonly ILogger<AutoService> _logger;

        public AutoService(ILogger<AutoService> logger)
        {
            _logger = logger;
        }

        public override async Task<AutoDto> GetById(GetAutoByIdRequest request, ServerCallContext context)
        {
            AutoManager manager = new AutoManager();
            Auto auto = await manager.GetById(request.Id);
            AutoDto result = auto.ConvertToDto();
            return result;
        }

        public override async Task<AllAutos> GetAll(Empty request, ServerCallContext context)
        {
            AutoManager manager = new AutoManager();
            List<Auto> autoList = await manager.GetAll();
            AllAutos allAutoDto = new AllAutos();
            foreach (Auto k in autoList)
            {
                allAutoDto.Auto.Add(k.ConvertToDto());
            }
            return allAutoDto;
        }

        public override async Task<AutoDto> Insert(AutoDto request, ServerCallContext context)
        {
            try
            {
                AutoManager manager = new AutoManager();
                Auto auto = await manager.Insert(request.ConvertToEntity());
                AutoDto result = auto.ConvertToDto();
                return result;
            }
            catch (OptimisticConcurrencyException<Auto> e)
            {
                throw new RpcException(new Status(StatusCode.Aborted, e.Message));
            }
        }

        public override async Task<AutoDto> Update(AutoDto request, ServerCallContext context)
        {
            try
            {
                AutoManager manager = new AutoManager();
                Auto auto = await manager.Update(request.ConvertToEntity());
                AutoDto result = auto.ConvertToDto();
                return result;
            }
            catch (OptimisticConcurrencyException<Auto> e)
            {
                throw new RpcException(new Status(StatusCode.Aborted, e.Message));
            }
        }

        public override async Task<Empty> Delete(AutoDto request, ServerCallContext context)
        {
            try
            {
                AutoManager manager = new AutoManager();
                await manager.Delete(request.ConvertToEntity());
                return new Empty();
            }
            catch (OptimisticConcurrencyException<Auto> e)
            {
                throw new RpcException(new Status(StatusCode.Aborted, e.Message));
            }
        }
    }
}
