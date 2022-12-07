using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Text.Json.Serialization;


namespace TracerLib
{
  
    public class MethodRes
    {
     
        public string methodName;
       
        public string className;
      
        public long time;
        public readonly List<MethodRes> childMethods = new List<MethodRes>();

        public void set_time(long time)
        {
            this.time = time;
        }
        public void set_methodName(string methodName)
        {
            this.methodName = methodName;
        }
        public void set_className(string className)
        {
            this.className = className;
        }
        public void addChild(MethodRes childMethod)
        {
            this.childMethods.Add(childMethod);
        }


    }

   
    public class ThreadRes
    {
        public long time;

        public List<MethodRes> methods = new List<MethodRes>();


        public void addMethod(MethodRes method)
        {
            methods.Add(method);
        }

        public void addTime(long time)
        {
            this.time += time;
        }
    }

  
    public class TraceResult
    {
        //[Newtonsoft.Json.JsonProperty("id")]
        public Dictionary<int, ThreadRes> threads { get; private set; }

        public TraceResult()        {
            threads = new Dictionary<int, ThreadRes>();
        }

        public TraceResult(Dictionary<int, ThreadRes> threads)
        {
            this.threads = threads;
        }
        public void addThread(int ID_Thread, ThreadRes thread)
        {
            threads.Add(ID_Thread, thread);
        }
        public ThreadRes getThreadInfo(int ID_Thread)
        {
            return threads[ID_Thread];
        }
        public Dictionary<int, ThreadRes> getResult()
        {
            return threads;
        }
    }
}
