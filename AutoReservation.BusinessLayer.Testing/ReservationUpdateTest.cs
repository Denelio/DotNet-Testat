using System;
using System.Threading.Tasks;
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
            //throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert

            var result = await _target.GetById(2);
            DateTime thisDate1 = new DateTime(2020, 1, 10);
            var resultDate = (result.Bis - result.Von).TotalHours;
            Assert.Equal(1, resultDate);
        }
    }
}
