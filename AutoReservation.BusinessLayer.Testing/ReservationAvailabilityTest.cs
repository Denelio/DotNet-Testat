using System;
using System.Threading.Tasks;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationAvailabilityTest
        : TestBase
    {
        private readonly ReservationManager _target;

        private Reservation _reservation = new Reservation
        {
            Von = new DateTime(2020, 3, 10),
            Bis = new DateTime(2020, 3, 20),
            AutoId = 2,
            KundeId = 1
        };

        public ReservationAvailabilityTest()
        {
            _target = new ReservationManager();
        }

        [Fact]
        public async Task ScenarioOkay01Test()
        {
            // arrange
            //| ---Date 1--- |
            //               | ---Date 2--- |
            Reservation reservation = new Reservation
            {
                Von = new DateTime(2020, 3, 20),
                Bis = new DateTime(2020, 3, 30),
                AutoId = 2,
                KundeId = 1
            };

            // act
            await _target.Insert(_reservation);
            bool result = _target.HasCollision(reservation);

            // assert
            Assert.False(result);


        }

        [Fact]
        public async Task ScenarioOkay02Test()
        {
            // arrange
            //| ---Date 1--- |
            //                 | ---Date 2--- |
            Reservation reservation = new Reservation
            {
                Von = new DateTime(2020, 4, 10),
                Bis = new DateTime(2020, 4, 20),
                AutoId = 2,
                KundeId = 1
            };

            // act
            await _target.Insert(_reservation);
            bool result = _target.HasCollision(reservation);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task ScenarioOkay03Test()
        {
            // arrange
            //                | ---Date 1--- |
            //| ---Date 2-- - |
            Reservation reservation = new Reservation
            {
                Von = new DateTime(2020, 3, 1),
                Bis = new DateTime(2020, 3, 10),
                AutoId = 2,
                KundeId = 1
            };

            // act
            await _target.Insert(_reservation);
            bool result = _target.HasCollision(reservation);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task ScenarioOkay04Test()
        {
            // arrange
            //                | ---Date 1--- |
            //| ---Date 2--- |
            Reservation reservation = new Reservation
            {
                Von = new DateTime(2020, 1, 20),
                Bis = new DateTime(2020, 2, 29),
                AutoId = 2,
                KundeId = 1
            };

            // act
            await _target.Insert(_reservation);
            bool result = _target.HasCollision(reservation);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task ScenarioNotOkay01Test()
        {
            // arrange
            //| ---Date 1--- |
            //    | ---Date 2--- |
            Reservation reservation = new Reservation
            {
                Von = new DateTime(2020, 3, 17),
                Bis = new DateTime(2020, 3, 25),
                AutoId = 2,
                KundeId = 1
            };

            // act
            await _target.Insert(_reservation);
            bool result = _target.HasCollision(reservation);

            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task ScenarioNotOkay02Test()
        {
            // arrange
            //    | ---Date 1--- |
            //| ---Date 2--- |
            Reservation reservation = new Reservation
            {
                Von = new DateTime(2020, 3, 5),
                Bis = new DateTime(2020, 3, 15),
                AutoId = 2,
                KundeId = 1
            };

            // act
            await _target.Insert(_reservation);
            bool result = _target.HasCollision(reservation);

            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task ScenarioNotOkay03Test()
        {
            // arrange
            //| ---Date 1--- |
            //| --------Date 2-------- |
            Reservation reservation = new Reservation
            {
                Von = new DateTime(2020, 3, 10),
                Bis = new DateTime(2020, 3, 30),
                AutoId = 2,
                KundeId = 1
            };

            // act
            await _target.Insert(_reservation);
            bool result = _target.HasCollision(reservation);

            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task ScenarioNotOkay04Test()
        {
            // arrange
            //| --------Date 1-------- |
            //| ---Date 2--- |
            Reservation reservation = new Reservation
            {
                Von = new DateTime(2020, 3, 10),
                Bis = new DateTime(2020, 3, 15),
                AutoId = 2,
                KundeId = 1
            };

            // act
            await _target.Insert(_reservation);
            bool result = _target.HasCollision(reservation);

            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task ScenarioNotOkay05Test()
        {
            // arrange
            //| ---Date 1--- |
            //| ---Date 2--- |
            Reservation reservation = new Reservation
            {
                Von = new DateTime(2020, 3, 10),
                Bis = new DateTime(2020, 3, 20),
                AutoId = 2,
                KundeId = 1
            };

            // act
            await _target.Insert(_reservation);
            bool result = _target.HasCollision(reservation);

            // assert
            Assert.True(result);
        }
    }
}
