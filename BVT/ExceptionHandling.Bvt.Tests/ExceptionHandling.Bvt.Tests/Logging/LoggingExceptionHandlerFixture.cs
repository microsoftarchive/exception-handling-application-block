// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.TestUtilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.Logging
{
    [TestClass]
    public class LoggingExceptionHandlerFixture
    {
        private IConfigurationSource configurationSource;

        [TestInitialize]
        public void Initialize()
        {
            this.configurationSource = new FileConfigurationSource(@"configurations\\LoggingExceptionHandler.config");

            LogWriterFactory factory = new LogWriterFactory((e) => { return this.configurationSource.GetSection(e); });
            LogWriter writer = factory.Create();
            Logger.SetLogWriter(writer);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Logger.Reset();

            if (this.configurationSource != null)
            {
                this.configurationSource.Dispose();
            }
        }

        [TestMethod]
        public void CreatesXmlExceptionFormatter()
        {
            TextWriter writer = new System.IO.StringWriter();
            Exception exception = new Exception("test exception");
            Guid handlingInstanceID = Guid.NewGuid();

            XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, exception, handlingInstanceID);
            formatter.Format();

            XmlDocument document = new XmlDocument();
            ExceptionAssertHelper.DoesNotThrow(() => document.LoadXml(writer.ToString()));
        }

        [TestMethod]
        public void LogsExceptionWhenExceptionShouldBeHandled()
        {
            var exceptionManager = ((ExceptionHandlingSettings)this.configurationSource.GetSection("exceptionHandling")).BuildExceptionManager();
            exceptionManager.HandleException(new DBConcurrencyException(), "Logging DBConcurrencyException, None postHandling");

            var entry = EventLogEntries.Last;
            var document = XDocument.Parse(entry.Message);
            Assert.AreEqual("Exception", document.Root.Name);
            var exceptionType = Type.GetType(document.Root.Element("ExceptionType").Value);
            Assert.AreEqual(typeof(DBConcurrencyException), exceptionType);
        }
    }
}
