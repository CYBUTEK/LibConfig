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
                if (section.Name != "")
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
            ConfigSection configSection = ConfigSection.Empty;

            CleanDocument();

            foreach (string line in lines)
            {
                string lineTrim = line.Trim();

                // Checks if the line is a section identifier.
                if (lineTrim.StartsWith("[") && lineTrim.EndsWith("]"))
                {
                    string sectionName = lineTrim.TrimStart('[').TrimEnd(']');
                    
                    _sections.Add(configSection);

                    configSection = new ConfigSection(sectionName);

                    continue;
                }

                // Checks if the line is a setting.
                if (line.Contains("="))
                {
                    int separatorCharIndex = line.IndexOf('=');

                    string settingName = line.Substring(0, separatorCharIndex);
                    string settingValue = line.Substring(separatorCharIndex + 1, line.Length - separatorCharIndex - 1);
                    configSection.Lines.Add((new ConfigSetting(settingName, settingValue)));

                    continue;
                }

                // Line is not a section identifier or setting.
                configSection.Lines.Add(new ConfigLine(line));
            }

            _sections.Add(configSection);
        }

        /// <summary>
        /// Cleans the document of all lines.
        /// </summary>
        public void CleanDocument()
        {
            _sections.Clear();
            //ConfigSection.None.Lines.Clear();
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
        /// Returns a blank section of the document (i.e. top of the page).
        /// </summary>
        /// <returns></returns>
        public ConfigSection GetSection()
        {
            return GetSection("");
        }

        /// <summary>
        /// Returns the section matching the name given.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public ConfigSection GetSection(string sectionName)
        {
            ConfigSection configSection = ConfigSection.Empty;


            foreach (ConfigSection section in _sections)
            {
                if (section.Name == sectionName)
                {
                    return section;
                }
            }

            return configSection;
        }

        /// <summary>
        /// Returns the value for a setting which is not part of a section.
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public string GetValue(string settingName)
        {
            return GetValue("", settingName);
        }

        /// <summary>
        /// Returns the value for a setting which is part of the specified section. 
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public string GetValue(string sectionName, string settingName)
        {
            ConfigSection configSection = ConfigSection.Empty;

            foreach (ConfigSection section in _sections)
            {
                if (section.Name == sectionName)
                {
                    configSection = section;
                    break;
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
        public void SetValue(string settingName, string settingValue)
        {
            SetValue("", settingName, settingValue);
        }

        /// <summary>
        /// Sets a setting within the specified section.  Creates a new setting and/or section if not present.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="settingName"></param>
        /// <param name="settingValue"></param>
        public void SetValue(string sectionName, string settingName, string settingValue)
        {
            ConfigSection configSection = ConfigSection.Empty;
            bool sectionNotFound = true;

            foreach (ConfigSection section in _sections)
            {
                if (section.Name == sectionName)
                {
                    configSection = section;
                    sectionNotFound = false;
                    break;
                }
            }

            // If section is not present, create it.
            if (sectionNotFound)
            {
                configSection = new ConfigSection(sectionName);
                _sections.Add(configSection);
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
