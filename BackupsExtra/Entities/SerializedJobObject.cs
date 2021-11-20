using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Backups.Interfaces;
using Backups.Tools;

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
            List<string> files = JsonSerializer.Deserialize<List<string>>(Files);
            string typePath = JsonSerializer.Deserialize<string>(JobObjectType);
            string package = typePath?.Split(".")[0];
            var type = Type.GetType($"{typePath}, {package}");

            if (type is null)
            {
                throw new BackupsException("There is no such type");
            }

            if (files is null)
            {
                throw new BackupsException("Json error, there is no \"files\"");
            }

            var jobObject = (IJobObject)Activator.CreateInstance(type);

            foreach (string file in files)
            {
                jobObject!.AddFile(new FileInfo(file));
            }

            return jobObject;
        }
    }
}