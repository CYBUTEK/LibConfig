using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibConfig
{
    public class ConfigSetting : ConfigLine
    {
        private string _name = "";
        private string _value = "";

        /// <summary>
        /// Gets the name of the setting.
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Gets and sets the value of the setting.
        /// </summary>
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value.Trim();
                _content = _name + " = " + _value;
            }
        }

        /// <summary>
        /// Creates a new setting using the specified name and value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public ConfigSetting(string name, string value)
        {
            _name = name.Trim();
            _value = value.Trim();
            _content = _name + " = " + _value;
        }

    }
}
