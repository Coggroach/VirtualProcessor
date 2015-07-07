using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Software.Assembly
{
    public class SFile
    {
        public const Int32 Hexadecimal = 0;
        public const Int32 Decimal = 1;
        public const Int32 Assembly = 2;

        private const Int32 BufferSize = 1024;
        private readonly String path;

        private FileStream stream;        
        private String builder;
        private Int32 mode;

        public SFile(String path, Int32 mode = 0)
        {
            this.mode = mode;
            this.path = path;
            this.stream = new FileStream(this.path, FileMode.OpenOrCreate);
            this.Init();
        }

        public Int32 GetMode()
        {
            return this.mode;
        }

        private void Init()
        {
            this.Load();
        }

        public String GetString()
        {
            return this.builder;
        }

        public void SetString(String s)
        {
            this.builder = s;
        }

        public void Load()
        {
            var buffer = new Byte[BufferSize];
            this.stream.Position = 0;
            this.stream.Read(buffer, 0, buffer.Length);
            this.builder = Encoding.ASCII.GetString(buffer).Replace("\0", "");
        }

        public void Save()
        {
            var buffer = new Byte[this.builder.Length * sizeof(Char)];
            Buffer.BlockCopy(this.builder.ToCharArray(), 0, buffer, 0, buffer.Length);
            this.stream.Position = 0;
            this.stream.Write(buffer, 0, buffer.Length);
        }
                
        public void Close()
        {
            this.stream.Close();
        }
    }
}
