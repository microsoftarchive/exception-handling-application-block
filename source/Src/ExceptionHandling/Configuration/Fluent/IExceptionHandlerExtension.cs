// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Used to provide context to extensions of the Exception Handling fluent configuration interface.
    /// </summary>
    public interface IExceptionHandlerExtension : IFluentInterface
    {
        /// <summary>
        /// Retrieves data about the currently built up ExceptionTypeData.  Exception handler configuration extensions will use this to 
        /// add their handler information to the exception.
        /// </summary>
        /// <seealso cref="ReplaceHandler"/>
        /// <seealso cref="ReplaceWithHandlerLoggingConfigurationSourceBuilderExtensions"/>
        /// 
        /// <seealso cref="WrapHandler"/>
        /// <seealso cref="WrapWithHandlerLoggingConfigurationSourceBuilderExtensions"/>
        ExceptionTypeData CurrentExceptionTypeData { get; }
    }
}
