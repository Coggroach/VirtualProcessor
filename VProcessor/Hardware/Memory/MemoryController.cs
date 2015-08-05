using System;
using System.Collections.Generic;
using VProcessor.Hardware.Interfacing;
using VProcessor.Hardware.Interrupts;
using VProcessor.Hardware.Peripherals;
using VProcessor.Tools;

namespace VProcessor.Hardware.Memory
{
    public class MemoryController : ITickable
    {
        private MemoryDualChannel channel;
        private Memory32 memory;
        private IList<IPeripheral> mappedMemory;

        private MemoryController() 
            : this(null) {}

        public MemoryController(MemoryDualChannel channel)
        {
            this.memory = new Memory32(VPConsts.RandomAccessMemorySize);
            this.channel = channel;
            this.mappedMemory = new List<IPeripheral>();
        }

        #region Push/Pop
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
        #endregion

        public void RegisterMappedMemory(IPeripheral peripheral)
        {
            this.mappedMemory.Add(peripheral);
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
        private IMemory<UInt32> MemoryChunk(Int32 Address)
        {
            if (Address < VPConsts.RandomAccessMemorySize)
                return this.memory;
            var total = VPConsts.RandomAccessMemorySize;
            foreach(var mapped in this.mappedMemory)
            {
                if (total <= Address && Address < total + mapped.Length)
                    return mapped;
                total += mapped.Length;
            }
            throw new MachineException("MemoryController: Address out of Bounds");
        }

        private UInt32 Read(Int32 address, Int32 offset = 0)
        {
            var netAddress = address + offset;
            return this.MemoryChunk(netAddress).GetMemory(netAddress);
        }

        private void Write(Int32 address, UInt32 value, Int32 offset = 0)
        {
            var netAddress = address + offset;
            this.MemoryChunk(netAddress).SetMemory(address + offset, value);
        }
        #endregion
    }
}
