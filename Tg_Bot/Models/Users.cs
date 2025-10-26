using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tg_Bot.Models
{
    public class Users
    {
        [Key]
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Nickname { get; set; }
        public DateTime RecuestDate { get; set; }
    }
}
