// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners
{
    public class MockTraceListenerData : TraceListenerData
    {
        public MockTraceListenerData()
        {
        }

        public MockTraceListenerData(string name)
            : base(name, typeof(MockTraceListener), TraceOptions.None, SourceLevels.All)
        {
        }

        protected override TraceListener CoreBuildTraceListener(LoggingSettings settings)
        {
            return new MockTraceListener(this.Name);
        }
    }
}
