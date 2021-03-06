﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Vcr.Test
{
    public class MemoryStorage : ICasseteStorage
    {
        private ConcurrentDictionary<string, List<HttpInteraction>> _storage = new ConcurrentDictionary<string, List<HttpInteraction>>();

        public List<HttpInteraction> Load(string name)
        {
            if (_storage.TryGetValue(name, out var value))
                return value;

            return null;
        }

        public void Save(string name, IEnumerable<HttpInteraction> httpInteractions)
        {
            _storage[name] = httpInteractions.ToList();
        }
    }
}
