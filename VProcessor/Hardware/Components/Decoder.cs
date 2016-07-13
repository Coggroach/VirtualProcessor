using VProcessor.Tools;

namespace VProcessor.Hardware.Components
{
    public class Decoder
    {
        public ulong Memory { get; set; }

        public ushort Opcode { get; private set; }
        public byte ChannelD { get; private set; }
        public byte ChannelA { get; private set; }
        public byte ChannelB { get; private set; }
        public ushort Constant { get; private set; }

        public bool LoadRegister { get; private set; }
        public byte ProgramControl { get; private set; }
        public byte CarControl { get; private set; }

        public bool MemoryInterrupt { get; private set; }
        public bool MemoryIn { get; private set; }
        public bool MemoryOut { get; private set; }

        public byte StoreInstruction { get; private set; }
        public bool ConstantIn { get; private set; }
        public bool DataIn { get; private set; }
        public byte BranchUpdate { get; private set; }

        public byte FunctionCode { get; private set; }
        public bool StorePc { get; private set; }
        public bool LoadPc { get; private set; }

        public byte Mode { get; private set; }
        public bool ExecutionMode { get; private set; }

        public bool EndOfInterrupt { get; private set; }
        
        public ushort NextAddress { get; private set; }


        public void Decode(Register instruction)
        {
            Opcode = (ushort)BitHelper.BitExtract(instruction.Value, 16, 0xFFFF);
            ChannelD = (byte)(BitHelper.BitMatch(Memory, 2, 1) ? 0xF : BitHelper.BitExtract(instruction.Value, 8, 0xF));
            ChannelA = (byte)(BitHelper.BitMatch(Memory, 1, 1) ? 0xF : BitHelper.BitExtract(instruction.Value, 4, 0xF));
            ChannelB = (byte)(BitHelper.BitMatch(Memory, 0, 1) ? 0xF : BitHelper.BitExtract(instruction.Value, 0, 0xF));
            Constant = (ushort)BitHelper.BitExtract(instruction.Value, 0, 0xF);

            LoadRegister   = BitHelper.BitMatch(Memory, 3, 1);
            ProgramControl = (byte)(BitHelper.BitExtract(Memory, 4, 3));
            CarControl     = (byte)(BitHelper.BitExtract(Memory, 6, 3));

            MemoryInterrupt= BitHelper.BitMatch(Memory, 8, 1);
            MemoryIn       = BitHelper.BitMatch(Memory, 9, 1);
            MemoryOut      = BitHelper.BitMatch(Memory, 10, 1);

            StoreInstruction   = (byte)(BitHelper.BitExtract(Memory, 12));
            ConstantIn = BitHelper.BitMatch(Memory, 13, 1);
            DataIn     = BitHelper.BitMatch(Memory, 14, 1);
            BranchUpdate       = (byte)(BitHelper.BitExtract(Memory, 15));

            FunctionCode = (byte)(BitHelper.BitExtract(Memory, 16, 0x3F));
            StorePc    = BitHelper.BitMatch(Memory, 22, 1);
            LoadPc     = BitHelper.BitMatch(Memory, 23, 1);

            Mode = (byte)(BitHelper.BitExtract(Memory, 24, 0xF));
            ExecutionMode = BitHelper.BitMatch(Memory, 28, 1);

            EndOfInterrupt = BitHelper.BitMatch(Memory, 29, 1);

            NextAddress = (ushort)BitHelper.BitExtract(Memory, 48, 0xFFFF);
        }

    }
}
