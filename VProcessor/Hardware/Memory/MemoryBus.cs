using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Hardware.Components;
using VProcessor.Tools;

namespace VProcessor.Hardware.Memory
{
    public class MemoryBus
    {
        private Random rand; 
        private Int32 inCycle;
        private Int32 outCycle;
        private MemoryController controller;
        private Processor processor;
        private MemoryDualChannel channel;

        public MemoryBus() : this(null, null)
        {
            
        }

        public MemoryBus(Processor processor, MemoryController controller)
        {
            this.rand = new Random();
            this.inCycle = 0;
            this.outCycle = 0;
            this.controller = controller;
            this.processor = processor;
            this.channel = this.processor.GetMemoryDualChannel();            
        }

        public void SetProcessorReference(Processor processor)
        {
            this.processor = processor;
        }

        public void SetDDRMemoryControllerReference(MemoryController controller)
        {
            this.controller = controller;
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
                    packet.Value = this.controller.Read(packet.Address, packet.Offset);
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
                    this.controller.Write(packet.Address, packet.Value, packet.Offset);
                    //Delete Packet
                }
            }
        }

        private void PushInput()
        {
            if (this.processor.MemoryPullRequest == Processor.Pull)
            {
                this.channel.PushInput(this.processor.ChannelPacket);
                this.processor.MemoryPullRequest = Processor.Complete;
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
    }
}
