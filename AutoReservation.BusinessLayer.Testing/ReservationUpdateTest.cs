using System;
using System.Threading.Tasks;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationUpdateTest
        : TestBase
    {
        private readonly ReservationManager _target;

        public ReservationUpdateTest()
        {
            _target = new ReservationManager();
        }

        [Fact]
        public async Task UpdateReservationTest()
        {

            var reservation = await _target.GetById(2);
            reservation.Von = new DateTime(2020, 1, 20);
            await _target.Update(reservation);

            var result = await _target.GetById(2);
            Assert.Equal(new DateTime(2020, 1, 20), result.Von);
        }
    }
}
