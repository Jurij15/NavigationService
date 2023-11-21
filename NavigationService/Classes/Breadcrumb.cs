using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationService
{
    //thanks to https://github.com/microsoft/devhome/blob/main/settings/DevHome.Settings/Models/Breadcrumb.cs#L10
    public class Breadcrumb
    {
        public Breadcrumb(string label, Type page)
        {
            Label = label;
            Page = page;
        }
        public string Label { get; }

        public Type Page { get; }

        public override string ToString() => Label;
    }
}
