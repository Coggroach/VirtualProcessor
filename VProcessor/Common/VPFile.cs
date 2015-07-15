using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Software.Assembly
{
    public class VPFile
    {
        public const Int32 Hexadecimal = 0;
        public const Int32 Decimal = 1;
        public const Int32 Assembly = 2;

        public const Char Delimiter = ';';

        private String path;        
        private String builder;
        private Int32 mode;
        
        public VPFile(String path, Int32 mode = 0)
        {
            this.mode = mode;
            this.path = path;            
            this.Init();
        }

        public Int32 GetMode()
        {
            return this.mode;
        }

        public void SetMode(Int32 mode)
        {
            this.mode = mode;
        }

        private void Init()
        {
            this.Load();
        }

        public String GetString()
        {
            return this.builder;
        }

        public void CleanUp()
        {
            String s = Delimiter.ToString();
            this.builder = this.builder.Replace("???", "").Replace("\r\n", s).Replace("\n", s).Replace("\r", s).Replace(s + s, s).Replace("\0", "");
        }

        public void SetString(String s)
        {
            this.builder = s;
        }

        public void Load()
        {
            this.Load(this.path);
        }

        public void Save()
        {
            this.Save(this.path);
        }

        public void Save(String path)
        {
            this.path = path;
            this.SetMode();
            using (StreamWriter writer = File.CreateText(path))
            {
                writer.Write(this.builder);
            }
        }

        public void Load(String path)
        {
            this.path = path;
            this.SetMode();
            using (StreamReader reader = File.OpenText(path))
            {
                this.builder = reader.ReadToEnd();
            }
            this.CleanUp();
        }

        private void SetMode()
        {
            var extenstion = Path.GetExtension(this.path);

            if(extenstion == ".vps")
                this.mode = Assembly;
            if (extenstion == ".vpo")
                this.mode = Hexadecimal;
        }

    }
}
