using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TracerLib;
using lab1;
using System.Threading;

namespace TracerTests
{
    [TestClass]
    public class UnitTest1
    {
        private Tracer tracer = new Tracer();
        private Foo foo;
        private int id;

        [TestInitialize]
        public void Initialize()
        {
            foo = new Foo(tracer);
            id = Thread.CurrentThread.ManagedThreadId;
        }

        [TestMethod]
        public void OneThread()
        {
            foo.MyMethod();
            var result = tracer.GetTraceResult();
            Assert.AreEqual(result.threads.Count, 1);
        }

        [TestMethod]
        public void ThreadID()
        {
            Thread secondThread = new Thread(foo.MyMethod);
            secondThread.Start();
            secondThread.Join();
            foo.MyMethod();

            var result = tracer.GetTraceResult();
            Assert.AreEqual(id, result.threads[id].id);
        }

        [TestMethod]
        public void TwoThread()
        {
            Thread secondThread = new Thread(foo.MyMethod);
            secondThread.Start();
            secondThread.Join();
            foo.MyMethod();

            var result = tracer.GetTraceResult();
            Assert.AreEqual(result.threads.Count, 2);
        }

        [TestMethod]
        public void TestMyMethod()
        {
            foo.MyMethod();

            var result = tracer.GetTraceResult();
            Assert.AreEqual(nameof(foo.MyMethod), result.threads[id].methods[0].methodName);
            Assert.AreEqual(nameof(Foo), result.threads[id].methods[0].className);
        }


    }
}
