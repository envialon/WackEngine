namespace WackEditor.Components.Multiselection
{
    public interface IMSComponent { }

    public abstract class MultiSelectComponent<T> : ViewModelBase, IMSComponent where T : Component
    {

    }
}


