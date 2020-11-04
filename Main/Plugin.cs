using DevIncModule;
using Rocket.Core.Plugins;
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
            DIMessager.PluginInfoStatus(this);
            Instance = this;
            Level.onPostLevelLoaded += Event_OnPostLevelLoaded;
        }

        protected override void Unload()
        {
            DIMessager.PluginInfoStatus(this);
            Instance = null;
            Level.onPostLevelLoaded -= Event_OnPostLevelLoaded;
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
