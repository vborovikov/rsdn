namespace Rsdn.Xaml.Rendering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Markup;
    using Markup.Rendering;
    using Microsoft.Toolkit.Uwp.UI.Controls;
    using Text;
    using Windows.Storage.Streams;
    using Windows.UI.Text;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Documents;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;

    internal sealed class MarkupRenderer : RendererBase<Block, Inline>
    {
        private static class VisualParameters
        {
            public const double TextIndent = 12d;
            public static readonly Brush ThemeBrandBrush;
            public static readonly Brush PhoneSubtleBrush;
            public static readonly Brush PhoneSubtleBrush2;
            public static readonly Brush PhoneSubtleBrush3;
            public static readonly Brush PhoneSubtleBrush4;
            public static readonly Brush PhoneAccentBrush;
            public static readonly Brush PhoneDisabledBrush;
            public static readonly double PhoneFontSizeLarge = 20d;
            public static readonly double PhoneFontSizeSmall = 12d;
            public static readonly double PhoneFontSizeSuperSmall = 10d;
            public static readonly FontFamily CodeFontFamily = new FontFamily("Consolas");
            public static Thickness SectionMargin = new Thickness(6d, 0d, 0d, 0d);

            static VisualParameters()
            {
                ThemeBrandBrush = Application.Current.Resources["SystemControlHyperlinkTextBrush"] as Brush;
                PhoneAccentBrush = Application.Current.Resources["SystemControlForegroundAccentBrush"] as Brush;
                PhoneSubtleBrush = Application.Current.Resources["SystemControlForegroundBaseMediumHighBrush"] as Brush;
                PhoneSubtleBrush2 = Application.Current.Resources["SystemControlForegroundBaseMediumBrush"] as Brush;
                PhoneSubtleBrush3 = Application.Current.Resources["SystemControlForegroundBaseMediumLowBrush"] as Brush;
                PhoneSubtleBrush4 = Application.Current.Resources["SystemControlForegroundBaseLowBrush"] as Brush;
                PhoneDisabledBrush = Application.Current.Resources["SystemControlDisabledBaseLowBrush"] as Brush;
            }
        }

        private readonly MarkupBox control;
        private readonly Stack<SectionStyle> sectionStyles;
        private RichTextBlock markupHost;

        public MarkupRenderer(MarkupBox control)
        {
            this.control = control;
            this.sectionStyles = new Stack<SectionStyle>();
        }

        public override void BeginRender()
        {
            this.markupHost = this.control.MarkupHost;
            this.markupHost.Blocks.Clear();
            base.BeginRender();
        }

        public override void EndRender()
        {
            base.EndRender();
            this.markupHost = null;
            this.sectionStyles.Clear();
        }

        public override void EndRenderSection()
        {
            base.EndRenderSection();
            this.sectionStyles.Pop();
        }

        protected override Block CreateBlock(BlockType blockType, Tag tag)
        {
            Block block = null;

            switch (blockType)
            {
                case BlockType.Section:
                    this.sectionStyles.Push(new SectionStyle());
                    break;

                case BlockType.Paragraph:
                    block = new Paragraph();
                    break;

                case BlockType.List:
                    this.sectionStyles.Push(new SectionStyle());
                    break;

                case BlockType.ListItem:
                    //todo: get it done, use unicode
                    block = new Paragraph
                    {
                        TextIndent = VisualParameters.TextIndent,
                        Inlines =
                        {
                            new Run { Text = $"{Whitespace.ThinSpace}\u2022{Whitespace.ThinSpace}" }
                        }
                    };
                    break;

                case BlockType.Header:
                    block = new Paragraph
                    {
                        Foreground = VisualParameters.PhoneAccentBrush,
                        FontSize = VisualParameters.PhoneFontSizeLarge
                    };
                    break;

                case BlockType.Cite1:
                case BlockType.Cite2:
                case BlockType.Cite3:
                case BlockType.Cite4:
                    block = new Paragraph
                    {
                        Foreground =
                            blockType == BlockType.Cite1 ? VisualParameters.PhoneSubtleBrush :
                            blockType == BlockType.Cite2 ? VisualParameters.PhoneSubtleBrush2 :
                            blockType == BlockType.Cite3 ? VisualParameters.PhoneSubtleBrush3 :
                            VisualParameters.PhoneSubtleBrush4,
                    };
                    break;

                case BlockType.Quote:
                    this.sectionStyles.Push(new SectionStyle
                    {
                        Foreground = VisualParameters.PhoneSubtleBrush,
                        FontSize = VisualParameters.PhoneFontSizeSmall,
                        Margin = VisualParameters.SectionMargin,
                    });
                    break;

                case BlockType.Footnote:
                    block = new Paragraph
                    {
                        Foreground = tag.Name == RsdnMarkupReference.KnownTags.Moderator ? VisualParameters.PhoneAccentBrush : VisualParameters.PhoneSubtleBrush,
                        FontSize = VisualParameters.PhoneFontSizeSuperSmall,
                    };
                    break;

                case BlockType.Cut:
                    this.sectionStyles.Push(new SectionStyle());
                    break;

                case BlockType.Code:
                    this.sectionStyles.Push(new SectionStyle
                    {
                        FontFamily = VisualParameters.CodeFontFamily,
                        FontSize = VisualParameters.PhoneFontSizeSmall,
                        Margin = VisualParameters.SectionMargin,
                    });
                    break;

                case BlockType.HorizontalLine:
                    //todo: get it done, use unicode
                    block = new Paragraph
                    {
                        Inlines =
                        {
                            new Run { Text = String.Concat(Enumerable.Repeat("\u2015", 12)) }
                        }
                    };
                    break;

                default:
                    break;
            }

            if (block != null && this.sectionStyles.Count > 0)
            {
                var computedStyle = this.sectionStyles.Aggregate(SectionStyle.Compute);
                computedStyle.Apply(block);
            }

            return block;
        }

        protected override Inline CreateInline(InlineType inlineType, Tag tag)
        {
            Inline inline = null;

            switch (inlineType)
            {
                case InlineType.Bold:
                    inline = new Bold();
                    break;

                case InlineType.Italic:
                    inline = new Italic();
                    break;

                case InlineType.StrikeThrough:
                    inline = new Span { Foreground = VisualParameters.PhoneDisabledBrush };
                    break;

                case InlineType.Underline:
                    inline = new Underline();
                    break;

                case InlineType.Hyperlink:
                    {
                        Uri uri = null;
                        if (Uri.TryCreate(tag.Value, UriKind.Absolute, out uri))
                        {
                            inline = new Hyperlink
                            {
                                NavigateUri = uri,
                                Foreground = VisualParameters.PhoneAccentBrush
                            };
                        }
                    }
                    break;

                case InlineType.Image:
                    inline = CreateImageInline(tag);
                    break;

                case InlineType.PhoneNumber:
                    inline = new Span();
                    break;

                case InlineType.Command:
                    {
                        var commandContent = tag.ToString();
                        if (String.IsNullOrWhiteSpace(commandContent))
                        {
                            commandContent = tag.Value;
                        }

                        Uri uri = null;
                        if (Uri.TryCreate(tag.Value, UriKind.Absolute, out uri))
                        {
                            inline = new InlineUIContainer
                            {
                                Child = new HyperlinkButton
                                {
                                    Content = commandContent,
                                    Command = this.control.Command,
                                    CommandParameter = uri,
                                    Style = this.control.HyperlinkButtonStyle,
                                    Foreground = VisualParameters.ThemeBrandBrush
                                }
                            };
                        }
                    }
                    break;

                case InlineType.Code:
                    inline = new Span
                    {
                        FontFamily = VisualParameters.CodeFontFamily,
                    };
                    break;

                default:
                    break;
            }

            if (inline == null)
            {
                inline = new Span();
            }

            return inline;
        }

        protected override Inline CreateRun(string text, ContentType contentType)
        {
            var run = new Run { Text = text };

            if (contentType == ContentType.Emoji)
            {
                run.FontSize = VisualParameters.PhoneFontSizeLarge;
            }
            else if (contentType == ContentType.CodeKeyword)
            {
                run.Foreground = VisualParameters.PhoneAccentBrush;
                run.FontWeight = FontWeights.SemiBold;
            }

            return run;
        }

        protected override void AddBlock(Block block)
        {
            if (block != null)
            {
                this.markupHost.Blocks.Add(block);
            }
        }

        protected override bool AddBlock(Block block, Block nestedBlock)
        {
            return false;
        }

        protected override bool AddInline(Block block, Inline inline)
        {
            var paragraph = block as Paragraph;

            if (paragraph != null)
            {
                paragraph.Inlines.Add(inline);
                return true;
            }

            return false;
        }

        protected override bool AddInline(Inline inline, Inline nestedInline)
        {
            var span = inline as Span;

            if (span != null)
            {
                span.Inlines.Add(nestedInline);
                return true;
            }

            return false;
        }

        private static Inline CreateImageInline(Tag tag)
        {
            ImageSource source = null;

            Uri uri = null;
            if (tag.Value.StartsWith("data:", StringComparison.Ordinal) || Uri.TryCreate(tag.Value, UriKind.Absolute, out uri) == false)
            {
                // decode base64
                source = DecodeBase64Image(tag.Value);
            }
            else if (uri != null)
            {
                source = new BitmapImage
                {
                    UriSource = uri,
                    CreateOptions = BitmapCreateOptions.None
                };
            }

            if (source != null)
            {
                return new InlineUIContainer
                {
                    Child = new ImageEx
                    {
                        Source = source,
                        Stretch = Stretch.None,
                    }
                };
            }

            return null;
        }

        private static ImageSource DecodeBase64Image(string base64)
        {
            //todo: handle format data:image/<jpeg|png|gif|webp>;base64,<base64>
            var commaIdx = base64.IndexOf(',');
            if (commaIdx > -1)
            {
                base64 = base64.Substring(commaIdx + 1);
            }

            // read stream
            byte[] bytes = null;
            try
            {
                bytes = Convert.FromBase64String(base64);
            }
            catch (FormatException)
            {
                return null;
            }

            // create bitmap
            using (var ms = new InMemoryRandomAccessStream())
            {
                // Writes the image byte array in an InMemoryRandomAccessStream
                // that is needed to set the source of BitmapImage.
                using (var writer = new DataWriter(ms.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(bytes);

                    // The GetResults here forces to wait until the operation completes
                    // (i.e., it is executed synchronously), so this call can block the UI.
                    writer.StoreAsync().GetResults();
                }

                var image = new BitmapImage();
                image.SetSource(ms);
                return image;
            }
        }
    }
}