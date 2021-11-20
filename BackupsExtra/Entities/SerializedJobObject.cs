using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Backups.Interfaces;

namespace BackupsExtra.Entities
{
    public class SerializedJobObject
    {
        [JsonConstructor]
        public SerializedJobObject(string files, string jobObjectType)
        {
            Files = files;
            JobObjectType = jobObjectType;
        }

        public string Files { get; }
        public string JobObjectType { get; }

        public IJobObject GetJobObject()
        {
            string typePath = EraseExtraSymbols(JobObjectType);
            string package = EraseExtraSymbols(JobObjectType.Split(".")[0]);
            var type = Type.GetType($"{typePath}, {package}", true);

            var jobObject = (IJobObject)Activator.CreateInstance(type);
            List<string> files = JsonSerializer.Deserialize<List<string>>(Files);

            foreach (string file in files)
            {
                jobObject.AddFile(new FileInfo(file));
            }

            return jobObject;
        }

        private string EraseExtraSymbols(string path)
        {
            char[] chars = path.Where(c => c != '\"').ToArray();
            return new string(chars);
        }
    }
}