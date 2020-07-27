using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodsAmongSheep.Shared.Models;

namespace GodsAmongSheep.Shared.Controllers
{
    public class GasGroupChatsController
    {
        private readonly GasContext _gasContext;
        public GasGroupChatsController(GasContext gasContext)
        {
            _gasContext = gasContext;
        }

        private IEnumerable<GroupChat> GetGroupChats => _gasContext.GroupChats;
        private IEnumerable<Message> GetMessages => _gasContext.Messages;

        public async Task<GroupChat> FindGroupChat(int id) => await Task.Run(() => GetGroupChats.FirstOrDefault(gc => gc.GroupChatId == id));
        public async Task<Message> FindMessage(int id) => await Task.Run(() => GetMessages.FirstOrDefault(m => m.MessageId == id));

        //public async Task<GroupChat> FindWorkoutGroupGroupChat(int workoutGroupId)
        public GroupChat FindWorkoutGroupGroupChat(int workoutGroupId)
        {
            using (GasContext context = new GasContext())
            {
                context.SetupServer();
                //GroupChat groupChat = await Task.Run(() => GetGroupChats.FirstOrDefault(gc => gc.WorkoutGroupId == workoutGroupId));
                GroupChat groupChat = GetGroupChats.FirstOrDefault(gc => gc.WorkoutGroupId == workoutGroupId);
                return groupChat;
            }
        }

        //public async Task<IList<Message>> FindGroupChatMessages(int groupChatId)
        public IList<Message> FindGroupChatMessages(int groupChatId)
        {
            using (GasContext context = new GasContext())
            {
                context.SetupServer();
                //IList<Message> messages = await Task.Run(() => GetMessages.Where(m => m.GroupChatId == groupChatId).ToList());
                IList<Message> messages = GetMessages.Where(m => m.GroupChatId == groupChatId).ToList();
                return messages;
            }
        }

        private async Task InsertGroupChat(GroupChat gc)
        {
            using (var context = new GasContext())
            {
                context.SetupServer();
                await Task.Run(() => context.GroupChats.Add(gc));
                context.SaveChanges();
            }
        }

        private async Task InsertMessage(Message m)
        {
            using (var context = new GasContext())
            {
                context.SetupServer();
                await Task.Run(() => context.Messages.Add(m));
                context.SaveChanges();
            }
        }

        private async Task DeleteGroupChat(GroupChat gc)
        {
            using (var context = new GasContext())
            {
                context.SetupServer();
                await Task.Run(() => context.GroupChats.Remove(gc));
                context.SaveChanges();
            }
        }

        private async Task DeleteMessage(Message m)
        {
            using (var context = new GasContext())
            {
                context.SetupServer();
                await Task.Run(() => context.Messages.Remove(m));
                context.SaveChanges();
            }
        }

        private async Task UpdateGroupChat(GroupChat updated_gc)
        {
            using (GasContext context = new GasContext())
            {
                context.SetupServer();
                GroupChat old_gc = await FindGroupChat(updated_gc.GroupChatId);
                await Task.Run(() => DeleteGroupChat(old_gc));
                await Task.Run(() => InsertGroupChat(updated_gc));
                context.SaveChanges();
            }
        }

        // shouldn't ever need to update a message... (users will not have the ability to edit what they said)

        public async Task CreateGasGroupChat(GroupChat gc)
        {
            try
            {
                if (gc == null) throw new Exception("!!! Cannot add null group chat !!!");
                await InsertGroupChat(gc);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public async Task CreateGasMessage(Message m)
        {
            try
            {
                if (m == null) throw new Exception("!!! Cannot add null message !!!");
                await InsertMessage(m);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public async Task UpdateGasGroupChat(GroupChat gc)
        {
            try
            {
                if (gc == null) throw new Exception("!!! Cannot update null group chat !!!");
                GroupChat gcExists = await FindGroupChat(gc.GroupChatId);
                if (gcExists == null) throw new Exception("!!! A group chat does not exist with that id !!!");
                await UpdateGroupChat(gc);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        // again should never need to update a message, users do not have the ability to edit what they have said...

        public async Task DeleteGasGroupChat(GroupChat gc)
        {
            try
            {
                GroupChat gcExists = await FindGroupChat(gc.GroupChatId);
                if (gcExists == null) throw new Exception("!!! Cannot delete a group chat that does not exist!!!");
                await DeleteGroupChat(gcExists);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        public async Task DeleteGasMessage(Message m)
        {
            try
            {
                Message mExists = await FindMessage(m.MessageId);
                if (mExists == null) throw new Exception("!!! Cannot delete a message that does not exist!!!");
                await DeleteMessage(mExists);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
