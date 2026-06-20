using SuchByte.MacroDeck.Plugins;

namespace tabsik12.TimeDateSplit
{
    public class Main : MacroDeckPlugin
    {
        private TimeDateSplitPlugin _timeDateSplitPlugin;

        public Main()
        {
            // Dane opisowe bierzemy z ExtensionManifest.json,
            // więc nie nadpisujemy tu Name/Author/itp.
        }

        public override void Enable()
        {
            _timeDateSplitPlugin = new TimeDateSplitPlugin(this);
            _timeDateSplitPlugin.Enable();
        }

        public override void OpenConfigurator()
        {
            using (var configurator = new TimeDateSplitConfigurator(this))
            {
                configurator.ShowDialog();
            }
        }

        internal void ReloadCulture()
        {
            _timeDateSplitPlugin?.LoadCultureFromConfig();
        }
    }
}
