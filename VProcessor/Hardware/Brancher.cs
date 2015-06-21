using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware
{
    public class Brancher
    {
        private Byte Nzcv { get; set; }       
        public Brancher()
        {
            this.Nzcv = 0;
        }        

        public Boolean Branch(Int32 code)
        {

            return false;
        }
    }
}
