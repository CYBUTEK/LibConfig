using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibConfig
{
    public class ConfigSection
    {
        private List<ConfigLine> _lines = new List<ConfigLine>();
        private string _name = "";

        /// <summary>
        /// Gets all of the ConfigLines within the section.
        /// </summary>
        public List<ConfigLine> Lines { get { return _lines; } }

        /// <summary>
        /// Gets the name of the section.
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Gets all of the ConfigSettings within the section.
        /// </summary>
        public List<ConfigSetting> Settings
        {
            get
            {
                List<ConfigSetting> settings = new List<ConfigSetting>();

                foreach (ConfigLine line in _lines)
                {
                    if (line is ConfigSetting)
                    {
                        settings.Add((ConfigSetting)line);
                    }
                }

                return settings;
            }
        }

        /// <summary>
        /// Creates a section without a name.
        /// </summary>
        private ConfigSection() { }

        /// <summary>
        /// Creates a section using the specified name.
        /// </summary>
        /// <param name="name"></param>
        public ConfigSection(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Adds a line into the section.
        /// </summary>
        /// <param name="line"></param>
        public void AddLine(ConfigLine line)
        {
            if (_lines.Count > 0)
            {
                int lastLineIndex = _lines.Count - 1;
                ConfigLine lastLine = _lines[lastLineIndex];

                if (lastLine.Content == "")
                {
                    _lines.Insert(lastLineIndex, line);
                    return;
                }
            }

            _lines.Add(line);
        }

        /// <summary>
        /// Removes a line from the section.
        /// </summary>
        /// <param name="line"></param>
        public void RemoveLine(ConfigLine line)
        {
            _lines.Remove(line);
        }

        /// <summary>
        /// Variable which is represents no section (i.e. top of the page).
        /// </summary>
        public static readonly ConfigSection None = new ConfigSection();

        /// <summary>
        /// Gets an empty section without a name.
        /// </summary>
        public static ConfigSection Empty { get { return new ConfigSection(); } }
    }
}
