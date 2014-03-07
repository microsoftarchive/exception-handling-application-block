// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.TestUtilities
{
    public static class EventLogEntries
    {
        public static EventLogEntry Last
        {
            get
            {
                string logName = EventLog.LogNameFromSourceName("Application", ".");

                using (EventLog eventlog = new EventLog(logName))
                {
                    return eventlog.Entries.Count == 0 ? null : eventlog.Entries[eventlog.Entries.Count - 1];
                }
            }
        }
    }
}
