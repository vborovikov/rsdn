﻿namespace Rsdn.Community.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Interaction;
    using Interaction.Requests.Forums;
    using Interaction.Requests.Update;
    using NavigationModel;

    [Navigable("Rsdn.Xaml.DirectoryPage, Rsdn")]
    public class DirectoryPresenter : NavigablePresenter
    {
        private readonly IPresenterHost host;

        public DirectoryPresenter(IPresenterHost host)
        {
            this.host = host;
            this.Groups = new ObservableCollection<GroupModel>();
            //this.Forums = (new CollectionViewSource
            //{
            //    Source = this.groups,
            //    IsSourceGrouped = true,
            //    ItemsPath = new PropertyPath(nameof(GroupDetails.Forums)),
            //}).View;
        }

        public ObservableCollection<GroupModel> Groups { get; }

        public ICommand UpdateDirectoryCommand => GetCommand(UpdateDirectoryAsync);

        public ICommand SettingsCommand => GetCommand(OpenSettingsAsync);

        public ICommand VisitForumCommand => GetCommand<ForumModel>(VisitForumAsync);

        protected override async Task OnDeserializingAsync(IDictionary<string, object> state)
        {
            using (Busy())
            {
                await LoadGroupsAsync();
            }
        }

        private Task VisitForumAsync(ForumModel forum)
        {
            this.host.Navigate<ForumPresenter>(forum.Id);
            return Task.FromResult(0);
        }

        private async Task UpdateDirectoryAsync()
        {
            using (Busy())
            {
                await this.host.ExecuteCommandAsync(new UpdateDirectoryCommand());
            }
        }

        private async Task LoadGroupsAsync()
        {
            do
            {
                var groups = await this.host.RunQueryAsync(new GroupsQuery());
                if (groups.Any())
                {
                    this.Groups.Clear();
                    foreach (var group in groups)
                    {
                        this.Groups.Add(group);
                    }
                }
                else
                {
                    await UpdateDirectoryAsync();
                }
            } while (this.Groups.Any() == false);
        }

        private Task OpenSettingsAsync()
        {
            return Task.FromResult(0);
        }
    }
}