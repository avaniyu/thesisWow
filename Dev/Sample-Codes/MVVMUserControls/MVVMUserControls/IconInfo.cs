using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace MVVMUserControls
{
    public class IconInfo
    {
        public string Label { get; set; }
        public string ResourcePath { get; set; }
        public ICommand Command { get; set; }
    }
}
