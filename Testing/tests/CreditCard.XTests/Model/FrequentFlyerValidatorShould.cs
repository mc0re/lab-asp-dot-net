using CreditCards.Core.Model;
using System;
using Xunit;

namespace CreditCard.XTests
{
    public class FrequentFlyerValidatorShould
    {
        [Theory]
        [InlineData("012345-A")]
        [InlineData("012345-Q")]
        [InlineData("012345-Y")]
        public void AcceptValidSchemes(string number)
        {
            var sut = new FrequentFlyerValidator();
            Assert.True(sut.IsValid(number));
        }


        [Theory]
        [InlineData("012345-1")]
        [InlineData("012345-B")]
        [InlineData("012345-P")]
        [InlineData("012345-R")]
        [InlineData("012345-X")]
        [InlineData("012345-Z")]
        [InlineData("012345- ")]
        [InlineData("012345-a")]
        [InlineData("012345-q")]
        [InlineData("012345-y")]
        public void RejectInalidSchemes(string number)
        {
            var sut = new FrequentFlyerValidator();
            Assert.False(sut.IsValid(number));
        }


        [Theory]
        [InlineData("0012345-A")]
        [InlineData("X12345-A")]
        [InlineData("01234X-A")]
        public void RejectInalidMemberNumbers(string number)
        {
            var sut = new FrequentFlyerValidator();
            Assert.False(sut.IsValid(number));
        }


        [Theory]
        [InlineData("      -A")]
        [InlineData("  1   -A")]
        [InlineData("1     -A")]
        [InlineData("     1-A")]
        public void RejectEmptyMemberNumbers(string number)
        {
            var sut = new FrequentFlyerValidator();
            Assert.False(sut.IsValid(number));
        }


        [Theory]
        [InlineData("        ")]
        [InlineData("")]
        public void RejectEmptyNumbers(string number)
        {
            var sut = new FrequentFlyerValidator();
            Assert.False(sut.IsValid(number));
        }


        [Fact]
        public void ThrowWhenNullNumber()
        {
            var sut = new FrequentFlyerValidator();
            var ex = Assert.Throws<ArgumentNullException>(() => sut.IsValid(null));
            Assert.Equal("Value cannot be null.\r\nParameter name: number", ex.Message);
        }
    }
}
