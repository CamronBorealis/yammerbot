using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YammerBot.Core.Compliment.Interface
{
    public interface IComplimentTextDeserializer
    {
        List<Entity.Compliments.Compliment> GetComplimentsFromComplimentText(string text);
    }
}
