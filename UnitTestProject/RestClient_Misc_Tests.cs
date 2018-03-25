using LittleRestClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace UnitTestProject
{
    [TestClass]
    public class RestClient_Misc_Tests
    {
        [TestMethod]
        public void Empty_Url_Test()
        {
            Assert.ThrowsException<ArgumentException>(() => new RestClient(""));
        }

        [TestMethod]
        public void Empty_Config_Test()
        {
            var config = new Mock<IRestClientConfig>();
            Assert.ThrowsException<ArgumentException>(() => new RestClient(config.Object));
        }
    }
}
