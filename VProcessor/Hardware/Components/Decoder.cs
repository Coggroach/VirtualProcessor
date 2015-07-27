using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Tools;

namespace VProcessor.Hardware.Components
{
    public class Decoder
    {
        public UInt64 Memory { get; set; }

        public UInt16 Opcode { get; set; }
        public Byte ChannelD { get; set; }
        public Byte ChannelA { get; set; }
        public Byte ChannelB { get; set; }
        public UInt16 Constant { get; set; }

        public Boolean LoadRegister { get; set; }
        public Byte ProgramControl { get; set; }
        public Byte CarControl { get; set; }

        public Boolean MemoryInterrupt { get; set; }
        public Boolean MemoryIn { get; set; }
        public Boolean MemoryOut { get; set; }

        public Byte StoreInstruction { get; set; }
        public Boolean ConstantIn { get; set; }
        public Boolean DataIn { get; set; }
        public Byte BranchUpdate { get; set; }

        public Byte FunctionCode { get; set; }
        public Boolean StorePc { get; set; }
        public Boolean LoadPc { get; set; }

        public Byte Mode { get; set; }
        public UInt16 NextAddress { get; set; }


        public void Decode(Register instruction)
        {
            this.Opcode = (UInt16)BitHelper.BitExtract(instruction.Value, 16, 0xFFFF);
            this.ChannelD = (Byte)(BitHelper.BitMatch(Memory, 2, 1) ? 0xF : BitHelper.BitExtract(instruction.Value, 8, 0xF));
            this.ChannelA = (Byte)(BitHelper.BitMatch(Memory, 1, 1) ? 0xF : BitHelper.BitExtract(instruction.Value, 4, 0xF));
            this.ChannelB = (Byte)(BitHelper.BitMatch(Memory, 0, 1) ? 0xF : BitHelper.BitExtract(instruction.Value, 0, 0xF));
            this.Constant = (UInt16)BitHelper.BitExtract(instruction.Value, 0, 0xF);

            this.LoadRegister   = BitHelper.BitMatch(Memory, 3, 1);
            this.ProgramControl = (Byte)(BitHelper.BitExtract(Memory, 4, 3));
            this.CarControl     = (Byte)(BitHelper.BitExtract(Memory, 6, 3));

            this.MemoryInterrupt= BitHelper.BitMatch(Memory, 8, 1);
            this.MemoryIn       = BitHelper.BitMatch(Memory, 9, 1);
            this.MemoryOut      = BitHelper.BitMatch(Memory, 10, 1);

            this.StoreInstruction   = (Byte)(BitHelper.BitExtract(Memory, 12));
            this.ConstantIn = BitHelper.BitMatch(Memory, 13, 1);
            this.DataIn     = BitHelper.BitMatch(Memory, 14, 1);
            this.BranchUpdate       = (Byte)(BitHelper.BitExtract(Memory, 15));

            this.FunctionCode = (Byte)(BitHelper.BitExtract(Memory, 16, 0x3F));
            this.StorePc    = BitHelper.BitMatch(Memory, 22, 1);
            this.LoadPc     = BitHelper.BitMatch(Memory, 23, 1);

            this.Mode = (Byte)(BitHelper.BitExtract(Memory, 24, 0xF));

            this.NextAddress = (UInt16)BitHelper.BitExtract(Memory, 48, 0xFFFF);
        }

    }
}
