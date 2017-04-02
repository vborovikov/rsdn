namespace Rsdn.Community.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Relay.PresentationModel;

    public abstract class ModelPresenter<TModel> : Presenter
        where TModel : IIdentifiable, new()
    {
        private TModel model;

        protected ModelPresenter()
        {
            this.model = new TModel();
        }

        internal TModel Model
        {
            get
            {
                return this.model;
            }
            set
            {
                this.model = value;
                OnModelChanged();
            }
        }

        protected virtual void OnModelChanged()
        {
            RaisePropertyChanged();
        }
    }
}