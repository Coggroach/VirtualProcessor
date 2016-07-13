using System;
using System.IO;

namespace VProcessor.Tools
{
    public class Logger
    {
        private static readonly Logger _instance;

        static Logger()
        {
            _instance = new Logger();
        }

        public static Logger Instance()
        {
            return _instance;
        }

        private readonly string _path;
        private bool _mode;        

        public Logger() : this("LoggerDebug.txt", true)
        {
            
        }

        public Logger(string s, bool b)
        {
            _path = s;
            _mode = b;
            if(!_mode)
                File.Delete(_path);
        }

        public void Log(string line)
        {
            using (var file = new StreamWriter(_path, true))
            {
                file.WriteLine(AddInfo(line));
                file.Close();
            }           
        }

        public void Append(bool b) => _mode = b;

        private string AddInfo(string line) => AddType(AddDate(line));

        private string AddType(string line) => "[Logger]" + line;

        private string AddDate(string line) => "[" + DateTime.Now.ToLongTimeString() + "]:" + line;
    }
}
