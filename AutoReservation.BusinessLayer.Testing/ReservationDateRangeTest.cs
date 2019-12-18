using System;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationDateRangeTest
        : TestBase
    {
        private readonly ReservationManager _target;

        public ReservationDateRangeTest()
        {
            _target = new ReservationManager();
        }

        [Fact]
        public void ScenarioOkay01TestAsync()
        {
            Reservation reservation = new Reservation
            {
                Von = new DateTime(2020, 1, 9),
                Bis = new DateTime(2020, 1, 10),
                AutoId = 2,
                KundeId = 1
            };

            Assert.True(_target.IsReservationValid(reservation).Result);
        }

        [Fact]
        public void ScenarioOkay02Test()
        {
            Reservation reservation = new Reservation
            {
                Von = new DateTime(2020, 1, 10),
                Bis = new DateTime(2020, 2, 10),
                AutoId = 2,
                KundeId = 1
            };

            Assert.True(_target.IsReservationValid(reservation).Result);

        }

        [Fact]
        public void ScenarioNotOkay01Test()
        {
            Reservation reservation = new Reservation
            {
                Von = new DateTime(2020, 2, 22),
                Bis = new DateTime(2020, 2, 11),
                AutoId = 2,
                KundeId = 1
            };

            Assert.False(_target.IsReservationValid(reservation).Result);

        }

        [Fact]
        public void ScenarioNotOkay02Test()
        {
            Reservation reservation = new Reservation
            {
                Von = new DateTime(2020, 2, 22, 12, 0, 0),
                Bis = new DateTime(2020, 2, 22, 13, 0, 0),
                AutoId = 2,
                KundeId = 1
            };

            Assert.False(_target.IsReservationValid(reservation).Result);
        }

        [Fact]
        public void ScenarioNotOkay03Test()
        {
            Reservation reservation = new Reservation
            {
                Von = new DateTime(2020, 2, 1, 12, 1, 0),
                Bis = new DateTime(2020, 2, 2, 12, 0, 0),
                AutoId = 2,
                KundeId = 1
            };

            Assert.False(_target.IsReservationValid(reservation).Result);
        }
    }
}
