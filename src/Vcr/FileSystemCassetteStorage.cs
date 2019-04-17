using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            {
                var serializer = new Serializer();
                var tape = serializer.Deserialize<StorageWrapperV1>(stream);
                return tape.HttpInteractions
                    .Select(n => new HttpInteraction { Request = n })
                    .ToList();
            }
        }

        public void Save(string name, List<HttpInteraction> httpInteractions)
        {
            var safeName = GetSafeFileName(name);
            var file = new FileInfo(Path.Combine(_storageLocation.FullName, safeName));

            List<HttpRequest> requests = new List<HttpRequest>(httpInteractions.Count);
            foreach(var httpInteraction in httpInteractions)
            {
                requests.Add(httpInteraction.Request);
            }

            using (var stream = file.Open(FileMode.OpenOrCreate, FileAccess.Write))
            {
                var serializer = new Serializer(new SerializerSettings {
                     DefaultStyle = SharpYaml.YamlStyle.Block,
                     EmitAlias = false,
                     EmitTags = false,
                     SortKeyForMapping = false
                });
                serializer.Serialize(stream, new StorageWrapperV1 { HttpInteractions = requests });
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
            [SharpYaml.Serialization.YamlMember(1)]
            public int Version { get; set; } = 1;
            [SharpYaml.Serialization.YamlMember(2)]
            public List<HttpRequest> HttpInteractions { get; set; }
        }
    }
}
