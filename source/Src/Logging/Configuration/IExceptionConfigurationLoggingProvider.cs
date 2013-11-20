// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Defines the fluent configuration extensions for the logging provider.
    /// </summary>
    public interface IExceptionConfigurationLoggingProvider : IExceptionConfigurationForExceptionTypeOrPostHandling
    {
        /// <summary>
        /// Title to use when logging an exception.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        IExceptionConfigurationLoggingProvider UsingTitle(string title);

        /// <summary>
        /// EventId to use when logging an exception.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        IExceptionConfigurationLoggingProvider UsingEventId(int eventId);

        /// <summary>
        /// Type of exception formatter to use when logging.
        /// </summary>
        /// <param name="exceptionFormatterType"></param>
        /// <returns></returns>
        IExceptionConfigurationLoggingProvider UsingExceptionFormatter(Type exceptionFormatterType);

        /// <summary>
        /// Type of exception formatter to use when logging.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IExceptionConfigurationLoggingProvider UsingExceptionFormatter<T>();

        /// <summary>
        /// Severity to use when logging an exception.
        /// </summary>
        /// <param name="severity"></param>
        /// <returns></returns>
        IExceptionConfigurationLoggingProvider WithSeverity(TraceEventType severity);

        /// <summary>
        /// Priority to use when logging an exception.
        /// </summary>
        /// <param name="priority"></param>
        /// <returns></returns>
        IExceptionConfigurationLoggingProvider WithPriority(int priority);
    }
}
