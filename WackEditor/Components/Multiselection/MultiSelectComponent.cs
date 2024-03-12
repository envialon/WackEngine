using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WackEditor.Components.Multiselection
{
    public interface IMSComponent { }

    public abstract class MultiSelectComponent<T> :ViewModelBase, IMSComponent where T: Component
    {

    }
}


