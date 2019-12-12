﻿using System;
using System.Threading.Tasks;
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
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task GetAllTest()
        {
            //throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
            var result = await _target.GetAll();
            Assert.Equal(4, result.Count);

        }
    }
}
