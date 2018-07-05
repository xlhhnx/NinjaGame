using System.Collections.Generic;

namespace LoadFileManager
{
    public class DefinitionList<T> : IDefinitionList
    {
        public string FileName { get; set; }
        public List<T> Definitions { get; set; }

        public DefinitionList()
        {
            FileName = "";
            Definitions = new List<T>();
        }
    }
}
