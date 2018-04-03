using Microsoft.VisualStudio.TestTools.UnitTesting;
using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Tests
{
    [TestClass()]
    public class MessageTests
    {
        [TestMethod()]
        public void BuildMessageFromTcpStringTest_ShouldSplitOnDelimiterCorrectly()
        {
            //String to test
            string tcpString = "this \\0 is \\0 a \\0 test\002-Apr-18 12:54:43 PM\0testUser<EOF>";
            DateTime tcpTimeStamp = DateTime.Parse("02-Apr-18 12:54:43 PM");

            Message message = Message.BuildMessageFromTcpString(tcpString);

            string content = message.Content;
            DateTime timeStamp = message.TimeStamp;
            string user = "testUser";

            Assert.AreEqual("this \\0 is \\0 a \\0 test", content);
            Assert.AreEqual(tcpTimeStamp, timeStamp);
            Assert.AreEqual("testUser<EOF>", user);
        }
    }
}