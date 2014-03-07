// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.TestObjects
{
    internal class TestExceptionHandler : IExceptionHandler
    {
        static TestExceptionHandler()
        {
            HandlingNames = new List<string>();
        }

        public TestExceptionHandler(string name)
        {
            Name = name;
        }

        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            HandlingNames.Add(Name);

            var newException = new ApplicationException("foo", exception);

            return newException;
        }

        public string Name { get; private set; }

        public static IList<string> HandlingNames { get; private set; }
    }
}
