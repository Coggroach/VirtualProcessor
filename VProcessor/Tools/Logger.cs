using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Tools
{
    public class Logger
    {
        private static Logger instance;

        static Logger()
        {
            instance = new Logger();
        }

        public static Logger Instance()
        {
            return instance;
        }

        private String path;
        private Boolean mode;

        public Logger()
        {
            this.path = "LoggerDebug.txt";
            this.mode = true;
        }

        public Logger(String s)
        {
            this.path = s;
        }

        public void Log(String line)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(this.path, mode))
            {
                file.WriteLine(AddInfo(line));
                file.Close();
            }           
        }

        public void Append(Boolean b)
        {
            this.mode = b;
        }

        private String AddInfo(String line)
        {
            return AddType(AddDate(line));
        }

        private String AddType(String line)
        {
            return "[Logger]" + line;
        }

        private String AddDate(String line)
        {
            return "[" + System.DateTime.Now.ToLongTimeString() + "]:" + line;
        }
    }
}
