// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// This interface supports the configuration of the Exception Handling Application Block.
    /// </summary>
    public interface IExceptionConfigurationForExceptionTypeOrPostHandling :
        IExceptionConfigurationGivenPolicyWithName,
        IExceptionConfigurationAddExceptionHandlers,
        IExceptionConfigurationThenDoPostHandlingAction
    {
    }
}
