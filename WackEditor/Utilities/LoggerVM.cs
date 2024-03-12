using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace WackEditor.Utilities
{
    public enum MessageTypes
    {
        Info = 0x01,
        Warning = 0x02,
        Error = 0x04
    }

    public class LogMessage
    {
        public DateTime Time { get; }
        public MessageTypes MessageType { get; }
        public string Message { get; }
        public string File { get; }
        public string Caller { get; }
        public int Line { get; }

        public string MetaData => $"{File}: {Caller} ({Line})";
        public LogMessage(MessageTypes type, string msg, string file, string caller, int line)
        {
            Time = DateTime.Now;
            MessageType = type;
            Message = msg;
            File = Path.GetFileName(file);
            Caller = caller;
            Line = line;
        }
    }

    static class LoggerVM
    {
        private static int _messageFilter = (int)(MessageTypes.Info | MessageTypes.Warning | MessageTypes.Error);

        private static ObservableCollection<LogMessage> _messages = new ObservableCollection<LogMessage>();
        public static ReadOnlyObservableCollection<LogMessage> Messages { get; } = new ReadOnlyObservableCollection<LogMessage>(_messages);
        public static CollectionViewSource FilteredMessages { get; } = new CollectionViewSource() { Source = Messages };

        public static async void Log(MessageTypes type, string msg, [CallerFilePath] string file = "", [CallerFilePath] string caller = "", int line = 0)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                _messages.Add(new LogMessage(type, msg, file, caller, line));
            }));
        }

        public static async void Clear()
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                _messages.Clear();
            }));
        }

        public static void SetMessageFilter(int mask)
        {
            _messageFilter = mask;
            FilteredMessages.View.Refresh();
        }

        static LoggerVM()
        {
            //Set the way the messages need to be filtered when the Refresh method is called
            //(By checking the current filter mask)
            FilteredMessages.Filter += (s, e) =>
            {
                var type = (int)(e.Item as LogMessage).MessageType;
                e.Accepted = (type & _messageFilter) != 0;
            };
        }
    }
}
