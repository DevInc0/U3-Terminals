using DevIncModule;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace Terminals
{
    public class Plugin : RocketPlugin<Config>
    {

        public static Plugin Instance;

        protected override void Load()
        {
            DIMessager.WritePluginInfo(this);
            Instance = this;
            Level.onPostLevelLoaded += Event_OnPostLevelLoaded;
            UseableConsumeable.onConsumeRequested += Event_OnConsumeRequested;
        }

        protected override void Unload()
        {
            DIMessager.WritePluginInfo(this);
            Instance = null;
            Level.onPostLevelLoaded -= Event_OnPostLevelLoaded;
            UseableConsumeable.onConsumeRequested -= Event_OnConsumeRequested;
        }

        private void Event_OnConsumeRequested(Player player, ItemConsumeableAsset itemAsset, ref bool shouldAllow)
        {
            // Should delete. Testing thing.
            Rocket.Core.Logging.Logger.Log($"Player name - {player.gameObject.name} or {player.transform.gameObject.name}");

            var unturnedPlayer = UnturnedPlayer.FromPlayer(player);
            Transform objectTransform = DamageTool.raycast(new Ray(player.look.aim.position, player.look.aim.forward), 10f, RayMasks.ROOFS_INTERACT).transform;

            if (itemAsset.id == Configuration.Instance.GroceryTerminalID || itemAsset.id == Configuration.Instance.OrderingTerminalID)
            {
                shouldAllow = false;

                Terminal terminal = Configuration.Instance.terminals.Find(newTerminal => newTerminal.position == objectTransform.position);

                terminal.OpenTerminal(unturnedPlayer.CSteamID);
            }
        }

        private void Event_OnPostLevelLoaded(int level)
        {
            foreach (Transform transform in LevelObjects.models)
            {
                var ID = ushort.Parse(transform.gameObject.name);

                if (ID == Configuration.Instance.GroceryTerminalID || ID == Configuration.Instance.OrderingTerminalID)
                {
                    Storage storage = ID == Configuration.Instance.GroceryTerminalID ? new Storage(new List<StoredItem>(Configuration.Instance.standardGroceryItems)) : new Storage(new List<StoredItem>(Configuration.Instance.standardOrderingItems), new List<PlayerShoppingBasket>());
                    var newTerminal = new Terminal(transform.position, storage, new Error());
                    Configuration.Instance.terminals.Add(newTerminal);
                }
            }

            Configuration.Save();
        }
    }
}
