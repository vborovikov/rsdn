namespace Rsdn.Community.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Client.Editor;
    using Markup;
    using Relay.PresentationModel;

    public class NewPostPresenter : Presenter
    {
        public NewPostPresenter()
        {
        }

        public string Title { get; set; }

        public string Message { get; set; }

        public string SelectedText { get; set; }

        public ICommand EditCommand => GetCommand<object>(Edit);

        public void Clear()
        {
            this.Title = null;
            this.Message = null;
        }

        private Task Edit(object obj)
        {
            if (obj is string)
            {
                this.SelectedText = (string)obj;
            }
            else if (obj is CodeLanguage)
            {
                var lang = (CodeLanguage)obj;
                InsertLangTag(lang);
            }
            else if (obj is FormatAction)
            {
                var action = (FormatAction)obj;
                InsertFormatTag(action);
            }
            return Task.CompletedTask;
        }

        private void InsertFormatTag(FormatAction action)
        {
        }

        private void InsertLangTag(CodeLanguage lang)
        {
        }
    }
}