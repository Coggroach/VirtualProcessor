using System.IO;
using System.Text.RegularExpressions;
using VProcessor.Common;
using VProcessor.Tools;

namespace VProcessor.Gui
{
    public class UserSettings
    {
        private static readonly UserSettings _defaultSettings;

        static UserSettings()
        {
            _defaultSettings = new UserSettings
            {
                FlashFileLocation = VpConsts.FlashMemoryLocation,
                FileMode = VpFile.Hexadecimal,
                IndentMode = IndentSpace,
                IndentSize = IndentSize1,
                Highlight = true
            };
        }

        public const int IndentSpace = 0;
        public const int IndentTab = 1;
        public const int IndentSize1 = 1;
        public const int IndentSize2 = 2;
        public const int IndentSize4 = 4;        

        public string FlashFileLocation;
        public int FileMode;
        public int IndentMode;
        public int IndentSize;
        public bool Highlight;

        private string[] _lines;

        public UserSettings Load()
        {
            using(StreamReader reader = File.OpenText(VpConsts.UserSettingsLocation))
            {
                _lines = reader.ReadToEnd().Replace("\r\n", "").Split(';');

                FlashFileLocation = GetStringValue("FlashFileLocation", _defaultSettings.FlashFileLocation);
                FileMode = GetInt32Value("FileMode", _defaultSettings.FileMode);
                IndentMode = GetInt32Value("IndentMode", _defaultSettings.IndentMode);
                IndentSize = GetInt32Value("IndentSize", _defaultSettings.IndentSize);
                Highlight = GetBooleanValue("Highlight", _defaultSettings.Highlight);
            }
            return this;
        }
        
        private int GetInt32Value(string match, int defaultValue)
        {
            var unparsed = GetUnparsedValue(match);

            if (unparsed == null)
                return defaultValue;

            return int.Parse(unparsed);
        }

        private string GetStringValue(string match, string defaultValue)
        {
            var unparsed = GetUnparsedValue(match);

            if (unparsed == null)
                return defaultValue;

            return unparsed;
        }

        private bool GetBooleanValue(string match, bool defaultValue)
        {
            var unparsed = GetUnparsedValue(match);

            if (unparsed == null)
                return defaultValue;

            return bool.Parse(unparsed);
        }

        private string GetUnparsedValue(string match)
        {
            var regex = InsertStringIntoRegex(match);
            for (var i = 0; i < _lines.Length; i++)
            {
                if (!string.IsNullOrEmpty(_lines[i]) && Regex.IsMatch(_lines[i].Replace(@"\", @"/"), regex))
                {
                    return Regex.Replace(_lines[i], GetRegex(match), "");
                }
            }
            return null;
        }


        public void Save()
        {
            using (StreamWriter writer = File.CreateText(VpConsts.UserSettingsLocation))
            {
                writer.WriteLine("s:FlashFileLocation=" + FlashFileLocation + ";");
                writer.WriteLine("i:FileMode=" + FileMode + ";");
                writer.WriteLine("i:IndentMode=" + IndentMode + ";");
                writer.WriteLine("i:IndentSize=" + IndentSize + ";");
            }
        }

        private string InsertStringIntoRegex(string s)
        {
            var array = s.ToCharArray();
            var result = @"^[\w][:]";
            for(var i = 0; i < array.Length; i++)
                result += "[" + array[i] + "]";

            result += @"[=][\w\\.;]+";
            return result;
        }

        private string GetRegex(string s)
        {
            return InsertStringIntoRegex(s).Replace(@"[\w\\.;]+", "");
        }
    }
}
