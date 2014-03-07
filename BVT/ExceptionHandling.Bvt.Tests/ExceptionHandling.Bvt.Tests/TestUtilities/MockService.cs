// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using System;
using System.Collections;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.TestUtilities
{
    [ServiceContract]
    public interface IMockService
    {
        [OperationContract]
        [FaultContract(typeof(TestFault))]
        void Test();
    }

    [DataContract]
    public class TestFault
    {
        [DataMember]
        public Guid FaultID { get; set; }

        [DataMember]
        public string FaultMessage { get; set; }
    }

    public class MockServiceProxy : ClientBase<IMockService>, IMockService
    {
        public void Test()
        {
            this.Channel.Test();
        }
    }

    [ExceptionShielding("WCFTestingPolicy")]
    public class MockService1 : IMockService
    {
        public void Test()
        {
            Exception ex = new Exception();
            throw new ArgumentNullException("MockService1 exception", ex.InnerException);
        }   
    }

    [ExceptionShielding("PolicyNotExisting")]
    public class MockService2 : IMockService
    {
        public void Test()
        {
            Exception ex = new Exception();
            throw new ArithmeticException("MockService2 exception", ex.InnerException);
        }
    }

    [ExceptionShielding("WCFTestingPolicyWithNoExceptionMessage")]
    public class MockService3 : IMockService
    {
        public void Test()
        {
            Exception ex = new Exception();
            throw new ArithmeticException("MockService3 exception", ex.InnerException);
        }
    }

    [ExceptionShielding("WCFTestingPolicyWithNotThrowNew")]
    public class MockService4 : IMockService
    {
        public void Test()
        {
            Exception ex = new Exception();
            throw new ArithmeticException("MockService4 exception", ex.InnerException);
        }
    }

    [ExceptionShielding("WCFTestingPolicyWithNotifyRethrow")]
    public class MockService5 : IMockService
    {
        public void Test()
        {
            Exception ex = new Exception();
            throw new ArithmeticException("MockService5 exception", ex.InnerException);
        }
    }

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
