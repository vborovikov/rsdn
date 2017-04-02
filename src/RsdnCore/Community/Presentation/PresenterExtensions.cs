namespace Rsdn.Community.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Community;
    using Relay.PresentationModel;

    public static class PresenterExtensions
    {
        public static async Task<ItemsState> RefreshItemsAsync<TPresenter, TModel>(
            ObservableCollection<TPresenter> targetCollection,
            Task<IEnumerable<TModel>> sourceCollectionTask,
            Func<TModel, TPresenter> present)
            where TPresenter : Presenter
            where TModel : IIdentifiable
        {
            return RefreshItems(targetCollection, await sourceCollectionTask, present);
        }

        public static ItemsState RefreshItems<TPresenter, TModel>(
            ObservableCollection<TPresenter> targetCollection,
            IEnumerable<TModel> sourceCollection,
            Func<TModel, TPresenter> present)
            where TPresenter : Presenter
            where TModel : IIdentifiable
        {
            var hasAnySource = sourceCollection.Any();

            targetCollection.Clear();
            foreach (var item in sourceCollection)
            {
                targetCollection.Add(present(item));
            }

            return hasAnySource ? ItemsState.Loaded : ItemsState.Empty;
        }
    }
}