﻿using Rocket.API;
using System.Collections.Generic;

namespace Terminals
{
    public class Config : IRocketPluginConfiguration
    {

        public ushort GroceryTerminalID, OrderingTerminalID;

        public List<Terminal> terminals;

        public List<Error> errors;

        public List<StoredItem> standardGroceryItems, standardOrderingItems;

        public void LoadDefaults()
        {
            errors = new List<Error>
            {
                new Error("ESRCH \n No such process", "crt /n prc/ *|Main| -res -rel -d [0xPARAMETER]", 300f),
                new Error("EINTR \n Interrupted system call", "gen_ser connect *|Main| -res -rel -d [PARAMETER]", 600f),
                new Error("EIO \n Input/output error", "*|Main| -res -rel -d [0xPARAMETER]", 900f)
            };

            standardGroceryItems = new List<StoredItem>
            {
                new StoredItem { ID = 81, amount = 2 },
                new StoredItem { ID = 15, amount = 3 }
            };

            standardOrderingItems = new List<StoredItem>
            {
                new StoredItem { ID = 253, amount = 4 },
                new StoredItem { ID = 269, amount = 1 }
            };

            GroceryTerminalID = 0;
            OrderingTerminalID = 0;
        }
    }
}
