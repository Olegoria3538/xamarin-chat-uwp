using SimpleChatApp.CommonTypes;
using SimpleChatApp.GrpcService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SimpleChatApp.GrpcService.ChatService;

namespace AwesomeChatUWP3
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateAccount : ContentPage
    {
        public bool succes = false;
        private ChatServiceClient chatServiceClient = null;
        public CreateAccount(ChatServiceClient chatServiceClient)
        {
            this.chatServiceClient = chatServiceClient;
            InitializeComponent();
        }
        private void UndoNavigation_Pressed(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
        private async void CreateAccount_Pressed(object sender, EventArgs e)
        {
            var login = Login?.Text != null ? Login.Text : "";
            var pass = Password?.Text != null ? Password.Text : "";
            var pass2 = Password2?.Text != null ? Password2.Text : "";
            if (!(login.Length == 0 || pass.Length == 0 || pass2.Length == 0))
            {
                if(pass != pass2)
                {
                    await DisplayAlert("Пароли не равны", "", "OK");
                } else
                {
                    var userData = new UserData()
                    {
                        Login = login,
                        PasswordHash = SHA256.GetStringHash(pass)
                    };
                    var res = await chatServiceClient.RegisterNewUserAsync(userData);
                    if(res.Status == SimpleChatApp.GrpcService.RegistrationStatus.RegistrationSuccessfull)
                    {
                        succes = true;
                        Navigation.PopModalAsync();
                    } else
                    {
                        await DisplayAlert("Беда", "", "OK");
                    }
                }
            }
        }
    }
}