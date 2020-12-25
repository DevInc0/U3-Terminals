using Rocket.API;
using System.Collections.Generic;

namespace Terminals
{
    public class Config : IRocketPluginConfiguration
    {

        public ushort groceryTerminalID, orderingTerminalID;

        public List<Terminal> terminals;

        public List<Error> errors;

        public Dictionary<ushort, byte> groceryItems, orderingItems;

        public void LoadDefaults()
        {
            groceryTerminalID = 0;
            orderingTerminalID = 0;

            terminals = new List<Terminal>();

            errors = new List<Error>
            {
                new Error("ESRCH \n No such process", "crt /n prc/ *|Main| -res -rel -d [0xPARAMETER]", 300f),
                new Error("EINTR \n Interrupted system call", "gen_ser connect *|Main| -res -rel -d [PARAMETER]", 600f),
                new Error("EIO \n Input/output error", "*|Main| -res -rel -d [0xPARAMETER]", 900f)
            };

            groceryItems = new Dictionary<ushort, byte>
            {
                { 81, 2 },
                { 15, 3 } 
            };

            orderingItems = new Dictionary<ushort, byte>
            {
                { 253, 4 },
                { 269, 1 }
            };
        }
    }
}
