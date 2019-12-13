using System;
using System.Threading.Tasks;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class AutoUpdateTests
        : TestBase
    {
        private readonly AutoManager _target;

        public AutoUpdateTests()
        {
            _target = new AutoManager();
        }

        [Fact]
        public async Task UpdateAutoTest()
        {
            var auto = await _target.GetById(4);
            auto.Marke = "Ferrari";
            await _target.Update(auto);
            Assert.Equal("Ferrari" , auto.Marke);
        }

        [Fact]
        public async Task GetAllTest()
        {
            var result = await _target.GetAll();
            Assert.Equal(4, result.Count);

        }
    }
}
