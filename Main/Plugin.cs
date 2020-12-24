using DevIncModule;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections;
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
            EffectManager.onEffectButtonClicked += Event_OnButtonClicked;
            EffectManager.onEffectTextCommitted += Event_OnTextCommitted;
        }

        protected override void Unload()
        {
            ReloadAllTerminals();
            DIMessager.WritePluginInfo(this);
            Instance = null;
            Level.onPostLevelLoaded -= Event_OnPostLevelLoaded;
            UseableConsumeable.onConsumeRequested -= Event_OnConsumeRequested;
            EffectManager.onEffectButtonClicked -= Event_OnButtonClicked;
            EffectManager.onEffectTextCommitted -= Event_OnTextCommitted;
        }

        private void Event_OnPostLevelLoaded(int level)
        {
            foreach (Transform transform in Level.level)
            {
                // Should delete. Testing thing.
                Rocket.Core.Logging.Logger.Log($"Transform name - {transform.gameObject.name}");
                //

                var ID = ushort.Parse(transform.gameObject.name);

                if (ID == Configuration.Instance.GroceryTerminalID || ID == Configuration.Instance.OrderingTerminalID)
                {
                    Storage storage;
                    if (ID == Configuration.Instance.GroceryTerminalID)
                        storage = new Storage(new List<StoredItem>(Configuration.Instance.groceryItems), new Dictionary<ulong, List<ushort>>());
                    else
                        storage = new Storage(new List<StoredItem>(Configuration.Instance.orderingItems), new Dictionary<ulong, List<ushort>>());
                    var newTerminal = new Terminal(transform.position, storage, new Error());
                    Configuration.Instance.terminals.Add(newTerminal);
                }
            }

            Configuration.Save();
        }

        private void Event_OnConsumeRequested(Player player, ItemConsumeableAsset itemAsset, ref bool shouldAllow)
        {
            // Should delete. Testing thing.
            Rocket.Core.Logging.Logger.Log($"Player name - {player.gameObject.name} or {player.transform.gameObject.name}");
            //
            var unturnedPlayer = UnturnedPlayer.FromPlayer(player);
            Transform objectTransform = DamageTool.raycast(new Ray(player.look.aim.position, player.look.aim.forward), 10f, RayMasks.ROOFS_INTERACT).transform;

            if (itemAsset.id == Configuration.Instance.GroceryTerminalID || itemAsset.id == Configuration.Instance.OrderingTerminalID)
            {
                shouldAllow = false;
                Terminal terminal = Configuration.Instance.terminals.Find(newTerminal => newTerminal.position == objectTransform.position);
                terminal.OpenTerminal(unturnedPlayer.CSteamID);
            }
        }

        private void Event_OnButtonClicked(Player player, string buttonName)
        {
            var unturnedPlayer = UnturnedPlayer.FromPlayer(player);
            Transform objectTransform = DamageTool.raycast(new Ray(player.look.aim.position, player.look.aim.forward), 10f, RayMasks.ROOFS_INTERACT).transform;
            Terminal currentTerminal = Configuration.Instance.terminals.Find(newTerminal => newTerminal.position == objectTransform.position);

            if (buttonName == "Terminal.CloseTerminal")
            {
                foreach (ushort id in Enum.GetValues(typeof(EUIs)))
                    EffectManager.askEffectClearByID(id, unturnedPlayer.CSteamID);
            }
            else if (buttonName.Contains("Terminal.AddButton"))
            {
                byte boxNumber = byte.Parse(buttonName.Trim().Substring(buttonName.Length - 1));
                if (currentTerminal.storage.items.Count <= boxNumber)
                {

                }
            }
        }

        private void Event_OnTextCommitted(Player player, string inputFieldName, string text)
        {
            var unturnedPlayer = UnturnedPlayer.FromPlayer(player);
            Transform objectTransform = DamageTool.raycast(new Ray(player.look.aim.position, player.look.aim.forward), 10f, RayMasks.ROOFS_INTERACT).transform;
            var terminal = Configuration.Instance.terminals.Find(newTerminal => newTerminal.position == objectTransform.position);

            if (inputFieldName == "DebugTerminal.CommandLine")
            {
                if (terminal.error.FixError(text))
                {
                    terminal.isReloading = true;
                    StartCoroutine(StartTerminalReloading(terminal));
                }
            }
        }

        IEnumerator StartTerminalReloading(Terminal terminal)
        {
            yield return new WaitForSeconds(terminal.error.reloadingTime);
            terminal.ReloadTerminal();
        }

        private void ReloadAllTerminals()
        {
            foreach (Terminal terminal in Configuration.Instance.terminals)
            {
                if (terminal.isReloading)
                {
                    StopCoroutine(StartTerminalReloading(terminal));
                    terminal.ReloadTerminal();
                }
            }
            Configuration.Save();
        }
    }
}
