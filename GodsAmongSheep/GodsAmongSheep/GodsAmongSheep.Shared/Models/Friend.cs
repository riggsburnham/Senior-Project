using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GodsAmongSheep.Shared.Controllers;

namespace GodsAmongSheep.Shared.Models
{
    public class Friend
    {
        public Friend()
        {
            IsAccepted = false;
        }

        [Key]
        public int FriendId { get; set; }

        // public GasUser Friend { get; set; }
        [Required]
        public int SenderId { get; set; }

        [Required]
        public int RecipientId { get; set; }

        public bool IsAccepted { get; set; }

        public Friend ShallowCopy()
        {
            return (Friend)this.MemberwiseClone();
        }

        //public string IsAcceptedString(GasUser user)
        //{
        //    if (IsAccepted) return "";
        //    using (var context = new GasContext())
        //    {
        //        context.SetupServer();
        //        var controller = new GasContextController(context);
        //        var friendController = controller.GasFriendsController;
        //        var friend = friendController.FindFriend(FriendId);
        //        var friendsUser = friendController.FindFriendsGasUserByFriend(user, friend);
        //        return user.UserId == SenderId ? $"{friendsUser.Username} - Request Pending..." : $"{friendsUser.Username} - Accept Request?";
        //    }
        //}

    }
}
