using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace WackEditor.Utilities
{
    public static class Serializer
    {

        /// <summary>
        /// Serializes a given DataContract into a file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The object instance to serialize, must be a DataContract</param>
        /// <param name="path">The full path of the file (including filename)</param>
        public static void ToFile<T>(T instance, string path)
        {
            try
            {
                using var fs = new FileStream(path, FileMode.Create);
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(fs, instance);
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                LoggerVM.Log(MessageTypes.Error, $"Failed to serialize {instance} to {path}");
                throw;
            }
        }

        /// <summary>
        /// Reads a serialized file and returns an instance
        /// </summary>
        /// <typeparam name="T">The type of the instance to be returned</typeparam>
        /// <param name="path">The full path of the file (including filename)</param>
        /// <returns></returns>
        public static T FromFile<T>(string path)
        {
            try
            {
                using var fs = new FileStream(path, FileMode.Open);
                var serializer = new DataContractSerializer(typeof(T));
                T instance = (T)serializer.ReadObject(fs);
                return instance;
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                LoggerVM.Log(MessageTypes.Error, $"Failed to deserialize {path}");
                throw;
            }
        }
    }
}
