using System.Collections.Generic;

namespace Vcr
{
    public interface ICasseteStorage
    {
        List<HttpInteraction> Load(string name);
        void Save(string name, List<HttpInteraction> httpInteractions);
    }
}
