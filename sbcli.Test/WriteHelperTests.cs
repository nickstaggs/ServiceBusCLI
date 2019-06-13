using NUnit.Framework;
using ServiceBusCLI.CommandLineOptions;
using ServiceBusCLI.Helpers.VerbHelpers;
using System;

namespace sbcli.Test
{
    [TestFixture]
    public class WriteHelperTests
    {

        [Test]
        public void CreateSBMessageShouldCreateProperMessageWithUserProps()
        {
            var message = WriteHelper.CreateSBMessage("Hello", "k1:v1");

            Assert.AreEqual("v1", message.UserProperties["k1"]);
        }

        [Test]
        public void CreateSBMessageShouldThrowArgumentExceptionWhenProvidedMalformedUserProperties()
        {
            Assert.Throws<ArgumentException>(() => WriteHelper.CreateSBMessage("hello", "k1v1"));
        }
    }
}
