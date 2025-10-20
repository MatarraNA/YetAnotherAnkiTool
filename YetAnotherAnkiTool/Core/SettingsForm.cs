using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YetAnotherAnkiTool.Core.Config;
using static YetAnotherAnkiTool.Core.Config.Config;

namespace YetAnotherAnkiTool.Core
{
    public partial class SettingsForm : Form
    {
        private static Panel scrollContainer = null!;

        public SettingsForm()
        {
            InitializeComponent();
            WrapConfigPanelInScrollContainer();
            InitializeConfigForm();
        }

        private void WrapConfigPanelInScrollContainer()
        {
            scrollContainer = new Panel
            {
                Location = configPanel.Location,
                Size = configPanel.Size,
                AutoScroll = true,
                BackColor = configPanel.BackColor
            };

            this.Controls.Remove(configPanel);
            configPanel.Location = new Point(0, 0);
            scrollContainer.Controls.Add(configPanel);
            this.Controls.Add(scrollContainer);
            scrollContainer.BringToFront();
        }

        private void InitializeConfigForm()
        {
            var config = Configuration;
            var props = typeof(JsonConfig).GetProperties();
            int y = 10;

            foreach (var prop in props)
            {
                // Label
                var label = new Label
                {
                    Text = prop.Name,
                    Location = new Point(10, y + 3),
                    AutoSize = true,
                    ForeColor = Color.White
                };
                configPanel.Controls.Add(label);

                // TextBox
                var textbox = new TextBox
                {
                    Name = $"txt_{prop.Name}",
                    Text = prop.GetValue(config)?.ToString() ?? "",
                    Location = new Point(200, y),
                    Width = 225
                };
                configPanel.Controls.Add(textbox);

                y += 30;
            }

            configPanel.Height = y + 30;
            configPanel.Width = scrollContainer.ClientSize.Width - 20;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            var config = Configuration;
            var props = typeof(JsonConfig).GetProperties();
            var errors = new List<string>();

            foreach (var prop in props)
            {
                var textbox = configPanel.Controls.Find($"txt_{prop.Name}", true).FirstOrDefault() as TextBox;
                if (textbox == null || !prop.CanWrite) continue;

                string input = textbox.Text.Trim();
                object? parsedValue = null;

                try
                {
                    Type type = prop.PropertyType;

                    if (type == typeof(string))
                        parsedValue = input;
                    else if (type == typeof(int))
                        parsedValue = int.Parse(input);
                    else if (type == typeof(float))
                        parsedValue = float.Parse(input);
                    else if (type == typeof(double))
                        parsedValue = double.Parse(input);
                    else if (type == typeof(bool))
                        parsedValue = bool.Parse(input);
                    else if (type.IsEnum)
                        parsedValue = Enum.Parse(type, input, ignoreCase: true);
                    else
                        errors.Add($"{prop.Name}: Unsupported type {type.Name}");
                }
                catch
                {
                    errors.Add($"{prop.Name}: Invalid value '{input}' for type {prop.PropertyType.Name}");
                }

                if (parsedValue != null)
                    prop.SetValue(config, parsedValue);
            }

            if (errors.Any())
            {
                MessageBox.Show("Validation failed:\n" + string.Join("\n", errors), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveConfig();
            this.Close();
        }

    }
}
