namespace Rsdn.Community.Presentation
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Community;
    using Interaction.Requests;
    using Interaction.Requests.Credentials;
    using Relay.RequestModel;
    using IUwpCommand = System.Windows.Input.ICommand;

    [Dialog("Rsdn.Xaml.SigninDialog, Rsdn")]
    public class SigninViewModel : DialogViewModel, IEventHandler<CredentialVerificationEvent>
    {
        private readonly IRequestDispatcher requestDispatcher;
        private CancellationTokenSource busySource;
        private string errorMessage;
        private string username;
        private string password;

        public SigninViewModel(IRequestDispatcher requestDispatcher)
        {
            this.requestDispatcher = requestDispatcher;
        }

        public string Username
        {
            get { return this.username; }
            set { this.username = value; RaisePropertyChanged(nameof(this.Username)); }
        }

        public string Password
        {
            get { return this.password; }
            set { this.password = value; RaisePropertyChanged(nameof(this.Password)); }
        }

        public string ErrorMessage
        {
            get { return this.errorMessage; }
            private set { this.errorMessage = value; RaisePropertyChanged(nameof(this.ErrorMessage)); }
        }

        public IUwpCommand SigninCommand => GetCommand(Signin, CanSignin);

        public IUwpCommand CancelCommand => GetCommand(Cancel, CanCancel);

        public void Handle(CredentialVerificationEvent e)
        {
            if (e.CredentialVerificationResult != CredentialVerificationResult.Success)
            {
                //TODO: set proper error message
                this.ErrorMessage = e.CredentialVerificationResult.ToString();
            }
            else
            {
                CloseDialog();
            }
        }

        private bool CanCancel() => this.IsBusy;

        private Task Cancel()
        {
            this.busySource?.Cancel();
            CloseDialog();
            return Task.CompletedTask;
        }

        private bool CanSignin() => this.IsBusy == false &&
            String.IsNullOrWhiteSpace(this.Username) == false &&
            String.IsNullOrWhiteSpace(this.Password) == false;

        private async Task Signin()
        {
            using (this.busySource = new CancellationTokenSource())
            using (Busy())
            {
                await this.requestDispatcher.ExecuteAsync(new SigninCommand(this.Username, this.Password)
                {
                    CancellationToken = this.busySource.Token
                });
                CloseDialog();
            }
        }
    }
}