using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WackEditor.Components;
using WackEditor.EngineAPIStructs;

namespace WackEditor.EngineAPIStructs
{
    [StructLayout(LayoutKind.Sequential)]
    class TransformDescriptor
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    [StructLayout(LayoutKind.Sequential)]
    class GameEntityDescriptor
    {
       public  TransformDescriptor transform = new TransformDescriptor();
    }
}

namespace WackEditor.DLLWrapper
{
    static class EngineAPI
    {
        private const string _dllName = "EngineDLL.dll";

        [DllImport(_dllName)]
        private static extern int CreateGameEntity(EngineAPIStructs.GameEntityDescriptor descriptor);
        public static  int CreateGameEntity(GameEntity entity) { 
            GameEntityDescriptor descriptor = new GameEntityDescriptor();

            //Transfrom component
            { 
                Transform t = entity.GetComponent<Transform>();
                descriptor.transform.Position = t.Position;
                descriptor.transform.Rotation = t.Rotation;
                descriptor.transform.Scale = t.Scale;

            }
            

            return CreateGameEntity(descriptor);
        }

        [DllImport(_dllName)]
        private static extern void RemoveGameEntity(int id);
        public static  void RemoveGameEntity(GameEntity entity)
        {
            RemoveGameEntity(entity.EntityID);
        }
    }
}
