namespace Rsdn.Xaml.Interactivity
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading.Tasks;
    using Windows.Foundation.Collections;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using XamlHub = Windows.UI.Xaml.Controls.Hub;

    public class Hub : XamlHub
    {
        public static readonly DependencyProperty SectionsSourceProperty =
            DependencyProperty.Register(nameof(SectionsSource), typeof(object), typeof(Hub),
                new PropertyMetadata(null, HandleSectionsSourcePropertyChanged));

        public static readonly DependencyProperty SectionTemplateProperty =
            DependencyProperty.Register(nameof(SectionTemplate), typeof(DataTemplate), typeof(Hub),
                new PropertyMetadata(null, HandleTemplatePropertiesChanged));

        public static readonly DependencyProperty SectionHeaderTemplateProperty =
            DependencyProperty.Register(nameof(SectionHeaderTemplate), typeof(DataTemplate), typeof(Hub),
                new PropertyMetadata(null, HandleTemplatePropertiesChanged));

        public object SectionsSource
        {
            get { return (object)GetValue(SectionsSourceProperty); }
            set { SetValue(SectionsSourceProperty, value); }
        }

        public DataTemplate SectionTemplate
        {
            get { return (DataTemplate)GetValue(SectionTemplateProperty); }
            set { SetValue(SectionTemplateProperty, value); }
        }

        public DataTemplate SectionHeaderTemplate
        {
            get { return (DataTemplate)GetValue(SectionHeaderTemplateProperty); }
            set { SetValue(SectionHeaderTemplateProperty, value); }
        }

        private static void HandleTemplatePropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Hub)?.OnTemplatesChanged(e);
        }

        private static void HandleSectionsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Hub)?.OnSectionsSourceChanged(e);
        }

        private void OnTemplatesChanged(DependencyPropertyChangedEventArgs e)
        {
            ResetSections();
        }

        private void OnSectionsSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            UnsubscribeFromChanges(e.OldValue);
            SubscribeToChanges(e.NewValue);
        }

        private void UnsubscribeFromChanges(object sectionsSource)
        {
            var observableVector = sectionsSource as IObservableVector<object>;
            if (observableVector != null)
            {
                observableVector.VectorChanged -= HandleSectionVectorChanged;
            }
            var observableCollection = sectionsSource as INotifyCollectionChanged;
            if (observableCollection != null)
            {
                observableCollection.CollectionChanged -= HandleSectionCollectionChanged;
            }
        }

        private void SubscribeToChanges(object sectionsSource)
        {
            var observableVector = sectionsSource as IObservableVector<object>;
            if (observableVector != null)
            {
                observableVector.VectorChanged += HandleSectionVectorChanged;
            }
            else
            {
                var observableCollection = sectionsSource as INotifyCollectionChanged;
                if (observableCollection != null)
                {
                    observableCollection.CollectionChanged += HandleSectionCollectionChanged;
                }
            }
        }

        private void HandleSectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        var i = e.NewStartingIndex;
                        foreach (var item in e.NewItems)
                        {
                            AddSection(item, i++);
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    {
                        var i = e.OldStartingIndex;
                        foreach (var item in e.OldItems)
                        {
                            RemoveSection(item, i++);
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    ResetSections();
                    break;
            }
        }

        private void ResetSections()
        {
            this.Sections.Clear();

            var items = this.SectionsSource as IEnumerable;
            if (items != null)
            {
                foreach (var item in items)
                {
                    AddSection(item);
                }
            }
        }

        private void RemoveSection(object item, int index)
        {
            this.Sections.RemoveAt(index);
        }

        private void AddSection(object item, int index = -1)
        {
            var section = new HubSection
            {
                DataContext = item,
                Header = item,
                HeaderTemplate = this.SectionHeaderTemplate,
                ContentTemplate = this.SectionTemplate,
            };
            if (index > -1)
            {
                this.Sections.Insert(index, section);
            }
            else
            {
                this.Sections.Add(section);
            }
        }

        private void HandleSectionVectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs e)
        {
            switch (e.CollectionChange)
            {
                case CollectionChange.Reset:
                    ResetSections();
                    break;

                case CollectionChange.ItemInserted:
                    AddSection(sender[(int)e.Index], (int)e.Index);
                    break;

                case CollectionChange.ItemRemoved:
                    RemoveSection(sender[(int)e.Index], (int)e.Index);
                    break;
            }
        }
    }
}