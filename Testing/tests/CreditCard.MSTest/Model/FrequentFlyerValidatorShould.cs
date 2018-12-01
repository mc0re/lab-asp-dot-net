using CreditCards.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CreditCard.MSTest
{
    [TestClass]
    public class FrequentFlyerValidatorShould
    {
        [DataTestMethod]
        [DataRow("012345-A")]
        [DataRow("012345-Q")]
        [DataRow("012345-Y")]
        public void AcceptValidSchemes(string number)
        {
            var sut = new FrequentFlyerValidator();
            Assert.IsTrue(sut.IsValid(number));
        }


        [DataTestMethod]
        [DataRow("012345-1")]
        [DataRow("012345-B")]
        [DataRow("012345-P")]
        [DataRow("012345-R")]
        [DataRow("012345-X")]
        [DataRow("012345-Z")]
        [DataRow("012345- ")]
        [DataRow("012345-a")]
        [DataRow("012345-q")]
        [DataRow("012345-y")]
        public void RejectInalidSchemes(string number)
        {
            var sut = new FrequentFlyerValidator();
            Assert.IsFalse(sut.IsValid(number));
        }


        [TestMethod]
        public void ThrowWhenNullNumber()
        {
            var sut = new FrequentFlyerValidator();
            var ex = Assert.ThrowsException<ArgumentNullException>(() => sut.IsValid(null));
            Assert.AreEqual("Value cannot be null.\r\nParameter name: number", ex.Message);
        }
    }
}
