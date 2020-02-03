using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DaysOff.Utils
{
    public class JsonUtils
    {

        public static List<string> ConvertJsonStr(string strInput)
        {
            string str = strInput;
            List<string> strObjs = new List<string>();
            strObjs.Add(str);
            return strObjs;
        }





        public static T getJsonObjectFromFile<T>(string fileName, T obj) where T : class
        {

            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                JsonConvert.PopulateObject(json, obj);
            };

            return obj;
        }

        public static T getJsonObjectFromFile<T>(string fileName) where T : class
        {


            T obj;
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                obj = JsonConvert.DeserializeObject<T>(json);
            }

            return obj;
        }

        public static void writeJsonObjectToFile(string fileName, object obj)
        {

            using (StreamWriter file = File.CreateText(fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, obj);
            }


        }
    }
}
