// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Defines an exception policy with a given name.
    /// </summary>
    public interface IExceptionConfigurationGivenPolicyWithName : IFluentInterface
    {
        /// <summary>
        /// Defines new policy with a given name.
        /// </summary>
        /// <param name="name">Name of policy</param>
        /// <returns></returns>
        IExceptionConfigurationForExceptionType GivenPolicyWithName(string name);
    }
}
