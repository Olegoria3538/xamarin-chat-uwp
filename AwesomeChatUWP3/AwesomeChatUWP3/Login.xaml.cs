using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AwesomeChatUWP3
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public string login = "";
        public string pass = "";
        public bool isLogin = false;
        public Login()
        {
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
                await Task.Delay(1000);
                isLogin = true;
                Navigation.PopModalAsync();
            }        
        }
    }
}