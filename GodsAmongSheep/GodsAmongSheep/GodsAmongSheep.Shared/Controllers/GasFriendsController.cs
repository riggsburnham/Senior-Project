using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GodsAmongSheep.Shared.Models;

namespace GodsAmongSheep.Shared.Controllers
{
    public class GasFriendsController
    {
        private readonly GasContext _gasContext;
        public GasFriendsController(GasContext gasContext)
        {
            _gasContext = gasContext;
        }

        public IEnumerable<Friend> GetFriends => _gasContext.Friends;
        //public IEnumerable<GasUser> GetGasUsers => _gasContext.Users;

        private Friend FindFriendBySenderAndRecipient(int senderId, int recipientId) => GetFriends.FirstOrDefault(friend =>
            (friend.SenderId == senderId) && (friend.RecipientId == recipientId) ||
            (friend.SenderId == recipientId) && (friend.RecipientId == senderId)
        );

        public Friend FindFriend(int id) => GetFriends.FirstOrDefault(friend => friend.FriendId == id);

        public Friend FindFriend(int UserId1, int UserId2)
        {
            return GetFriends.FirstOrDefault(friend => friend.SenderId == UserId1 && friend.RecipientId == UserId2 || 
                                                       friend.SenderId == UserId2 && friend.RecipientId == UserId1);
        }

        public IEnumerable<Friend> FindFriendsByGasUser(GasUser user) => GetFriends.Where(friend =>
            (user.UserId == friend.SenderId) || (user.UserId == friend.RecipientId));

        //public GasUser FindGasUser(int id) => GetGasUsers.FirstOrDefault(user => user.UserId == id);
        public GasUser FindGasUser(int id)
        {
            using (var context = new GasContext())
            {
                context.SetupServer();
                var contextController = new GasContextController(context);
                var userController = contextController.GasUsersController;
                return userController.FindGasUser(id);
            }
        }

        public GasUser FindFriendsGasUserByFriend(GasUser user, Friend friend)
        {
            var userId = -1;
            if (user.UserId == friend.SenderId)
            {
                userId = friend.RecipientId;
            }
            else
            {
                userId = friend.SenderId;
            }

            return FindGasUser(userId);
        }

        private void InsertFriend(Friend friend)
        {
            using (var context = new GasContext())
            {
                context.SetupServer();
                context.Friends.Add(friend);
                context.SaveChanges();
            }
            //_gasContext.Friends.Add(friend);
            //_gasContext.SaveChanges();
        }

        private void UpdateFriend(Friend updatedFriend)
        {
            var oldFriend = FindFriend(updatedFriend.FriendId);
            DeleteFriend(oldFriend);
            InsertFriend(updatedFriend);
            //using (var context = new GasContext())
            //{
            //    context.SetupServer();
            //    var oldFriend = FindFriend(updatedFriend.FriendId);
            //    context.Friends.Remove(oldFriend);
            //    context.SaveChanges();
            //    InsertFriend(updatedFriend);//context.Add(updatedFriend);
            //}
        }

        private void DeleteFriend(Friend friend)
        {
            using (var context = new GasContext())
            {
                context.SetupServer();
                context.Friends.Remove(friend);
                context.SaveChanges();
            }
        }

        public void CreateFriend(GasUser sender, GasUser recipient)
        {
            try
            {
                var friend = new Friend()
                {
                    SenderId = sender.UserId,
                    RecipientId = recipient.UserId,
                    IsAccepted = false
                };

                // need to make sure that that the sender and recipient aren't already friends...
                var alreadyFriends = FindFriendBySenderAndRecipient(friend.SenderId, friend.RecipientId);
                if (alreadyFriends != null)
                {
                    throw new Exception("!!! Invalid friend request, the users are already friends or their is a pending friend request !!!");
                }

                InsertFriend(friend);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void UpdateGasFriend(Friend friend)
        {
            try
            {
                if (friend == null)
                {
                    throw new Exception("!!! Cannot update null friend !!!");
                }

                var friendExists = FindFriend(friend.FriendId);
                if (friendExists == null)
                {
                    throw new Exception("!!! A friend does not exist with that id !!!");
                }
                UpdateFriend(friend);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void DeleteFriend(int friendId)
        {
            try
            {
                var friend = FindFriend(friendId);
                if (friend == null)
                {
                    throw new Exception("!!! Cannot delete a friend that doesn't exist !!!");
                }
                DeleteFriend(friend);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
