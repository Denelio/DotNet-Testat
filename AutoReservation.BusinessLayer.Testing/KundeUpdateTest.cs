using System;
using System.Threading.Tasks;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class KundeUpdateTest
        : TestBase
    {
        private readonly KundeManager _target;

        public KundeUpdateTest()
        {
            _target = new KundeManager();
        }
        
        [Fact]
        public async Task UpdateKundeTest()
        {
            var kunde = await _target.GetById(1);
            kunde.Vorname = "Wilma";
            await _target.Update(kunde);

            var result = await _target.GetById(1);
            Assert.Equal("Wilma", result.Vorname);

        }
    }
}
