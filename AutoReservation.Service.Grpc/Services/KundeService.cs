using AutoReservation.BusinessLayer;
using AutoReservation.Dal.Entities;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoReservation.Service.Grpc.Services
{
    internal class KundeService : Grpc.KundeService.KundeServiceBase
    {
        private readonly ILogger<KundeService> _logger;

        public KundeService(ILogger<KundeService> logger)
        {
            _logger = logger;
        }

        public override async Task<KundeDto> GetById(GetKundeByIdRequest request, ServerCallContext context)
        {
            KundeManager manager = new KundeManager();
            Kunde kunde = await manager.GetById(request.Id);
            KundeDto result = kunde.ConvertToDto();
            return result;
        }

        public override async Task<AllKunden> GetAll(Empty request, ServerCallContext context)
        {
            KundeManager manager = new KundeManager();
            List<Kunde> kundenList = await manager.GetAll();
            AllKunden allKundenDto = new AllKunden();
            foreach (Kunde k in kundenList)
            {
                allKundenDto.Kunde.Add(k.ConvertToDto());
            }
            return allKundenDto;
        }

        public override async Task<KundeDto> Insert(KundeDto request, ServerCallContext context)
        {
            try
            {
                KundeManager manager = new KundeManager();
                Kunde kunde = await manager.Insert(request.ConvertToEntity());
                KundeDto result = kunde.ConvertToDto();
                return result;
            }
            catch (BusinessLayer.Exceptions.OptimisticConcurrencyException<Auto> e)
            {
                throw new RpcException(new Status(StatusCode.Aborted, e.Message));
            }
        }

        public override async Task<KundeDto> Update(KundeDto request, ServerCallContext context)
        {
            try
            {
                KundeManager manager = new KundeManager();
                Kunde kunde = await manager.Update(request.ConvertToEntity());
                KundeDto result = kunde.ConvertToDto();
                return result;
            }
            catch (BusinessLayer.Exceptions.OptimisticConcurrencyException<Auto> e)
            {
                throw new RpcException(new Status(StatusCode.Aborted, e.Message));
            }
        }

        public override async Task<Empty> Delete(KundeDto request, ServerCallContext context)
        {
            try
            {
                KundeManager manager = new KundeManager();
                await manager.Delete(request.ConvertToEntity());
                return new Empty();
            }
            catch (BusinessLayer.Exceptions.OptimisticConcurrencyException<Auto> e)
            {
                throw new RpcException(new Status(StatusCode.Aborted, e.Message));
            }
        }
    }
}
