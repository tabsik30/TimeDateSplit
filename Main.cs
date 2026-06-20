using SuchByte.MacroDeck.Plugins;

namespace tabsik12.TimeDateSplit
{
    public class Plugin : MacroDeckPlugin
    {
        private TimeDateSplitPlugin _timeDateSplitPlugin;

        public override string Name => "Time & Date Split";
        public override string Author => "tabsik12";
        public override string Description => "Rozbija aktualny czas i datę na osobne zmienne (dzień, miesiąc, rok, godzina, minuta, sekunda, nazwy PL/EN, migający dwukropek).";

        public override bool CanConfigure => true;

        public override void Enable()
        {
            _timeDateSplitPlugin = new TimeDateSplitPlugin(this);
            _timeDateSplitPlugin.Enable();
        }

        public override void Disable()
        {
            _timeDateSplitPlugin?.Dispose();
        }

        public override void OpenConfigurator()
        {
            using (var configurator = new TimeDateSplitConfigurator(this))
            {
                configurator.ShowDialog();
            }
        }
    }
}