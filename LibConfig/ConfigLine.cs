using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibConfig
{
    public class ConfigLine
    {
        /// <summary>
        /// Variable used to store the writable line content.
        /// </summary>
        protected string _content = "";

        /// <summary>
        /// Gets the writable line content.
        /// </summary>
        public string Content { get { return _content; } }

        /// <summary>
        /// Creates a blank line.
        /// </summary>
        public ConfigLine() { }

        /// <summary>
        /// Creates a line containing some content.
        /// </summary>
        /// <param name="content"></param>
        public ConfigLine(string content)
        {
            _content = content;
        }
    }
}
