using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace TracerLib
{
    
    
        public class Record
        {
            [XmlIgnore]
            [JsonIgnore]
            public int ID;

            [XmlElement(ElementName = "Thread")]
            public ThreadRes res;

            public Record(int key, ThreadRes res)
            {
                ID = key;
                this.res = res;
            }
            public Record()
            {
            }
        }
    public class Serializers
    {

        public string toXML(TraceResult result)
        {
            List<Record> records = new List<Record>();
            foreach (int key in result.threads.Keys)
            {
                records.Add(new Record(key, result.threads[key]));
            }

            XmlSerializer formatter = new XmlSerializer(typeof(List<Record>));
          
            formatter.Serialize(stringWriter, records);

            return stringWriter.ToString();
        }

       

    }
}
