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

        public Logger()
        {
            path = "LoggerDebug.txt";
        }

        public void Log(String line)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(this.path, true))
            {
                file.WriteLine(AddInfo(line));
                file.Close();
            }           
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
