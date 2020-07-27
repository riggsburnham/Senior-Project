using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodsAmongSheep.Shared;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.Shared.Models;

namespace GasXamChat.Services
{
    public class GroupChatWebService : IGroupChatWebService
    {
        public async Task<GroupChat> GetGroupChat(int workoutGroupId)
        {
            using (GasContext context = new GasContext())
            {
                context.SetupServer();
                GasContextController contextController = new GasContextController(context);
                GasGroupChatsController groupChatsController = contextController.GasGroupChatsController;
                //return groupChatsController.FindWorkoutGroupGroupChat(workoutGroupId);
                return await Task.Run(() => groupChatsController.FindWorkoutGroupGroupChat(workoutGroupId));
            }
        }

        public async Task<IList<Message>> GetMessages(int groupChatId)
        {
            using (GasContext context = new GasContext())
            {
                context.SetupServer();
                GasContextController contextController = new GasContextController(context);
                GasGroupChatsController groupChatsController = contextController.GasGroupChatsController;
                return await Task.Run(() => groupChatsController.FindGroupChatMessages(groupChatId));
            }
        }

        public async Task SendMessage(Message message)
        {
            using (GasContext context = new GasContext())
            {
                context.SetupServer();
                GasContextController contextController = new GasContextController(context);
                GasGroupChatsController groupChatsController = contextController.GasGroupChatsController;
                await groupChatsController.CreateGasMessage(message);
            }
        }
    }
}
