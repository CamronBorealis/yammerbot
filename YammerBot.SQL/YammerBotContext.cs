using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YammerBot.Entity.Yammer;

namespace YammerBot.SQL
{
    public class YammerBotContext:DbContext
    {
        public DbSet<Message> Messages { get; set; }
    }
}
