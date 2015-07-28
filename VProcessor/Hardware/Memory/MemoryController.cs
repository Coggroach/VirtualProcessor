using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Hardware.Components;
using VProcessor.Tools;

namespace VProcessor.Hardware.Memory
{
    public class MemoryController
    {
        private MemoryDualChannel channel;
        private Memory32 memory;

        private MemoryController() 
            : this(null) {}

        public MemoryController(MemoryDualChannel channel)
        {
            this.memory = new Memory32(VPConsts.RandomAccessMemorySize);
            this.channel = channel;   
        }

        private void PopInput()
        {
            var packet = this.channel.PopInput();
            if (packet != null)
            {
                packet.Value = this.Read(packet.Address, packet.Offset);
                //Give Packet to Processor
            }
        }

        private void PopOutput()
        {
            var packet = this.channel.PopOutput();
            if (packet != null)
            {
                this.Write(packet.Address, packet.Value, packet.Offset);
                //Delete Packet
            }
        }

        private void PushInput()
        {
            if (this.channel.MemoryPullRequest == MemoryDualChannelRequest.Pull)
            {
                this.channel.PushInput(this.channel.PopOutput());
                this.channel.MemoryPullRequest = MemoryDualChannelRequest.Complete;
            }
        }

        public void Tick()
        {
            this.PushInput();

            if(this.channel.Status == MemoryChannel.Idle)
                return;
            
            this.PopInput();
            this.PopOutput();
        }

        #region Controller
        private UInt32 Read(Int32 address, Int32 offset = 0)
        {
            return this.memory.GetMemory(address + offset);
        }

        private void Write(Int32 address, UInt32 value, Int32 offset = 0)
        {
            this.memory.SetMemory(address + offset, value);
        }
        #endregion
    }
}
