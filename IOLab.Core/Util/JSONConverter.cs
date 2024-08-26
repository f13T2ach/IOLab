using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Core.Util
{
    public class JSONConverter
    {
        public static List<string> JArrayToListString(JArray array)
        {
            List<string> list = new List<string>();
            foreach(JToken jToken in array)
            {
                list.Add(list.ToString());
            }
            return list;
        }
    }
}
