using System;
using System.Globalization;
using System.Threading;
using SuchByte.MacroDeck.Plugins;
using SuchByte.MacroDeck.Variables;

namespace MaciejKaczmarek.TimeDateSplit
{
    public class TimeDateSplitPlugin : IDisposable
    {
        private readonly Plugin _plugin;
        private Timer _timer;
        private bool _colonVisible = true;
        private CultureInfo _culture;

        public TimeDateSplitPlugin(Plugin plugin)
        {
            _plugin = plugin;
        }

        public void Enable()
        {
            LoadCultureFromConfig();

            // Definicja zmiennych
            VariableManager.Register(new Variable("date_day", "Dzień", VariableType.String, _plugin));
            VariableManager.Register(new Variable("date_month", "Miesiąc (numer)", VariableType.String, _plugin));
            VariableManager.Register(new Variable("date_year", "Rok", VariableType.String, _plugin));
            VariableManager.Register(new Variable("date_month_name", "Nazwa miesiąca", VariableType.String, _plugin));
            VariableManager.Register(new Variable("date_weekday", "Dzień tygodnia", VariableType.String, _plugin));

            VariableManager.Register(new Variable("time_hours", "Godzina", VariableType.String, _plugin));
            VariableManager.Register(new Variable("time_minutes", "Minuta", VariableType.String, _plugin));
            VariableManager.Register(new Variable("time_seconds", "Sekunda", VariableType.String, _plugin));
            VariableManager.Register(new Variable("time_colon", "Dwukropek migający", VariableType.String, _plugin));

            _timer = new Timer(UpdateTime, null, 0, 1000);
        }

        public void LoadCultureFromConfig()
        {
            var lang = PluginConfiguration.GetValue(_plugin, "language") as string;
            if (string.IsNullOrWhiteSpace(lang))
            {
                lang = "pl"; // domyślnie polski
            }

            _culture = lang == "pl"
                ? new CultureInfo("pl-PL")
                : new CultureInfo("en-US");
        }

        private void UpdateTime(object state)
        {
            try
            {
                var now = DateTime.Now;

                VariableManager.SetValue("date_day", now.Day.ToString("00"), _plugin);
                VariableManager.SetValue("date_month", now.Month.ToString("00"), _plugin);
                VariableManager.SetValue("date_year", now.Year.ToString(), _plugin);

                VariableManager.SetValue("date_month_name", now.ToString("MMMM", _culture), _plugin);
                VariableManager.SetValue("date_weekday", now.ToString("dddd", _culture), _plugin);

                VariableManager.SetValue("time_hours", now.Hour.ToString("00"), _plugin);
                VariableManager.SetValue("time_minutes", now.Minute.ToString("00"), _plugin);
                VariableManager.SetValue("time_seconds", now.Second.ToString("00"), _plugin);

                VariableManager.SetValue("time_colon", _colonVisible ? ":" : " ", _plugin);
                _colonVisible = !_colonVisible;
            }
            catch
            {
                // ewentualnie logowanie, żeby nie uwalić timera
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}