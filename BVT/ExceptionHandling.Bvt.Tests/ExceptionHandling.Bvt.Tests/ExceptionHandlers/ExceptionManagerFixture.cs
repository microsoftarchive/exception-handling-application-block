// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.ExceptionHandlers
{
    [TestClass]
    public class ExceptionManagerFixture
    {
        [TestMethod]
        public void CreatesWithExceptionPolicies()
        {
            var policies = new Dictionary<string, ExceptionPolicyDefinition>();
            ExceptionManager exceptionManager = new ExceptionManager(policies);

            Assert.IsNotNull(exceptionManager);
        }

        [TestMethod]
        public void ThrowsArgumentNullExceptionWhenHadlingExceptionWithNullPolicy()
        {
            var policies = new Dictionary<string, ExceptionPolicyDefinition>();
            ExceptionManager exceptionManager = new ExceptionManager(policies);

            ExceptionAssertHelper.Throws<ArgumentNullException>(() => exceptionManager.HandleException(new Exception(), null));
        }

        [TestMethod]
        public void ThrowsExceptionHandlingExceptionWhenHadlingExceptionWithPolicyThatDoesNotExist()
        {
            var policies = new Dictionary<string, ExceptionPolicyDefinition>();
            var exceptionManager = new ExceptionManager(policies);

            ExceptionAssertHelper.Throws<ExceptionHandlingException>(() => exceptionManager.HandleException(new Exception(), "NonMatchingPolicy"));
        }

        [TestMethod]
        public void ThrowsArgumentNullExceptionWhenProcessingNullAction()
        {
            var policies = new Dictionary<string, ExceptionPolicyDefinition>();
            var exceptionManager = new ExceptionManager(policies);

            ExceptionAssertHelper.Throws<ArgumentNullException>(
                () => exceptionManager.Process(null, "Something"));
        }

        [TestMethod]
        public void ThrowsArgumentNullExceptionWhenProcessingNullPolicy()
        {
            var policies = new Dictionary<string, ExceptionPolicyDefinition>();
            var exceptionManager = new ExceptionManager(policies);

            ExceptionAssertHelper.Throws<ArgumentNullException>(
                () => exceptionManager.Process(() => { }, null));
        }

        [TestMethod]
        public void ThrowsExceptionHandlingExceptionWhenUsingProcessWithNonMatchingPolicy()
        {
            var policies = new Dictionary<string, ExceptionPolicyDefinition>();
            var exceptionManager = new ExceptionManager(policies);

            ExceptionAssertHelper.Throws<ExceptionHandlingException>(() => exceptionManager.Process(() => { throw new DBConcurrencyException(); }, "NonMatchingPolicy"));
        }
    }
}
