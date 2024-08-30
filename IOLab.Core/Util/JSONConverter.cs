using Newtonsoft.Json.Linq;

namespace IOLab.Core.Util
{
    public class JSONConverter
    {
        public static List<string> JArrayToListString(JArray array)
        {
            List<string> list = new List<string>();
            foreach (JToken jToken in array)
            {
                list.Add(jToken.ToString());
            }
            return list;
        }

        public static List<int> JArrayToListInt(JArray array)
        {
            List<int> list = new List<int>();
            foreach (JToken jToken in array)
            {
                list.Add((int)jToken);
            }
            return list;
        }
    }
}
