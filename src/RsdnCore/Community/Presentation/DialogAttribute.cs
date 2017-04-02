namespace Rsdn.Community.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DialogAttribute : Attribute
    {
        public DialogAttribute(string dialogType)
        {
            this.DialogType = dialogType;
        }

        public string DialogType { get; }
    }
}