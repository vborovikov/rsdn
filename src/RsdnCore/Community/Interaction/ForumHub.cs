namespace Rsdn.Community.Interaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ForumHub
    {
        public ForumHub(string name, IEnumerable<ForumDetails> forums)
        {
            this.Name = name;
            this.Forums = forums;
        }

        public string Name { get; }

        public object Tag { get; set; }

        public IEnumerable<ForumDetails> Forums { get; }
    }
}