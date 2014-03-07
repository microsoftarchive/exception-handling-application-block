// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.TestObjects
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        public CustomExceptionHandler(NameValueCollection unused)
        { }

        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            return new NotImplementedException();
        }
    }
}
