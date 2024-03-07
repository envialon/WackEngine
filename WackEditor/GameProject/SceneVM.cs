using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WackEditor.GameProject
{
    [DataContract]
    class SceneVM : ViewModelBase
    {

        private string _sceneName;
        [DataMember]
        public string Name
        {
            get { return _sceneName; }
            set
            {
                if (value != _sceneName)
                {
                    _sceneName = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        [DataMember]
        public ProjectVM Project { get; private set; }

        public SceneVM(string name, ProjectVM project)
        {
            Debug.Assert(project != null);
            Project = project;
            Name = name;
        }


    }
}
