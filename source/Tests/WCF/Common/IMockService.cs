// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests
{
    [ServiceContract()]
    interface IMockService
    {
        [OperationContract()]
        void Test();
    }
}
