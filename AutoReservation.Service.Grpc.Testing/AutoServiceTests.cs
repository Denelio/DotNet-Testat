using System;
using System.Threading.Tasks;
using AutoReservation.Service.Grpc.Testing.Common;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Xunit;
using static AutoReservation.Service.Grpc.AutoDto.Types;

namespace AutoReservation.Service.Grpc.Testing
{
    public class AutoServiceTests
        : ServiceTestBase
    {
        private readonly AutoService.AutoServiceClient _target;

        public AutoServiceTests(ServiceTestFixture serviceTestFixture)
            : base(serviceTestFixture)
        {
            _target = new AutoService.AutoServiceClient(Channel);
        }


        [Fact]
        public async Task GetAutosTest()
        {
            var result = await _target.GetAllAsync(new Empty());
            Assert.Equal(4, result.Auto.Count);
        }

        [Fact]
        public async Task GetAutoByIdTest()
        {
            var result = await _target.GetByIdAsync(new GetAutoByIdRequest { Id = 1 });
            Assert.Equal(1, result.Id);
            Assert.Equal("Fiat Punto", result.Marke);
            Assert.Equal(50, result.Tagestarif);
            Assert.Equal(AutoKlasse.Standard, result.AutoKlasse);
        }

        [Fact]
        public async Task GetAutoByIdWithIllegalIdTest()
        {
            Assert.Throws<RpcException>(() => _target.GetById(new GetAutoByIdRequest { Id = 42 }));
        }

        [Fact]
        public async Task InsertAutoTest()
        {
            var autoDto = new AutoDto { AutoKlasse = AutoKlasse.Luxusklasse, Basistarif = 2077, Marke = "Cybertruck", Tagestarif = 2077 };

            var auto = await _target.InsertAsync(autoDto);

            Assert.Equal(autoDto.AutoKlasse, auto.AutoKlasse);
            Assert.Equal(autoDto.Basistarif, auto.Basistarif);
            Assert.Equal(autoDto.Marke, auto.Marke);
            Assert.Equal(autoDto.Tagestarif, auto.Tagestarif);
        }

        [Fact]
        public async Task DeleteAutoTest()
        {
            var autoRequest = new GetAutoByIdRequest { Id = 1 };
            var auto = await _target.GetByIdAsync(autoRequest);
            _target.Delete(auto);
            Assert.Throws<RpcException>(() => _target.GetById(autoRequest));
        }

        [Fact]
        public async Task UpdateAutoTest()
        {
            var auto = await _target.GetByIdAsync(new GetAutoByIdRequest { Id = 1 });
            auto.Marke = "Cybertruck";
            var updatedAuto = _target.Update(auto);
            Assert.Equal(auto.Marke, updatedAuto.Marke);
        }

        [Fact]
        public async Task UpdateAutoWithOptimisticConcurrencyTest()
        {
            var auto = await _target.GetByIdAsync(new GetAutoByIdRequest { Id = 1 });
            var auto2 = auto;

            auto.Tagestarif = 100;
            auto2.Tagestarif = 200;

            await _target.UpdateAsync(auto);

            Assert.Throws<RpcException>(() => _target.Update(auto2));
        }
    }
}