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
            var result = _target.GetAll(new Empty());
            Assert.Equal(4, result.Reservation.Count);
        }

        [Fact]
        public async Task GetReservationByIdTest()
        {
            var result = _target.GetById(new GetReservationByIdRequest { Id = 1 });
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
            AutoDto autoDto = _autoClient.GetById(new GetAutoByIdRequest { Id = 4 });
            KundeDto kundeDto = _kundeClient.GetById(new GetKundeByIdRequest { Id = 1 });
            ReservationDto reservationDto = new ReservationDto
            {
                Von = new DateTime(1807, 11, 1, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(),
                Bis = new DateTime(1807, 12, 1, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(),
                RowVersion = Google.Protobuf.ByteString.CopyFromUtf8(""),
                Auto = autoDto,
                Kunde = kundeDto
            };
            ReservationDto reservation = _target.Insert(reservationDto);
            ReservationDto reservationFromDb = _target.GetById(new GetReservationByIdRequest { Id = 5 });

            Assert.Equal(reservation, reservationFromDb);
        }

        [Fact]
        public async Task DeleteReservationTest()
        {
            var reservationRequest = new GetReservationByIdRequest { Id = 2 };
            var reservation = _target.GetById(reservationRequest);
            _target.Delete(reservation);
            Assert.Throws<RpcException>(() => _target.GetById(reservationRequest));
        }

        [Fact]
        public async Task UpdateReservationTest()
        {
            var reservation = _target.GetById(new GetReservationByIdRequest { Id = 1 });
            reservation.Bis = new DateTime(2100, 11, 1, 0, 0, 0, DateTimeKind.Utc).ToTimestamp();
            var updatedReservation = _target.Update(reservation);
            Assert.Equal(reservation.Bis, updatedReservation.Bis);
        }

        [Fact]
        public async Task UpdateReservationWithOptimisticConcurrencyTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task InsertReservationWithInvalidDateRangeTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task InsertReservationWithAutoNotAvailableTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task UpdateReservationWithInvalidDateRangeTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task UpdateReservationWithAutoNotAvailableTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task CheckAvailabilityIsTrueTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task CheckAvailabilityIsFalseTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }
    }
}