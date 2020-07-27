using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GasXamChat.Services;
using GodsAmongSheep.Shared;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.Shared.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TableDependency.SqlClient;
using Xamarin.Forms;

namespace GodsAmongSheep.ViewModels
{
    public class GroupChatPageViewModel : INotifyPropertyChanged
    {
        #region private member variables
        private WorkoutGroupPageViewModel _parent;
        private WorkoutGroup _workoutGroup;
        private ObservableCollection<Message> _messages;
        private string _messageToSend;
        private GasUser _applicationUser;
        private GroupChat _currentGroupChat;
        #endregion

        #region GroupChatPageViewModel constructor
        public GroupChatPageViewModel(WorkoutGroupPageViewModel parent, WorkoutGroup workoutGroup)
        {
            _parent = parent;
            _workoutGroup = workoutGroup;
            _applicationUser = _parent.Parent.Parent.User;
            _currentGroupChat = Task.Run(GetCurrentGroupChat).Result;
            SendMessageCommand = new Command(async () => await SendMessage());
            new Thread(async () => await GetMessages()) { IsBackground = true}.Start();
        }
        #endregion


        #region property changed
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region properties
        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set
            {
                _messages = value;
                NotifyPropertyChanged();
            }
        }

        public string MessageToSend
        {
            get => _messageToSend;
            set
            {
                _messageToSend = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region private functions
        private async Task GetMessages()
        {
            // continuously update to find the messages
            while (true)
            {
                IGroupChatWebService groupChatWebService = new GroupChatWebService();
                ObservableCollection<Message> messagesCollection = new ObservableCollection<Message>();
                IList<Message> messages = await groupChatWebService.GetMessages(_currentGroupChat.GroupChatId);
                IList<Message> sortedMessages = messages.OrderBy(msg => msg.Date).ToList();
                foreach (Message m in sortedMessages.Reverse())
                {
                    messagesCollection.Add(m);
                }
                Messages = messagesCollection;
                messagesCollection = null;
                messages = null;
                sortedMessages = null;
            }
        }

        private async Task<GroupChat> GetCurrentGroupChat()
        {
            IGroupChatWebService groupChatWebService = new GroupChatWebService();
            return await groupChatWebService.GetGroupChat(_workoutGroup.WorkoutGroupId);
        }
        #endregion

        #region commands
        public Command SendMessageCommand { get; }

        private async Task SendMessage()
        {
            Message message = new Message()
            {
                Date = DateTime.Now,
                GroupChatId = _currentGroupChat.GroupChatId,
                MText = MessageToSend,
                UserId = _applicationUser.UserId,
                Username = _applicationUser.Username
            };
            IGroupChatWebService groupChatWebService = new GroupChatWebService();
            await groupChatWebService.SendMessage(message);
            MessageToSend = "";
        }
        #endregion
    }
}
