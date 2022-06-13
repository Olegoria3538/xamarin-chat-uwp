using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Grpc.Core;
using static SimpleChatApp.GrpcService.ChatService;

namespace AwesomeChatUWP3
{
	public partial class MainPage : ContentPage
	{
		private const string OurTitle = "Месседжер для меня и моего расстройства";
		private bool isLogin = false;
		private string login = "";
		private ChatPage chatPage = null;
		private ChatServiceClient chatServiceClient = null;
		private SimpleChatApp.GrpcService.Guid guid = null;

		public MainPage()
		{

			Title = "Мой awesome месседжер";
			InitializeComponent();
			BtnOpenChat.SetValue(IsVisibleProperty, false);
			BtnLogout.SetValue(IsVisibleProperty, false);
			chatServiceClient = new ChatServiceClient(new Channel("localhost", 30051, ChannelCredentials.Insecure));
		}
		private void OpenChat_Pressed(object sender, EventArgs e)
		{
			if (chatPage != null)
			{
				Navigation.PushModalAsync(chatPage);
			}
		}
		private void LoginHandler(object sender, EventArgs e)
		{
			if (chatPage != null)
			{
				Navigation.PushModalAsync(chatPage);
			}
		}
		private async void CreateLoginModal()
		{
			var LoginInstace = new Login(chatServiceClient);
			LoginInstace.Disappearing += async (sender2, e2) =>
			{
				isLogin = LoginInstace.isLogin;
				if (LoginInstace.isLogin)
				{
					guid = LoginInstace.guid;
					login = LoginInstace.login;
					UserName.Text = LoginInstace.login;
					StatusUser.Text = "online";
					StatusUser.TextColor = Color.Green;
					BtnLogin.SetValue(IsVisibleProperty, false);
					BtnCreateAccount.SetValue(IsVisibleProperty, false);
					BtnOpenChat.SetValue(IsVisibleProperty, true);
					BtnLogout.SetValue(IsVisibleProperty, true);
					await Task.Delay(1);
					chatPage = new ChatPage(guid, chatServiceClient);
					Navigation.PushModalAsync(chatPage);
				}
			};
			await Navigation.PushModalAsync(LoginInstace);
		}
		private async void Login_Pressed(object sender, EventArgs e)
		{
			CreateLoginModal();
		}
		private void CreateAccount_Pressed(object sender, EventArgs e)
		{
			var CreateAccountInstance = new CreateAccount(chatServiceClient);
			CreateAccountInstance.Disappearing += async (sender2, e2) =>
			{
				if (CreateAccountInstance.succes)
				{
					await Task.Delay(1);
					CreateLoginModal();
				}
			};
			Navigation.PushModalAsync(CreateAccountInstance);
		}
		private async void Logout()
		{
			UserName.Text = ":(";
			StatusUser.Text = "offline";
			StatusUser.TextColor = Color.Gray;
			BtnLogin.SetValue(IsVisibleProperty, true);
			BtnCreateAccount.SetValue(IsVisibleProperty, true);
			BtnOpenChat.SetValue(IsVisibleProperty, false);
			BtnLogout.SetValue(IsVisibleProperty, false);
			chatPage = null;
			chatServiceClient.Unsubscribe(guid);
		}
		private async void ButtonLogout_Pressed(object sender, EventArgs e)
		{
			var sheetResult = await DisplayAlert("Выйти?", "", "Да", "Нет");
			if (sheetResult)
			{
				Logout();
			}
		}
	}
}
