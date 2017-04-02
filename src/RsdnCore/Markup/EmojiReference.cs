namespace Rsdn.Markup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Rendering;

    public sealed class EmojiReference : SpecialContentReference
    {
        public static readonly Dictionary<string, string> KnownCodes = new Dictionary<string, string>
        {
            { ":)", "☺️" },
            { ":))", "😃" },
            { ":D", "😃" },
            { ":-D", "😃" },
            { ":)))", "😆" },
            { ":(", "☹" },
            { ";)", "😉" },
            { ":-\\", "😏" },
            { ":???:", "😕" },
            { ":NO:", "😧" },
            { ":UP:", "👍" },
            { ":DOWN:", "👎" },
            { ":SUPER:", "😎" },
            { ":SHUFFLE:", "😳" },
            { ":WOW:", "😲" },
            { ":CRASH:", "💀" },
            { ":USER:", "💻" },
            { ":MANIAC:", "😈" },
            { ":XZ:", "😐" },
            { ":BEER:", "🍻" },
            { ":FACEPALM:", "😒" },
            { ":SARCASM:", "💁" },
        };

        public EmojiReference()
            : base(ContentType.Emoji)
        {
        }

        internal bool TryCreate(string code, out SpecialContent emoji)
        {
            code = code.ToUpperInvariant();
            if (KnownCodes.ContainsKey(code))
            {
                emoji = new SpecialContent(this, code);
                return true;
            }

            emoji = null;
            return false;
        }

        protected override void RenderLine(string line, IRenderer renderer)
        {
            base.RenderLine(KnownCodes[line], renderer);
        }
    }
}