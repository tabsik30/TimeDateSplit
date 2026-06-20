using System;
using System.Globalization;
using System.Threading;
using SuchByte.MacroDeck.Plugins;
using SuchByte.MacroDeck.Variables;

namespace tabsik12.TimeDateSplit
{
    public class TimeDateSplitPlugin : IDisposable
    {
        private readonly Main _plugin;
        private System.Threading.Timer? _timer;
        private bool _colonVisible = true;
        private CultureInfo _culture = new("pl-PL");

        public TimeDateSplitPlugin(Main plugin)
        {
            _plugin = plugin;
        }

        public void Enable()
        {
            LoadCultureFromConfig();

            // Nie musimy ręcznie rejestrować zmiennych – SetValue je utworzy,
            // jeśli nie istnieją, zgodnie z aktualnym API Macro Deck.

            _timer = new System.Threading.Timer(UpdateTime!, null, 0, 1000);
        }

        public void LoadCultureFromConfig()
        {
            var lang = PluginConfiguration.GetValue(_plugin, "language") as string;
            if (string.IsNullOrWhiteSpace(lang))
                lang = "pl";

            _culture = lang == "pl"
                ? new CultureInfo("pl-PL")
                : new CultureInfo("en-US");
        }

        private void UpdateTime(object? state)
        {
            try
            {
                var now = DateTime.Now;

                VariableManager.SetValue("date_day", now.Day.ToString("00"), VariableType.String, _plugin);
                VariableManager.SetValue("date_month", now.Month.ToString("00"), VariableType.String, _plugin);
                VariableManager.SetValue("date_year", now.Year.ToString(), VariableType.String, _plugin);

                VariableManager.SetValue("date_month_name", now.ToString("MMMM", _culture), VariableType.String, _plugin);
                VariableManager.SetValue("date_weekday", now.ToString("dddd", _culture), VariableType.String, _plugin);

                VariableManager.SetValue("time_hours", now.Hour.ToString("00"), VariableType.String, _plugin);
                VariableManager.SetValue("time_minutes", now.Minute.ToString("00"), VariableType.String, _plugin);
                VariableManager.SetValue("time_seconds", now.Second.ToString("00"), VariableType.String, _plugin);

                VariableManager.SetValue("time_colon", _colonVisible ? ":" : " ", VariableType.String, _plugin);
                _colonVisible = !_colonVisible;
            }
            catch
            {
                // opcjonalnie logowanie
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
