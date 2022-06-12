using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AwesomeChatUWP3
{
	[XamlCompilation(XamlCompilationOptions.Compile)]

	public partial class ChatPage : ContentPage
	{
		string login = "";
		public ObservableCollection<UserMessage> Messages;
		public ChatPage(string login)
		{
			this.login = login;
			InitializeComponent();
			Messages = new ObservableCollection<UserMessage>();
			ChatList.ItemsSource = Messages;
		}
		private void SendMessage_Pressed(object sender, EventArgs e)
		{
			var text = FieldMassge?.Text != null ? FieldMassge.Text : "";
			if (text.Length != 0)
			{
				AddMessage(login, text);
				FieldMassge.Text = "";
			}
		}
		private void AddMessage(string user, string message)
		{
			var u = new UserMessage(user, message);
			Messages.Add(u);
			ChatList.ScrollTo(u, ScrollToPosition.End, true);
		}

		private void UndoNavigation_Pressed(object sender, EventArgs e)
		{
			Navigation.PopModalAsync();
		}
	}

	public class UserMessage
	{
		public string Username { get; private set; }
		public string Message { get; private set; }
		public UserMessage(string user, string message)
		{
			Username = user;
			Message = message;
		}
	}
}