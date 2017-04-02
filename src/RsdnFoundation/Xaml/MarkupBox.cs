namespace Rsdn.Xaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Markup;
    using Rendering;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    [TemplatePart(Name = TemplateParts.MarkupHost, Type = typeof(RichTextBlock))]
    public sealed class MarkupBox : Control
    {
        private static class TemplateParts
        {
            internal const string MarkupHost = "MarkupHost";
        }

        private readonly MarkupRenderer renderer;
        private RichTextBlock markupHost;

        public MarkupBox()
        {
            this.DefaultStyleKey = typeof(MarkupBox);
            this.renderer = new MarkupRenderer(this);
        }

        #region Markup (DependencyProperty)

        /// <summary>
        /// Identifies the <see cref="P:Markup"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkupProperty =
            DependencyProperty.Register("Markup", typeof(Tag), typeof(MarkupBox),
                new PropertyMetadata(null, new PropertyChangedCallback(HandleMarkupPropertyChanged)));

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="MarkupTextBox"/> is A description of the property..
        /// </summary>
        public Tag Markup
        {
            get { return (Tag)GetValue(MarkupProperty); }
            set { SetValue(MarkupProperty, value); }
        }

        /// <summary>
        /// Called when the <see cref="P:Markup"/> dependency property value is changed.
        /// </summary>
        /// <param name="d">The <see cref="System.Windows.DependencyObject"/> on which the property has changed value.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void HandleMarkupPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MarkupBox)d).OnMarkupChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="E:MarkupChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnMarkupChanged(DependencyPropertyChangedEventArgs e)
        {
            TryRenderMarkup();
        }

        #endregion Markup (DependencyProperty)

        #region Text (DependencyProperty)

        /// <summary>
        /// Identifies the <see cref="P:Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MarkupBox),
                new PropertyMetadata(null, new PropertyChangedCallback(HandleTextPropertyChanged)));

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="MarkupTextBox"/> is A description of the property..
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Called when the <see cref="P:Text"/> dependency property value is changed.
        /// </summary>
        /// <param name="d">The <see cref="System.Windows.DependencyObject"/> on which the property has changed value.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void HandleTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MarkupBox)d).OnTextChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="E:TextChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnTextChanged(DependencyPropertyChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(this.Text) == false)
            {
                this.Markup = RsdnMarkupReference.Current.Parse(this.Text);
            }
        }

        #endregion Text (DependencyProperty)

        #region TextWrapping (DependencyProperty)

        /// <summary>
        /// Identifies the <see cref="P:TextWrapping"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(MarkupBox),
                new PropertyMetadata(TextWrapping.Wrap, new PropertyChangedCallback(HandleTextWrappingPropertyChanged)));

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="MarkupTextBox"/> is A description of the property..
        /// </summary>
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        /// <summary>
        /// Called when the <see cref="P:TextWrapping"/> dependency property value is changed.
        /// </summary>
        /// <param name="d">The <see cref="System.Windows.DependencyObject"/> on which the property has changed value.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void HandleTextWrappingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MarkupBox)d).OnTextWrappingChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="E:TextWrappingChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnTextWrappingChanged(DependencyPropertyChangedEventArgs e)
        {
            TryRenderMarkup();
        }

        #endregion TextWrapping (DependencyProperty)

        #region TextAlignment (DependencyProperty)

        /// <summary>
        /// Identifies the <see cref="P:TextAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(MarkupBox),
                new PropertyMetadata(TextAlignment.Justify, new PropertyChangedCallback(HandleTextAlignmentPropertyChanged)));

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="MarkupTextBox"/> is A description of the property..
        /// </summary>
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        /// <summary>
        /// Called when the <see cref="P:TextAlignment"/> dependency property value is changed.
        /// </summary>
        /// <param name="d">The <see cref="System.Windows.DependencyObject"/> on which the property has changed value.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void HandleTextAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MarkupBox)d).OnTextAlignmentChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="E:TextAlignmentChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnTextAlignmentChanged(DependencyPropertyChangedEventArgs e)
        {
            TryRenderMarkup();
        }

        #endregion TextAlignment (DependencyProperty)

        #region TextBoxStyle (DependencyProperty)

        /// <summary>
        /// Identifies the <see cref="P:TextBoxStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextBoxStyleProperty =
            DependencyProperty.Register("TextBoxStyle", typeof(Style), typeof(MarkupBox),
                new PropertyMetadata(null, new PropertyChangedCallback(HandleTextBoxStylePropertyChanged)));

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="MarkupTextBox"/> is A description of the property..
        /// </summary>
        public Style TextBoxStyle
        {
            get { return (Style)GetValue(TextBoxStyleProperty); }
            set { SetValue(TextBoxStyleProperty, value); }
        }

        /// <summary>
        /// Called when the <see cref="P:TextBoxStyle"/> dependency property value is changed.
        /// </summary>
        /// <param name="d">The <see cref="System.Windows.DependencyObject"/> on which the property has changed value.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void HandleTextBoxStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MarkupBox)d).OnTextBoxStyleChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="E:TextBoxStyleChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnTextBoxStyleChanged(DependencyPropertyChangedEventArgs e)
        {
            TryRenderMarkup();
        }

        #endregion TextBoxStyle (DependencyProperty)

        #region HyperlinkButtonStyle (DependencyProperty)

        public static readonly DependencyProperty HyperlinkButtonStyleProperty =
                    DependencyProperty.Register("HyperlinkButtonStyle", typeof(Style), typeof(MarkupBox),
                      new PropertyMetadata(null));

        /// <summary>
        /// A description of the property.
        /// </summary>
        public Style HyperlinkButtonStyle
        {
            get { return (Style)GetValue(HyperlinkButtonStyleProperty); }
            set { SetValue(HyperlinkButtonStyleProperty, value); }
        }

        #endregion HyperlinkButtonStyle (DependencyProperty)

        #region Command (DependencyProperty)

        public static readonly DependencyProperty CommandProperty =
                    DependencyProperty.Register("Command", typeof(ICommand), typeof(MarkupBox),
                      new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        #endregion Command (DependencyProperty)

        internal RichTextBlock MarkupHost => this.markupHost;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.markupHost = GetTemplateChild(TemplateParts.MarkupHost) as RichTextBlock;
            TryRenderMarkup();
        }

        private void TryRenderMarkup()
        {
            if (this.markupHost != null)
            {
                this.Markup?.Render(this.renderer);
            }
        }
    }
}