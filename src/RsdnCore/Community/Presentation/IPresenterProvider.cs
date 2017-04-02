namespace Rsdn.Community.Presentation
{
    using Relay.PresentationModel;

    public interface IPresenterProvider
    {
        TPresenter Get<TPresenter>() where TPresenter : Presenter;
    }
}