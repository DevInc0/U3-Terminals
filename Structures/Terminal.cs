using DevIncModule;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Terminals
{
    public struct Terminal
    {
        public Vector3 position;

        public Dictionary<ulong, List<ushort>> baskets;

        public Dictionary<ushort, byte> items;

        public Error error;

        public bool isReloading;

        public Terminal(Vector3 _position, Dictionary<ulong, List<ushort>> _baskets, Dictionary<ushort, byte> _items, Error _error = new Error(), bool _isReloading = false)
        {
            position = _position;
            baskets = _baskets;
            items = _items;
            error = _error;
            isReloading = _isReloading;
        }

        public void OpenTerminal(CSteamID steamID)
        {
            if (isReloading)
                return;

            UnturnedPlayer.FromCSteamID(steamID).Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, true);

            if (items.All(item => item.Value == 0))
            {
                error.CreateError();
                EffectManager.sendUIEffect((ushort)EUIs.DEBUG_TERMINAL, 1000, steamID, true, error.parameters[0], error.parameters[1], error.parameters[2], error.warningMessage);
                return;
            }

            DisplayTerminalItems(steamID);
        }

        public void ReloadTerminal(Dictionary<ushort, byte> _items)
        {
            items = _items;

            error = new Error();

            isReloading = false;

            Plugin.Instance.Configuration.Save();
        }

        public void DisplayTerminalItems(CSteamID steamID)
        {
            EffectManager.sendUIEffect((ushort)EUIs.ORDERING_PAGE, 1000, steamID, true);

            byte index = 0;
            foreach (KeyValuePair<ushort, byte> item in items)
            {
                if (DIFinder.GetItemAsset(item.Key, out ItemAsset itemAsset))
                {
                    EffectManager.sendUIEffectText(1000, steamID, true, $"Terminal.ItemBox_{index}", $"{itemAsset.name.Trim()} \n {item.Value}");
                    index++;
                }
            }
        }

        public void AddItemToPlayerBasket(byte boxNumber, ulong steamID)
        {
            KeyValuePair<ushort, byte> item = items.ElementAt(boxNumber);

            if (baskets.ContainsKey(steamID))
            {
                baskets[steamID].Add(item.Key);
            }
            else
            {
                baskets.Add(steamID, new List<ushort> { item.Key });
            }
            EffectManager.sendUIEffect((ushort)EUIs.SUCCESS, 1001, (CSteamID)steamID, true);
        }

        public void RemoveItemFromPlayerBasket(byte boxNumber, ulong steamID)
        {
            baskets[steamID].Remove(items.ElementAt(boxNumber).Key);
            EffectManager.sendUIEffect((ushort)EUIs.SUCCESS, 1001, (CSteamID)steamID, true);
        }
    }
}
