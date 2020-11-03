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

            // TODO: сделать отправку UI эффекта через params
        }               
    }
}
