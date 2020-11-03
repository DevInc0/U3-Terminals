using Rocket.Core.Plugins;
using DevIncModule;

namespace Terminals
{
    public class Plugin : RocketPlugin<Config>
    {

        public static Plugin Instance;

        protected override void Load()
        {
            DIMessager.PluginInfoStatus(this);
            Instance = this;
        }

        protected override void Unload()
        {
            DIMessager.PluginInfoStatus(this);
            Instance = null;
        }
    }
}
