using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace TracerLib
{
    class Serializers
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
    }
}
