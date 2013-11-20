// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    public class MockReturnNullExceptionHandler : IExceptionHandler
    {
        public MockReturnNullExceptionHandler()
        {
        }

        public Exception HandleException(Exception exception, Guid handlingInstanceID)
        {
            return null;
        }
    }
}

