using SimpleChatApp.CommonTypes;
using SimpleChatApp.GrpcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SimpleChatApp.GrpcService.ChatService;

namespace AwesomeChatUWP3
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public string login = "";
        public string pass = "";
        public bool isLogin = false;
        private ChatServiceClient chatServiceClient = null;
        public Login(ChatServiceClient chatServiceClient)
        {
            this.chatServiceClient = chatServiceClient;
            InitializeComponent();
        }
        private void UndoNavigation_Pressed(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void Login_Pressed(object sender, EventArgs e)
        {
            if (LoginInput?.Text != null)
            {
                login = LoginInput.Text;
            }
            if (Password?.Text != null)
            {
                pass = Password.Text;
            }
            if (!(login == null || login?.Length == 0 || pass?.Length == null || pass?.Length == 0))
            {
                var userData = new UserData()
                {
                    Login = login,
                    PasswordHash = SHA256.GetStringHash(pass)
                };
                var authorizationData = new AuthorizationData()
                {
                    ClearActiveConnection = true,
                    UserData = userData
                };
                var res = await chatServiceClient.LogInAsync(authorizationData);
                if(res.Status == SimpleChatApp.GrpcService.AuthorizationStatus.AuthorizationSuccessfull)
                {
                    isLogin = true;
                    Navigation.PopModalAsync();
                } else
                {
                    await DisplayAlert("Беда", "", "OK");
                }
            }        
        }
    }
}