﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChatApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Core;

namespace ChatApp.Tests
{
    [TestClass()]
    public class MessageTests
    {
        [TestMethod()]
        public void BuildMessageFromTcpStringTest_ShouldSplitOnDelimiterCorrectly()
        {
            //String to test
            string tcpString = "this \\0 is \\0\\0a\\0 test\03/04/2018 2:14:31 PM\0testUser<EOF>";
            DateTime tcpTimeStamp = DateTime.Parse("03/04/2018 2:14:31 PM");

            Message message = Message.BuildMessageFromTcpString(tcpString);

            string content = message.Content;
            DateTime timeStamp = message.TimeStamp;
            string user = "testUser<EOF>";

            Assert.AreEqual("this \\0 is \\0\\0a\\0 test", content);
            Assert.AreEqual(tcpTimeStamp, timeStamp);
            Assert.AreEqual("testUser<EOF>", user);
        }
    }
}