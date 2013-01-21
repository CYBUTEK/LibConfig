using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibConfig
{
    public class ConfigDocument
    {
        private List<ConfigSection> _sections = new List<ConfigSection>();

        /// <summary>
        /// Gets or sets the sections contained within the document.
        /// </summary>
        public List<ConfigSection> Sections { get { return _sections; } }

        /// <summary>
        /// Creates a blank document.
        /// </summary>
        public ConfigDocument() { }

        /// <summary>
        /// Creates a document pre-populated with file data.
        /// </summary>
        /// <param name="lines"></param>
        public ConfigDocument(string[] lines)
        {
            WriteAllLines(lines);
        }

        /// <summary>
        /// Returns all of the lines contained within the document.
        /// </summary>
        /// <returns></returns>
        public string[] ReadAllLines()
        {
            List<string> lines = new List<string>();
            string previousLine = "";

            foreach (ConfigSection section in _sections)
            {
                // Check to see if a section identifier line needs to be added.
                if (section != ConfigSection.None)
                {
                    // Make sure there is a blank line between sections.
                    if (previousLine != "")
                    {
                        lines.Add("");
                    }

                    lines.Add("[" + section.Name + "]");
                    previousLine = ".";
                }

                foreach (ConfigLine line in section.Lines)
                {
                    lines.Add(line.Content);
                    previousLine = line.Content;
                }
            }

            return lines.ToArray();
        }

        /// <summary>
        /// Writes all of the provided lines into the document.  It will clear the document of all existing lines.
        /// </summary>
        /// <param name="lines"></param>
        public void WriteAllLines(string[] lines)
        {
            ConfigSection configSection = ConfigSection.None;

            CleanDocument();

            foreach (string line in lines)
            {
                string lineTrim = line.Trim();

                // Checks if the line is a section identifier.
                if (lineTrim.StartsWith("[") && lineTrim.EndsWith("]"))
                {
                    string sectionName = lineTrim = line.TrimStart('[').TrimEnd(']');
                    
                    _sections.Add(configSection);

                    configSection = new ConfigSection(sectionName);

                    continue;
                }

                // Checks if the line is a setting.
                if (lineTrim.Contains("="))
                {
                    int separatorCharIndex = lineTrim.IndexOf('=');

                    string settingName = lineTrim.Substring(0, separatorCharIndex);
                    string settingValue = lineTrim.Substring(separatorCharIndex + 1, lineTrim.Length - separatorCharIndex - 1);
                    configSection.AddLine(new ConfigSetting(settingName, settingValue));

                    continue;
                }

                // Line is not a section identifier or setting.
                configSection.AddLine(new ConfigLine(line));
            }

            _sections.Add(configSection);
        }

        /// <summary>
        /// Cleans the document of all lines.
        /// </summary>
        public void CleanDocument()
        {
            _sections.Clear();
            ConfigSection.None.Lines.Clear();
        }

        /// <summary>
        /// Checks to see if a section is present within the document.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public bool ContainsSection(string sectionName)
        {
            for (int i = 0; i < _sections.Count; i++)
            {
                if (_sections[i].Name == sectionName)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds a section to the document.
        /// </summary>
        /// <param name="sectionName"></param>
        public void AddSection(string sectionName)
        {
            _sections.Add(new ConfigSection(sectionName));
        }

        /// <summary>
        /// Removes a section from the document.
        /// </summary>
        /// <param name="sectionName"></param>
        public void RemoveSection(string sectionName)
        {
            for (int i = 0; i < _sections.Count; i++)
            {
                if (_sections[i].Name == sectionName)
                {
                    _sections.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// Returns the blank section of the document (i.e. top of the page).
        /// </summary>
        /// <returns></returns>
        public ConfigSection GetSection()
        {
            return ConfigSection.None;
        }

        /// <summary>
        /// Returns the section matching the name given.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public ConfigSection GetSection(string sectionName)
        {
            ConfigSection configSection = ConfigSection.None;

            if (sectionName != "")
            {
                foreach (ConfigSection section in _sections)
                {
                    if (section.Name == sectionName)
                    {
                        return section;
                    }
                }
            }
            return configSection;
        }

        /// <summary>
        /// Returns the value for a setting which is not part of a section.
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public string GetSetting(string settingName)
        {
            return GetSetting("", settingName);
        }

        /// <summary>
        /// Returns the value for a setting which is part of the specified section. 
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public string GetSetting(string sectionName, string settingName)
        {
            ConfigSection configSection = ConfigSection.None;

            // If the section is not blank, try to find it.
            if (sectionName != "")
            {
                foreach (ConfigSection section in _sections)
                {
                    if (section.Name == sectionName)
                    {
                        configSection = section;
                        break;
                    }
                }
            }

            // Try to find the setting within the section. 
            foreach (ConfigLine line in configSection.Lines)
            {
                if ((line is ConfigSetting) && (line as ConfigSetting).Name == settingName)
                {
                    return (line as ConfigSetting).Value;
                }
            }

            // Only reachable if the setting was not found, so return a blank string.
            return "";
        }

        /// <summary>
        /// Sets a setting that is not inside a section.  Creates a new setting if not present.
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="settingValue"></param>
        public void SetSetting(string settingName, string settingValue)
        {
            SetSetting("", settingName, settingValue);
        }

        /// <summary>
        /// Sets a setting within the specified section.  Creates a new setting and/or section if not present.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="settingName"></param>
        /// <param name="settingValue"></param>
        public void SetSetting(string sectionName, string settingName, string settingValue)
        {
            ConfigSection configSection = ConfigSection.None;

            // If the section is not blank, try to find it.
            if (sectionName != "")
            {
                foreach (ConfigSection section in _sections)
                {
                    if (section.Name == sectionName)
                    {
                        configSection = section;
                        break;
                    }
                }

                // If section is not present, create it.
                if (configSection == ConfigSection.None)
                {
                    configSection = new ConfigSection(sectionName);
                    _sections.Add(configSection);
                }
            }

            // Try to find the setting within the section.
            foreach (ConfigLine line in configSection.Lines)
            {
                if ((line is ConfigSetting) && (line as ConfigSetting).Name == settingName)
                {
                    (line as ConfigSetting).Value = settingValue;
                    return;
                }
            }

            // Only reachable if the setting was not found, so create a new one.
            configSection.AddLine(new ConfigSetting(settingName, settingValue));
        }
    }
}
