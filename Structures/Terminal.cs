using DevIncModule;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terminals.Structures
{
    public struct Terminal
    {
        public Vector3 position;

        public Error error;

        public Storage storage;

        public bool isReloading
        {
            get
            {
                return error.reloadingTime != 0f; 
            }
        }

        public void OpenTerminal(CSteamID steamID)
        {
            if(isReloading)
                return;

            UnturnedPlayer.FromCSteamID(steamID).Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, true);

            if(storage.storedItems.TrueForAll(item => item.amount == 0))
            {
                error = Error.GetRandomError();
                EffectManager.sendUIEffect((ushort)EUIs.DEBUG_TERMINAL, 1000, steamID, true, error.warningMessage);
                return;
            }

            DisplayTerminalItems(steamID);            
        }

        public void DisplayTerminalItems(CSteamID steamID)
        {
            ushort id = storage.baskets != null ? (ushort)EUIs.ORDERING_PAGE : (ushort)EUIs.GROCERY_PAGE;

            EffectManager.sendUIEffect(id, 1000, steamID, true);

            foreach(StoredItem storedItem in storage.storedItems)
            {
                if(DIFinder.GetItemAsset(storedItem.ID, out ItemAsset itemAsset))
                {
                    EffectManager.sendUIEffectText(1000, steamID, true, itemAsset.name.Trim(), $"{itemAsset.name.Trim()} \n {storedItem.amount}");
                }
            }
        }
    }
}
