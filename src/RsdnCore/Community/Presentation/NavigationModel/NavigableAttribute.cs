namespace Rsdn.Community.Presentation.NavigationModel
{
    using System;
    using System.Linq;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class NavigableAttribute : Attribute
    {
        public const string DefaultTarget = "default";

        public NavigableAttribute(string contentType)
            : this()
        {
            this.ContentType = contentType;
        }

        private NavigableAttribute()
        {
            this.Target = DefaultTarget;
        }

        public string ContentType { get; set; }

        public string Target { get; set; }
    }
}