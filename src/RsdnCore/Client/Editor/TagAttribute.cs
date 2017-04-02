namespace Rsdn.Client.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Field)]
    public class TagAttribute : Attribute
    {
        public TagAttribute(string tagName)
        {
            this.TagName = tagName;
        }

        public string TagName { get; }
    }
}