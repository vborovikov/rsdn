namespace Rsdn.Community.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Relay.PresentationModel;

    public abstract class DataViewModel<TData> : Presenter
        where TData : IIdentifiable, new()
    {
        private TData data;

        protected DataViewModel()
        {
            this.data = new TData();
        }

        internal TData Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
                OnDataChanged();
            }
        }

        protected virtual void OnDataChanged()
        {
            RaisePropertyChanged();
        }
    }
}