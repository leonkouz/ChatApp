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
        public void BuildMessageFromTcpStringTest_ShouldSplitTcpStringCorrectly()
        {
            string tcpString = "asdas|/][-02-Apr-18 12:21:19 PM|/][-sadas<EOF>";

            Message messaghe = Message.BuildMessageFromTcpString(tcpString);

        }
    }
}