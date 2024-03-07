using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WackEditor.GameProject
{
    [DataContract]
    class ProjectVM : ViewModelBase
    {
        public static string Extension { get; } = ".wack";
        [DataMember]
        public string ProjectName { get; private set; }
        [DataMember]
        public string ProjectPath { get; private set; }

        public string FullPath => $"{ProjectPath}{ProjectName}{Extension}";

        [DataMember]
        private ObservableCollection<SceneVM> _scenes = new ObservableCollection<SceneVM>();
        public ReadOnlyObservableCollection<SceneVM> Scenes { get; }

        public ProjectVM(string name, string path)
        {
            ProjectName = name;
            ProjectPath = path;

            _scenes.Add(new SceneVM("DefaultScene", this));

        }


    }
}
