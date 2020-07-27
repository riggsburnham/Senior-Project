using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GodsAmongSheep.Shared.Models
{
    public class GroupChat
    {
        [Key]
        public int GroupChatId { get; set; }
        public int WorkoutGroupId { get; set; }
        public string WorkoutGroupName { get; set; }
    }

    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public int GroupChatId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string MText { get; set; }
        public DateTime Date { get; set; }
    }
}
