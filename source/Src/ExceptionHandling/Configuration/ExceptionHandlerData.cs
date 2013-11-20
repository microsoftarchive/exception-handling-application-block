// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the configuration for an <see cref="IExceptionHandler"/>.
    /// </summary>    
    [ViewModel(ExceptionHandlingDesignTime.ViewModelTypeNames.ExceptionHandlerDataViewModel)]
    public class ExceptionHandlerData : NameTypeConfigurationElement
    {
        /// <summary>
        /// Initializes an instance of a <see cref="ExceptionHandlerData"/> class.
        /// </summary>
        public ExceptionHandlerData()
        {
        }

        /// <summary>
        /// Initializes an instance of an <see cref="ExceptionHandlerData"/> class with a name and an <see cref="IExceptionHandler"/> type.
        /// </summary>
        /// <param name="type">
        /// The configured <see cref="IExceptionHandler"/> type.
        /// </param>
        public ExceptionHandlerData(Type type)
            : this(null, type)
        {
        }


        /// <summary>
        /// Initializes an instance of an <see cref="ExceptionHandlerData"/> class with a name and an <see cref="IExceptionHandler"/> type.
        /// </summary>
        /// <param name="name">
        /// The name of the configured <see cref="IExceptionHandler"/>.
        /// </param>
        /// <param name="type">
        /// The configured <see cref="IExceptionHandler"/> type.
        /// </param>
        public ExceptionHandlerData(string name, Type type)
            : base(name, type)
        {
        }

        /// <summary>
        /// Builds the exception handler represented by this configuration object.
        /// </summary>
        /// <returns>An <see cref="IExceptionHandler"/>.</returns>
        public virtual IExceptionHandler BuildExceptionHandler()
        {
            // Cannot make abstract for serialization reasons.
            throw new NotImplementedException(Resources.MustBeImplementedBySubclassException);
        }
    }
}
