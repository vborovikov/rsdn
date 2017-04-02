namespace Rsdn.Community.Interaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GroupDetails : IIdentifiable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SortOrder { get; set; }

        public IEnumerable<ForumDetails> Forums { get; set; }
    }
}