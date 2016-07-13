using System.IO;

namespace VProcessor.Common
{
    public class VpFile
    {
        public const int Hexadecimal = 0;
        public const int Decimal = 1;
        public const int Assembly = 2;

        public const char Delimiter = ';';

        private string _path;        
        private string _builder;
        private int _mode;
        
        public VpFile(string path, int mode = 0)
        {
            _mode = mode;
            _path = path;            
            Init();
        }

        public int GetMode()
        {
            return _mode;
        }

        public void SetMode(int mode)
        {
            _mode = mode;
        }

        private void Init()
        {
            Load();
        }

        public string GetString()
        {
            return _builder;
        }

        public void CleanUp()
        {
            string s = Delimiter.ToString();
            _builder = _builder.Replace("???", "").Replace("\r\n", s).Replace("\n", s).Replace("\r", s).Replace(s + s, s).Replace("\0", "");
        }

        public void SetString(string s)
        {
            _builder = s;
        }

        public void Load()
        {
            Load(_path);
        }

        public void Save()
        {
            Save(_path);
        }

        public void Save(string path)
        {
            _path = path;
            SetMode();
            using (StreamWriter writer = File.CreateText(path))
            {
                writer.Write(_builder);
            }
        }

        public void Load(string path)
        {
            _path = path;
            SetMode();
            using (StreamReader reader = File.OpenText(path))
            {
                _builder = reader.ReadToEnd();
            }
            CleanUp();
        }

        private void SetMode()
        {
            var extenstion = Path.GetExtension(_path);

            if(extenstion == ".vps")
                _mode = Assembly;
            if (extenstion == ".vpo")
                _mode = Hexadecimal;
        }

    }
}
