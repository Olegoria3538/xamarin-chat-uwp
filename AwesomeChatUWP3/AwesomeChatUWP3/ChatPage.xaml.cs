using SimpleChatApp.GrpcService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SimpleChatApp.GrpcService.ChatService;
using Google.Protobuf.WellKnownTypes;

namespace AwesomeChatUWP3
{
	[XamlCompilation(XamlCompilationOptions.Compile)]

	public partial class ChatPage : ContentPage
	{
		string login = "";
		private ChatServiceClient chatServiceClient = null;
		public SimpleChatApp.GrpcService.Guid guid = null;
		public ObservableCollection<UserMessage> Messages;
		

		public ChatPage(string login, SimpleChatApp.GrpcService.Guid guid, ChatServiceClient chatServiceClient)
		{
			this.login = login;
			this.chatServiceClient = chatServiceClient;
			this.guid = guid;
			InitializeComponent();
			Messages = new ObservableCollection<UserMessage>();
			ChatList.ItemsSource = Messages;
			initChat();
		}
		private async void initChat()
		{
			await GetOldMsg();
			await Subscribe();
		}

		private async Task GetOldMsg()
		{
			var maxD = DateTime.MaxValue;
			var minD = DateTime.MinValue;
			var timeIntervalRequest = new TimeIntervalRequest()
			{
				StartTime = Timestamp.FromDateTime(minD.ToUniversalTime()),
				EndTime = Timestamp.FromDateTime(maxD.ToUniversalTime()),
				Sid = guid
			};
			var res = await chatServiceClient.GetLogsAsync(timeIntervalRequest);
			if(res.ActionStatus == ActionStatus.Allowed)
            {
				foreach (var x in res.Logs)
                {
					AddMessage(x.PlayerLogin, x.Text);
				}
            }	
		}
		private async Task Subscribe()
		{
			var streaming = chatServiceClient.Subscribe(guid);
			var canselToken = new System.Threading.CancellationToken();
			while (await streaming.ResponseStream.MoveNext(canselToken))
            {
				var messages = streaming.ResponseStream.Current;
				foreach (var x in messages.Logs)
                {
					AddMessage(x.PlayerLogin, x.Text);
				}
            }
		}
		private async void SendMsg(string msg)
		{
			var msgObj = new OutgoingMessage()
			{
				Sid = guid,
				Text = msg
			};
			await chatServiceClient.WriteAsync(msgObj);
 		}
		private void SendMessage_Pressed(object sender, EventArgs e)
		{
			var text = FieldMassge?.Text != null ? FieldMassge.Text : "";
			if (text.Length != 0)
			{
				SendMsg(text);
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