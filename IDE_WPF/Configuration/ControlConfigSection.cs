using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;

namespace IDE_WPF.Configuration
{
    public class ControlConfigSection : ConfigurationSection
    {
        public static readonly ControlConfigSection Current =(ControlConfigSection)ConfigurationManager.GetSection("controls");

        [ConfigurationProperty("StandardHandlingFee", DefaultValue = "4.95")]
        public decimal StandardHandlingFee
        {
            get { return (decimal)base["StandardHandlingFee"]; }
            set { base["StandardHandlingFee"] = value; }
        }

        [ConfigurationProperty("PageSize", DefaultValue = "10")]
        public int PageSize
        {
            get { return (int)base["PageSize"]; }
            set { base["PageSize"] = value; }
        }
    }


    public class KeyBindingCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            throw new NotImplementedException();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    ///    
    /// </summary>
    public class KeyBindingElement : ConfigurationElement
    {
        [ConfigurationProperty("action",IsRequired = true)]
        public Action action { get; set; }

        [ConfigurationProperty("modifier", DefaultValue = ModifierKeys.None, IsRequired = false)]
        public ModifierKeys modifier { get; set; }

        [ConfigurationProperty("key",IsRequired = true)]
        public Key key { get; set; }
    }
}
