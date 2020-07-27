using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GodsAmongSheep.Shared.Models;

namespace GasXamChat.Services
{
    public interface IGroupChatWebService
    {
        Task<GroupChat> GetGroupChat(int workoutGroupId);
        Task<IList<Message>> GetMessages(int groupChatId);
        Task SendMessage(Message message);
    }
}
