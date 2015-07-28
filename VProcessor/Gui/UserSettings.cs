using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VProcessor.Software.Assembly;
using VProcessor.Tools;
using VProcessor.Common;

namespace VProcessor.Gui
{
    public class UserSettings
    {
        private static UserSettings defaultSettings;

        static UserSettings()
        {
            defaultSettings = new UserSettings()
            {
                FlashFileLocation = VPConsts.FlashMemoryLocation,
                FileMode = VPFile.Hexadecimal,
                IndentMode = IndentSpace,
                IndentSize = IndentSize1,
                Highlight = true
            };
        }

        public const Int32 IndentSpace = 0;
        public const Int32 IndentTab = 1;
        public const Int32 IndentSize1 = 1;
        public const Int32 IndentSize2 = 2;
        public const Int32 IndentSize4 = 4;        

        public String FlashFileLocation;
        public Int32 FileMode;
        public Int32 IndentMode;
        public Int32 IndentSize;
        public Boolean Highlight;

        private String[] lines;        

        public UserSettings()
        {
            
        }

        public UserSettings Load()
        {
            using(StreamReader reader = File.OpenText(VPConsts.UserSettingsLocation))
            {
                this.lines = reader.ReadToEnd().Replace("\r\n", "").Split(';');

                this.FlashFileLocation = this.GetStringValue("FlashFileLocation", defaultSettings.FlashFileLocation);
                this.FileMode = this.GetInt32Value("FileMode", defaultSettings.FileMode);
                this.IndentMode = this.GetInt32Value("IndentMode", defaultSettings.IndentMode);
                this.IndentSize = this.GetInt32Value("IndentSize", defaultSettings.IndentSize);
                this.Highlight = this.GetBooleanValue("Highlight", defaultSettings.Highlight);
            }
            return this;
        }
        
        private Int32 GetInt32Value(String match, Int32 defaultValue)
        {
            var unparsed = this.GetUnparsedValue(match);

            if (unparsed == null)
                return defaultValue;

            return Int32.Parse(unparsed);
        }

        private String GetStringValue(String match, String defaultValue)
        {
            var unparsed = this.GetUnparsedValue(match);

            if (unparsed == null)
                return defaultValue;

            return unparsed;
        }

        private Boolean GetBooleanValue(String match, Boolean defaultValue)
        {
            var unparsed = this.GetUnparsedValue(match);

            if (unparsed == null)
                return defaultValue;

            return Boolean.Parse(unparsed);
        }

        private String GetUnparsedValue(String match)
        {
            var regex = this.InsertStringIntoRegex(match);
            for (var i = 0; i < this.lines.Length; i++)
            {
                if (!String.IsNullOrEmpty(this.lines[i]) && Regex.IsMatch(this.lines[i].Replace(@"\", @"/"), regex))
                {
                    return Regex.Replace(this.lines[i], this.GetRegex(match), "");
                }
            }
            return null;
        }


        public void Save()
        {
            using (StreamWriter writer = File.CreateText(VPConsts.UserSettingsLocation))
            {
                writer.WriteLine("s:FlashFileLocation=" + this.FlashFileLocation + ";");
                writer.WriteLine("i:FileMode=" + this.FileMode + ";");
                writer.WriteLine("i:IndentMode=" + this.IndentMode + ";");
                writer.WriteLine("i:IndentSize=" + this.IndentSize + ";");
            }
        }

        private String InsertStringIntoRegex(String s)
        {
            var array = s.ToCharArray();
            var result = @"^[\w][:]";
            for(var i = 0; i < array.Length; i++)
                result += "[" + array[i] + "]";

            result += @"[=][\w\\.;]+";
            return result;
        }

        private String GetRegex(String s)
        {
            return this.InsertStringIntoRegex(s).Replace(@"[\w\\.;]+", "");
        }
    }
}
