using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle
{
    public class ContentBlock
    {
        public IPermittable ViewPermission { get; set; } = Permittable.All;
        public Content Content { get; set; }

        public string Serialized()
        {
            return $"[[[viewers({ViewPermission.Serialized()})\r\n"
                + Content.ToString()
                + "\r\n]]]";
        }

        public override string ToString()
        {
            return $"[[[viewers({ViewPermission})\r\n"
                + Content.ToString()
                + "\r\n]]]";
        }
    }
}
