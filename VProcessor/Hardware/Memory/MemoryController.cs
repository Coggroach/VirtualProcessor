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
        private Random rand; 
        private Int32 inCycle;
        private Int32 outCycle;
        private MemoryDualChannel channel;
        private Memory32 memory;

        private MemoryController() 
            : this(null) {}

        public MemoryController(MemoryDualChannel channel)
        {
            this.rand = new Random();
            this.inCycle = 0;
            this.outCycle = 0;
            this.memory = new Memory32(Settings.RandomAccessMemorySize);
            this.channel = channel;   
        }

        private void GenerateInputCycleDelay()
        {
            this.inCycle = this.rand.Next(Settings.RandomAccessMemorySpeed, Settings.RandomAccessMemorySpread + Settings.RandomAccessMemorySpeed);
        }       

        private void GenerateOutputCycleDelay()
        {
            this.outCycle = this.rand.Next(Settings.RandomAccessMemorySpeed, Settings.RandomAccessMemorySpread + Settings.RandomAccessMemorySpeed);
        }

        private void PopInput()
        {
            if (this.inCycle == 0)
            {
                var packet = this.channel.PopInput();
                if (packet != null)
                {
                    packet.Value = this.Read(packet.Address, packet.Offset);
                    //Give Packet to Processor
                }
            }
        }

        private void PopOutput()
        {
            if (this.outCycle == 0)
            {
                var packet = this.channel.PopOutput();
                if (packet != null)
                {
                    this.Write(packet.Address, packet.Value, packet.Offset);
                    //Delete Packet
                }
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

            //this.inCycle--;
            //this.outCycle--;
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
