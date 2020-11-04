using DevIncModule;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace Terminals
{
    public struct Terminal
    {
        public Vector3 position;

        public Storage storage;

        public Error error;

        public Terminal(Vector3 _position, Storage _storage, Error _error)
        {
            position = _position;
            storage = _storage;
            error = _error;
        }

        public bool isReloading
        {
            get
            {
                return error.reloadingTime != 0f;
            }
        }

        public void OpenTerminal(CSteamID steamID)
        {
            if (isReloading)
                return;

            UnturnedPlayer.FromCSteamID(steamID).Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, true);

            if (storage.storedItems.TrueForAll(item => item.amount == 0))
            {
                error = Error.GetRandomError();
                EffectManager.sendUIEffect((ushort)EUIs.DEBUG_TERMINAL, 1000, steamID, true, error.warningMessage);
                return;
            }

            DisplayTerminalItems(steamID);
        }

        public void ReloadTerminal()
        {
            error = new Error();

            storage.storedItems = new List<StoredItem>(storage.baskets == null ? Plugin.Instance.Configuration.Instance.standardGroceryItems : Plugin.Instance.Configuration.Instance.standardOrderingItems);

            if (storage.baskets != null) storage.baskets.Clear();

            Plugin.Instance.Configuration.Save();
        }

        public void DisplayTerminalItems(CSteamID steamID)
        {
            ushort id = storage.baskets != null ? (ushort)EUIs.ORDERING_PAGE : (ushort)EUIs.GROCERY_PAGE;

            EffectManager.sendUIEffect(id, 1000, steamID, true);

            byte index = 0;
            foreach (StoredItem storedItem in storage.storedItems)
            {
                if (DIFinder.GetItemAsset(storedItem.ID, out ItemAsset itemAsset))
                {
                    EffectManager.sendUIEffectText(1000, steamID, true, $"Box_{index}", $"{itemAsset.name.Trim()} \n {storedItem.amount}");
                    index++;
                }
            }
        }
    }
}
