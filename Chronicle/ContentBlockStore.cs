using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle
{
    public class ContentBlockStore
    {
        private Dictionary<string, ContentBlock> _namedBlocks = new Dictionary<string, ContentBlock>();

        public IList<ContentBlock> Blocks { get; } = new List<ContentBlock>();

    }
}
