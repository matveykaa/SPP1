﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TracerLib
{
    public interface ITracer
    {
        void StartTrace();
        void StopTrace();
        TraceResult GetTraceResult();
    }


    class MethodTrace
    {
        private Stopwatch stopwatch;
        private MethodRes method = new MethodRes();

        public MethodTrace()
        {
            this.stopwatch = new Stopwatch();

            StackTrace trace = new StackTrace();
            var oneFrame = trace.GetFrame(3);
            method.set_methodName(oneFrame.GetMethod().Name);
            method.set_className(oneFrame.GetMethod().ReflectedType.Name);
        }

        public void StartTrace()
        {
            stopwatch.Start();
        }

        public void StopTrace()
        {
            stopwatch.Stop();
            var time = stopwatch.ElapsedMilliseconds;
            method.set_time(time);
        }

        public void addChildResult(MethodRes methodRes)
        {
            this.method.addChild(methodRes);
        }
        public MethodRes GetTraceRes()
        {
            return method;
        }
    }

    class ThreadTrace
    {
        private ThreadRes thread;
        private Stack<MethodTrace> stackList = new Stack<MethodTrace>();



        public ThreadTrace(int ID)
        {
            this.thread = new ThreadRes();
            this.thread.id = ID;
        }

        public void StartTrace()
        {
            var new_MethodTrace = new MethodTrace();
            stackList.Push(new_MethodTrace);
            new_MethodTrace.StartTrace();
        }

        public void StopTrace()
        {
            var lastTrace = stackList.Pop();
            lastTrace.StopTrace();
            var method = lastTrace.GetTraceRes();



            if (stackList.Count > 0)
            {
                //continue next method
                //create children method's

                var newLastTracer = stackList.Peek();
                newLastTracer.addChildResult(method);
            }
            else
            {
                //end of method's list
                thread.addMethod(method);
                thread.addTime(method.time);
            }

        }

        public ThreadRes GetTraceRes()
        {
            return thread;
        }
    }

    public class Tracer : ITracer
    {
        private Dictionary<int, ThreadTrace> thread_objects = new Dictionary<int, ThreadTrace>();


        // вызывается в начале замеряемого метода
        public void StartTrace()
        {
            int threadID = Thread.CurrentThread.ManagedThreadId;

            ThreadTrace thread;
            if (thread_objects.ContainsKey(threadID))
            {
                thread = thread_objects[threadID];
            }
            else
            {
                //create new threadTrace
                thread = new ThreadTrace(threadID);
                thread_objects[threadID] = thread;
            }

            thread.StartTrace();

        }

        // вызывается в конце замеряемого метода 
        public void StopTrace()
        {
            int threadID = Thread.CurrentThread.ManagedThreadId;

            if (thread_objects.ContainsKey(threadID))
            {
                thread_objects[threadID].StopTrace();
            }

        }

        // получить результаты измерений  
        public TraceResult GetTraceResult()
        {
            var values = thread_objects.Values;

            var results = new Dictionary<int, ThreadRes>();
            foreach (ThreadTrace value in values)
            {
                var threadResult = value.GetTraceRes();
                results.Add(threadResult.id, threadResult);
            }

            var result = new TraceResult(results);

            return result;
        }
    }

}
