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
        private FileStream stream;
        private readonly String path;
        private String builder;
        private const Int32 BufferSize = 1024;

        public SFile(String path)
        {
            this.path = path;
            this.stream = new FileStream(this.path, FileMode.OpenOrCreate);
            this.Init();
        }

        private void Init()
        {
            this.Load();
        }

        public String GetString()
        {
            return this.builder;
        }

        public void Load()
        {
            var buffer = new Byte[BufferSize];
            this.stream.Read(buffer, 0, buffer.Length);
            this.builder = Encoding.UTF8.GetString(buffer);
        }

        public void Save()
        {
            var buffer = new Byte[this.builder.Length * sizeof(Char)];
            Buffer.BlockCopy(this.builder.ToCharArray(), 0, buffer, 0, buffer.Length);
            this.stream.Write(buffer, 0, buffer.Length);
        }
    }
}
