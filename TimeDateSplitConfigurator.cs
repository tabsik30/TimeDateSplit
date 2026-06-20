using System;
using System.Drawing;
using System.Windows.Forms;
using SuchByte.MacroDeck.Plugins;

namespace MaciejKaczmarek.TimeDateSplit
{
    public class TimeDateSplitConfigurator : Form
    {
        private readonly Plugin _plugin;
        private ComboBox _cmbLanguage;

        public TimeDateSplitConfigurator(Plugin plugin)
        {
            _plugin = plugin;
            InitializeComponent();
            LoadConfig();
        }

        private void InitializeComponent()
        {
            Text = "Time & Date Split – konfiguracja";
            Width = 340;
            Height = 190;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            var lbl = new Label
            {
                Text = "Język nazw miesięcy i dni tygodnia:",
                Location = new Point(20, 20),
                AutoSize = true
            };

            _cmbLanguage = new ComboBox
            {
                Location = new Point(20, 50),
                Width = 280,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            _cmbLanguage.Items.Add("Polski (pl-PL)");
            _cmbLanguage.Items.Add("English (en-US)");

            var btnSave = new Button
            {
                Text = "Zapisz",
                Location = new Point(20, 100),
                Width = 100
            };
            btnSave.Click += (_, _) => SaveAndClose();

            var btnCancel = new Button
            {
                Text = "Anuluj",
                Location = new Point(200, 100),
                Width = 100
            };
            btnCancel.Click += (_, _) => Close();

            Controls.Add(lbl);
            Controls.Add(_cmbLanguage);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
        }

        private void LoadConfig()
        {
            var lang = PluginConfiguration.GetValue(_plugin, "language") as string;
            if (lang == "en")
            {
                _cmbLanguage.SelectedIndex = 1;
            }
            else
            {
                _cmbLanguage.SelectedIndex = 0;
            }
        }

        private void SaveAndClose()
        {
            var langCode = _cmbLanguage.SelectedIndex == 1 ? "en" : "pl";
            PluginConfiguration.SetValue(_plugin, "language", langCode);

            if (_plugin is Plugin concrete)
            {
                // Odśwież kulturę w działającym pluginie
                concrete?.GetType()
                    .GetField("_timeDateSplitPlugin", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
                    .GetValue(concrete) is TimeDateSplitPlugin logicInstance
                    ?.LoadCultureFromConfig();
            }

            Close();
        }
    }
}