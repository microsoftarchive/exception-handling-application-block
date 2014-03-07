// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.TestObjects
{
    [DataContract(Namespace = "http://FaultContracts/2006/03/MockFaultContract")]
    public class MockFaultContract
    {
        private string message;
        private IDictionary data;
        private Guid id;
        private double someNumber;

        public MockFaultContract()
        {
        }

        public MockFaultContract(string message)
        {
            this.message = message;
        }

        [DataMember]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        [DataMember]
        public IDictionary Data
        {
            get { return data; }
            set { data = value; }
        }

        [DataMember]
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public double SomeNumber
        {
            get { return someNumber; }
            set { someNumber = value; }
        }
    }
}
