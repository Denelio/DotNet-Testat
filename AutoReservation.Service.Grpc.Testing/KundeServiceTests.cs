﻿using System;
using System.Threading.Tasks;
using AutoReservation.Service.Grpc.Testing.Common;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Xunit;

namespace AutoReservation.Service.Grpc.Testing
{
    public class KundeServiceTests
        : ServiceTestBase
    {
        private readonly KundeService.KundeServiceClient _target;

        public KundeServiceTests(ServiceTestFixture serviceTestFixture)
            : base(serviceTestFixture)
        {
            _target = new KundeService.KundeServiceClient(Channel);
        }

        [Fact]
        public async Task GetKundenTest()
        {
            var result = await _target.GetAllAsync(new Empty());
            Assert.Equal(4, result.Kunde.Count);
        }

        [Fact]
        public async Task GetKundeByIdTest()
        {
            var result = await _target.GetByIdAsync(new GetKundeByIdRequest { Id = 1 });
            Assert.Equal(1, result.Id);
            Assert.Equal("Anna", result.Vorname);
            Assert.Equal("Nass", result.Nachname);
        }

        [Fact]
        public async Task GetKundeByIdWithIllegalIdTest()
        {
            Assert.Throws<RpcException>(() => _target.GetById(new GetKundeByIdRequest { Id = 42 }));
        }

        [Fact]
        public async Task InsertKundeTest()
        {
            var kundeDto = new KundeDto { Geburtsdatum = new DateTime(1807, 12, 1, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(), Vorname = "Banuel", Nachname = "Mauer" };

            var kunde = await _target.InsertAsync(kundeDto);

            Assert.Equal(kundeDto.Vorname, kunde.Vorname);
            Assert.Equal(kundeDto.Nachname, kunde.Nachname);
            Assert.Equal(kundeDto.Geburtsdatum, kunde.Geburtsdatum);
        }

        [Fact]
        public async Task DeleteKundeTest()
        {
            var kundeRequest = new GetKundeByIdRequest { Id = 1 };
            var kunde = await _target.GetByIdAsync(kundeRequest);
            _target.Delete(kunde);
            Assert.Throws<RpcException>(() => _target.GetById(kundeRequest));
        }

        [Fact]
        public async Task UpdateKundeTest()
        {
            var kunde = await _target.GetByIdAsync(new GetKundeByIdRequest { Id = 1 });
            kunde.Vorname = "JuanPabloFernandezRodriguezPaeliaTorres";
            var updatedKunde = _target.Update(kunde);
            Assert.Equal(kunde.Vorname, updatedKunde.Vorname);
        }

        [Fact]
        public async Task UpdateKundeWithOptimisticConcurrencyTest()
        {
            var kunde = await _target.GetByIdAsync(new GetKundeByIdRequest { Id = 1 });
            var kunde2 = kunde;

            kunde.Vorname = "Wilfried";
            kunde2.Vorname = "Sigismund";

            await _target.UpdateAsync(kunde);

            Assert.Throws<RpcException>(() => _target.Update(kunde2));
        }
    }
}