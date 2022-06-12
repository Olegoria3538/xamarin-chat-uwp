using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AwesomeChatUWP3
{
	public partial class MainPage : ContentPage
	{
		private const string OurTitle = "Месседжер для меня и моего расстройства";
		private bool isLogin = false;
		private string login = "";
		private ChatPage chatPage = null;

		public MainPage()
		{

			Title = "Мой awesome месседжер";
			InitializeComponent();
			BtnOpenChat.SetValue(IsVisibleProperty, false);
			BtnLogout.SetValue(IsVisibleProperty, false);
		}
		private void OpenChat_Pressed(object sender, EventArgs e)
		{
			if (chatPage != null)
			{
				Navigation.PushModalAsync(chatPage);
			}
		}
		private async void Login_Pressed(object sender, EventArgs e)
		{
			var LoginInstace = new Login();
			LoginInstace.Disappearing += async (sender2, e2) =>
			{
				isLogin = LoginInstace.isLogin;
				if (LoginInstace.isLogin)
				{
					login = LoginInstace.login;
					UserName.Text = LoginInstace.login;
					StatusUser.Text = "online";
					StatusUser.TextColor = Color.Green;
					BtnLogin.SetValue(IsVisibleProperty, false);
					BtnCreateAccount.SetValue(IsVisibleProperty, false);
					BtnOpenChat.SetValue(IsVisibleProperty, true);
					BtnLogout.SetValue(IsVisibleProperty, true);
					chatPage = new ChatPage(login);
					await Task.Delay(1);
					OpenChat_Pressed(sender, e);
				}
			};
			await Navigation.PushModalAsync(LoginInstace);
		}
		private void CreateAccount_Pressed(object sender, EventArgs e)
		{
			var CreateAccountInstance = new CreateAccount();
			CreateAccountInstance.Disappearing += async (sender2, e2) =>
			{
				if (CreateAccountInstance.succes)
				{
					await Task.Delay(1);
					Login_Pressed(sender, e);
				}
			};
			Navigation.PushModalAsync(CreateAccountInstance);
		}
		private async void ButtonLogout_Pressed(object sender, EventArgs e)
		{
			var sheetResult = await DisplayAlert("Выйти?", "", "Да", "Нет");
			if (sheetResult)
			{
				UserName.Text = ":(";
				StatusUser.Text = "offline";
				StatusUser.TextColor = Color.Gray;
				BtnLogin.SetValue(IsVisibleProperty, true);
				BtnCreateAccount.SetValue(IsVisibleProperty, true);
				BtnOpenChat.SetValue(IsVisibleProperty, false);
				BtnLogout.SetValue(IsVisibleProperty, false);
				chatPage = null;
			}
		}
	}
}
