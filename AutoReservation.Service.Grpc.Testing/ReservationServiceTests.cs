using System;
using System.Threading.Tasks;
using AutoReservation.Service.Grpc.Testing.Common;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Xunit;
using static AutoReservation.Service.Grpc.AutoDto.Types;

namespace AutoReservation.Service.Grpc.Testing
{
    public class ReservationServiceTests
        : ServiceTestBase
    {
        private readonly ReservationService.ReservationServiceClient _target;
        private readonly AutoService.AutoServiceClient _autoClient;
        private readonly KundeService.KundeServiceClient _kundeClient;

        public ReservationServiceTests(ServiceTestFixture serviceTestFixture)
            : base(serviceTestFixture)
        {
            _target = new ReservationService.ReservationServiceClient(Channel);
            _autoClient = new AutoService.AutoServiceClient(Channel);
            _kundeClient = new KundeService.KundeServiceClient(Channel);
        }

        [Fact]
        public async Task GetReservationenTest()
        {
            var result = await _target.GetAllAsync(new Empty());
            Assert.Equal(4, result.Reservation.Count);
        }

        [Fact]
        public async Task GetReservationByIdTest()
        {
            var result = await _target.GetByIdAsync(new GetReservationByIdRequest { Id = 1 });
            Assert.Equal(1, result.ReservationsNr);
        }

        [Fact]
        public async Task GetReservationByIdWithIllegalIdTest()
        {
            Assert.Throws<RpcException>(() => _target.GetById(new GetReservationByIdRequest { Id = 42 }));
        }

        [Fact]
        public async Task InsertReservationTest()
        {
            AutoDto autoDto = await _autoClient.GetByIdAsync(new GetAutoByIdRequest { Id = 1 });
            KundeDto kundeDto = await _kundeClient.GetByIdAsync(new GetKundeByIdRequest { Id = 1 });
            ReservationDto reservationDto = new ReservationDto
            {
                Von = new DateTime(1807, 1, 11, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(),
                Bis = new DateTime(1807, 1, 14, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(),
                RowVersion = Google.Protobuf.ByteString.CopyFromUtf8(""),
                Auto = autoDto,
                Kunde = kundeDto
            };
            ReservationDto reservation = await _target.InsertAsync(reservationDto);
            //ReservationDto reservationFromDb = await _target.GetByIdAsync(new GetReservationByIdRequest { Id = 5 });

            Assert.Equal(reservationDto.Auto, reservation.Auto);
            Assert.Equal(reservationDto.Kunde, reservation.Kunde);
            Assert.Equal(reservationDto.Von, reservation.Von);
            Assert.Equal(reservationDto.Bis, reservation.Bis);
        }

        [Fact]
        public async Task DeleteReservationTest()
        {
            var reservationRequest = new GetReservationByIdRequest { Id = 2 };
            var reservation = await _target.GetByIdAsync(reservationRequest);
            await _target.DeleteAsync(reservation);
            Assert.Throws<RpcException>(() => _target.GetById(reservationRequest));
        }

        [Fact]
        public async Task UpdateReservationTest()
        {
            var reservation = await _target.GetByIdAsync(new GetReservationByIdRequest { Id = 3 });
            reservation.Bis = new DateTime(2020, 1, 30, 0, 0, 0, DateTimeKind.Utc).ToTimestamp();
            var updatedReservation = await _target.UpdateAsync(reservation);
            Assert.Equal(reservation.Bis, updatedReservation.Bis);
        }

        [Fact]
        public async Task UpdateReservationWithOptimisticConcurrencyTest()
        {
            var reservation = await _target.GetByIdAsync(new GetReservationByIdRequest { Id = 1 });
            var reservation2 = reservation;

            reservation.Bis = new DateTime(2100, 1, 11, 0, 0, 0, DateTimeKind.Utc).ToTimestamp();
            reservation2.Bis = new DateTime(2110, 1, 11, 0, 0, 0, DateTimeKind.Utc).ToTimestamp();

            await _target.UpdateAsync(reservation);

            Assert.Throws<RpcException>(() => _target.Update(reservation2));
        }

        [Fact]
        public async Task InsertReservationWithInvalidDateRangeTest()
        {
            AutoDto autoDto = await _autoClient.GetByIdAsync(new GetAutoByIdRequest { Id = 1 });
            KundeDto kundeDto = await _kundeClient.GetByIdAsync(new GetKundeByIdRequest { Id = 1 });
            ReservationDto reservationDto = new ReservationDto
            {
                Von = new DateTime(1808, 1, 11, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(),
                Bis = new DateTime(1807, 1, 12, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(),
                RowVersion = Google.Protobuf.ByteString.CopyFromUtf8(""),
                Auto = autoDto,
                Kunde = kundeDto
            };
            Assert.Throws<RpcException>(() => _target.Insert(reservationDto)); 
        }

        [Fact]
        public async Task InsertReservationWithAutoNotAvailableTest()
        {
            AutoDto autoDto = await _autoClient.GetByIdAsync(new GetAutoByIdRequest { Id = 1 });
            KundeDto kundeDto = await _kundeClient.GetByIdAsync(new GetKundeByIdRequest { Id = 1 });
            ReservationDto reservationDto = new ReservationDto
            {
                Von = new DateTime(2020, 1, 11, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(),
                Bis = new DateTime(2020, 1, 28, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(),
                RowVersion = Google.Protobuf.ByteString.CopyFromUtf8(""),
                Auto = autoDto,
                Kunde = kundeDto
            };
            Assert.Throws<RpcException>(() => _target.Insert(reservationDto));
        }

        [Fact]
        public async Task UpdateReservationWithInvalidDateRangeTest()
        {
            var reservation = await _target.GetByIdAsync(new GetReservationByIdRequest { Id = 1 });

            reservation.Bis = new DateTime(1111, 1, 11, 0, 0, 0, DateTimeKind.Utc).ToTimestamp();

            Assert.Throws<RpcException>(() => _target.Update(reservation));
        }

        [Fact]
        public async Task UpdateReservationWithAutoNotAvailableTest()
        {
            var reservation = await _target.GetByIdAsync(new GetReservationByIdRequest { Id = 1 });

            reservation.Von = new DateTime(2020, 5, 20, 0, 0, 0, DateTimeKind.Utc).ToTimestamp();

            Assert.Throws<RpcException>(() => _target.Update(reservation));
        }

        [Fact]
        public async Task CheckAvailabilityIsTrueTest()
        {
            AutoDto autoDto = await _autoClient.GetByIdAsync(new GetAutoByIdRequest { Id = 1 });
            KundeDto kundeDto = await _kundeClient.GetByIdAsync(new GetKundeByIdRequest { Id = 1 });
            ReservationDto reservationDto = new ReservationDto
            {
                Von = new DateTime(2100, 1, 11, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(),
                Bis = new DateTime(2100, 1, 14, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(),
                RowVersion = Google.Protobuf.ByteString.CopyFromUtf8(""),
                Auto = autoDto,
                Kunde = kundeDto
            };

            Assert.True(_target.CarAvailability(reservationDto).IsAvailable);
        }

        [Fact]
        public async Task CheckAvailabilityIsFalseTest()
        {
            AutoDto autoDto = await _autoClient.GetByIdAsync(new GetAutoByIdRequest { Id = 1 });
            KundeDto kundeDto = await _kundeClient.GetByIdAsync(new GetKundeByIdRequest { Id = 1 });
            ReservationDto reservationDto = new ReservationDto
            {
                Von = new DateTime(2020, 1, 30, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(),
                Bis = new DateTime(2020, 1, 20, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(),
                RowVersion = Google.Protobuf.ByteString.CopyFromUtf8(""),
                Auto = autoDto,
                Kunde = kundeDto
            };

            Assert.False(_target.CarAvailability(reservationDto).IsAvailable);
        }
    }
}