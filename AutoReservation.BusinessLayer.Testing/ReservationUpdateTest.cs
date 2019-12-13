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
            reservation.Von = new DateTime(1981, 05, 05);
            await _target.Update(reservation);

            var result = await _target.GetById(2);
            var time = new DateTime(1981, 05, 05);
            Assert.Equal(time, result.Von);

        }
    }
}
