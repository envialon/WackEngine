using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WackEditor.Utilities
{

    public interface IUndoRedo
    {
        string Name { get; }
        void Undo();
        void Redo();
    }

    /// <summary>
    /// Class used by the UndoRedo system to keep track of the executed
    /// commands by the editor, and how to redo/undo them.
    /// </summary>
    public class UndoRedoAction : IUndoRedo
    {
        private Action _undoAction;
        private Action _redoAction;

        public string Name { get; set; }
        public void Undo() => _undoAction();
        public void Redo() => _redoAction();

        public UndoRedoAction(string name)
        {
            Name = name;
        }

        public UndoRedoAction(string name, Action undoAction, Action redoAction) : this(name)
        {

            Debug.Assert(undoAction != null && redoAction != null);
            _undoAction = undoAction;
            _redoAction = redoAction;
        }
    }

    /// <summary>
    /// Class that implmeents the Undo/Redo system.
    /// Not a singleton because different windows can have
    /// different Undo/Redo lists.
    /// </summary>
    public class UndoRedo
    {
        private readonly ObservableCollection<IUndoRedo> _undoList = new ObservableCollection<IUndoRedo>();
        private readonly ObservableCollection<IUndoRedo> _redoList = new ObservableCollection<IUndoRedo>();

        public ReadOnlyObservableCollection<IUndoRedo> UndoList { get; }
        public ReadOnlyObservableCollection<IUndoRedo> RedoList { get; }

        private void Reset()
        {
            _undoList.Clear();
            _redoList.Clear();
        }

        public void Add(IUndoRedo action)
        {
            _undoList.Add(action);
            _redoList.Clear();
        }

        public void Undo()
        {
            if (_undoList.Any())
            {
                IUndoRedo action = _undoList.Last();
                _undoList.RemoveAt(_undoList.Count - 1);
                action.Undo();
                _redoList.Insert(0, action);
            }
        }

        public void Redo()
        {
            if (_redoList.Any())
            {
                IUndoRedo action = _redoList.First();
                _redoList.RemoveAt(0);
                action.Redo();
                _undoList.Add(action);
            }
        }


        public UndoRedo()
        {
            UndoList = new ReadOnlyObservableCollection<IUndoRedo>(_undoList);
            RedoList = new ReadOnlyObservableCollection<IUndoRedo>(_redoList);
        }
    }
}
