using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Vcr
{
    public class FileSystemCassetteStorage : ICasseteStorage
    {
        private readonly DirectoryInfo _storageLocation;
        private readonly char[] _invalidFilenameChars = Path.GetInvalidFileNameChars();

        public FileSystemCassetteStorage(DirectoryInfo storageLocation)
        {
            _storageLocation = storageLocation ?? throw new ArgumentNullException(nameof(storageLocation));
            _storageLocation.Create();
        }

        public List<HttpInteraction> Load(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var safeName = GetSafeFileName(name);
            foreach (var file in _storageLocation.EnumerateFiles())
            {
                if (file.Name == safeName)
                    return LoadFile(file);
            }
            return null;
        }

        private List<HttpInteraction> LoadFile(FileInfo file)
        {
            using (var stream = file.OpenRead())
            using (var reader = new StreamReader(stream))
            {
                var serializer = new DeserializerBuilder()
                    .WithNamingConvention(new CamelCaseNamingConvention())
                    .Build();

                var tape = serializer.Deserialize<StorageWrapperV1>(reader);
                return tape.HttpInteractions
                    .Select(n => new HttpInteraction { Request = n.Request, Response = n.Response })
                    .ToList();
            }
        }

        public void Save(string name, IEnumerable<HttpInteraction> httpInteractions)
        {
            var safeName = GetSafeFileName(name);
            var file = new FileInfo(Path.Combine(_storageLocation.FullName, safeName));

            using (var stream = file.Open(FileMode.OpenOrCreate, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
            {
                var serializer = new SerializerBuilder()
                    .DisableAliases()
                    .WithNamingConvention(new CamelCaseNamingConvention())
                    .Build();

                serializer.Serialize(writer, new StorageWrapperV1 { HttpInteractions = httpInteractions.ToList() });
            }
        }

        private string GetSafeFileName(string name)
        {
            foreach(var invalidChar in _invalidFilenameChars)
            {
                name = name.Replace(invalidChar, '_');
            }
            return name + ".yaml";
        }

        private class StorageWrapperV1
        {
            [YamlMember(Order = 1)]
            public string Version { get; set; } = "VCR.net 1.0.0";
            [YamlMember(Order = 2)]
            public List<HttpInteraction> HttpInteractions { get; set; }
        }
    }
}
