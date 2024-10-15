using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingLib.Math;

namespace UnitTesting.DenobBlack
{
    public class LabWork6Tests
    {
        private readonly BasicCalc _calculator;

        public LabWork6Tests()
        {
            _calculator = new BasicCalc();
        }

        [Fact]
        public void Sqrt_ShouldReturnCorrectSqrt()
        {
            double result = _calculator.Sqrt(16);
            Assert.Equal(4, result);
        }

        [Theory]
        [InlineData(4, 2)]
        [InlineData(16, 4)]
        [InlineData(7, 2.6)]
        public void Sqrt_Theory(double number, double expectedResult)
        {
            double result = _calculator.Sqrt(number);
            Assert.Equal(expectedResult, result, 0.1);
        }

        [Fact]
        public void Sqrt_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _calculator.Sqrt(-1));
        }

        [Fact]
        public void IsPerfectNumber_ShouldReturnCorrectResult()
        {
            bool result = _calculator.IsPerfectNumber(6);
            Assert.True(result);
        }

        [Theory]
        [InlineData(6, true)]
        [InlineData(28, true)]
        [InlineData(7, false)]
        public void IsPerfectNumber_Theory(int number, bool expectedResult)
        {
            bool result = _calculator.IsPerfectNumber(number);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void IsPerfectNumber_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _calculator.IsPerfectNumber(-1));
        }
    }
}
