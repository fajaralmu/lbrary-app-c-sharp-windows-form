using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurLibraryApp.Gui.App.Controls
{
    class ComboboxItem
    {
        public ComboboxItem(object Key, object Value)
        {
            this.Key = Key;
            this.Value = Value;
        }
        public object Key { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Key.ToString();
        }
    }
}
