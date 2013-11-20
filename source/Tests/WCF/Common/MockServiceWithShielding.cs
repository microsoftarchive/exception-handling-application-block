// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests
{
    [ExceptionShielding()]
    class MockServiceWithShielding : IMockService
    {
        #region IMockService Members

        public void Test()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
