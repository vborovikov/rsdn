namespace Rsdn.Xaml.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Markup;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Data;

    public sealed partial class TextEditor : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string),
                typeof(TextEditor), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedTextProperty =
            DependencyProperty.Register(nameof(SelectedText), typeof(string),
                typeof(TextEditor), new PropertyMetadata(String.Empty));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand),
                typeof(TextEditor), new PropertyMetadata(null));

        public TextEditor()
        {
            this.InitializeComponent();
            if (this.Content is FrameworkElement)
                ((FrameworkElement)this.Content).DataContext = this;

            InitializeEmojiMenu();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string SelectedText
        {
            get { return (string)GetValue(SelectedTextProperty); }
            set { SetValue(SelectedTextProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        private void InitializeEmojiMenu()
        {
            var commandBinding = new Binding
            {
                Path = new PropertyPath(nameof(this.Command)),
                RelativeSource = new RelativeSource { Mode = RelativeSourceMode.TemplatedParent }
            };

            this.EmojiMenu.Items.Clear();
            foreach (var emoji in EmojiReference.KnownCodes)
            {
                var code = emoji.Key.EndsWith("D", StringComparison.Ordinal) ?
                    emoji.Key :
                    emoji.Key.ToLowerInvariant();

                var menuFlyoutItem = new MenuFlyoutItem
                {
                    Text = emoji.Value + " " + code,
                    CommandParameter = code,
                };
                menuFlyoutItem.SetBinding(MenuFlyoutItem.CommandProperty, commandBinding);

                this.EmojiMenu.Items.Add(menuFlyoutItem);
            }
        }
    }
}