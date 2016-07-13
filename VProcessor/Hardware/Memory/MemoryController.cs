using System.Collections.Generic;
using System.Linq;
using VProcessor.Hardware.Interfacing;
using VProcessor.Hardware.Interrupts;
using VProcessor.Hardware.Peripherals;
using VProcessor.Tools;

namespace VProcessor.Hardware.Memory
{
    public class MemoryController : ITickable
    {
        private readonly MemoryDualChannel _channel;
        private readonly Memory32 _memory;
        private readonly IList<IPeripheral> _mappedMemory;

        private MemoryController() 
            : this(null) {}

        public MemoryController(MemoryDualChannel channel)
        {
            _memory = new Memory32(VpConsts.RandomAccessMemorySize);
            _channel = channel;
            _mappedMemory = new List<IPeripheral>();
        }

        public int Length => _memory.Length + _mappedMemory.Sum(m => m.Length);

        #region Push/Pop
        private void PopInput()
        {
            var packet = _channel.PopInput();
            if (packet != null)
            {
                packet.Value = Read(packet.Address, packet.Offset);
                //Give Packet to Processor
            }
        }

        private void PopOutput()
        {
            var packet = _channel.PopOutput();
            if (packet != null)
            {
                Write(packet.Address, packet.Value, packet.Offset);
                //Delete Packet
            }
        }

        private void PushInput()
        {
            if (_channel.MemoryPullRequest != MemoryDualChannelRequest.Pull) return;
            _channel.PushInput(_channel.PopOutput());
            _channel.MemoryPullRequest = MemoryDualChannelRequest.Complete;
        }
        #endregion

        public void RegisterMappedMemory(IPeripheral peripheral) => _mappedMemory.Add(peripheral);

        public void Tick()
        {
            PushInput();

            if(_channel.Status == Channel.Idle)
                return;
            
            PopInput();
            PopOutput();
        }

        #region Controller        
        private IMemory<uint> MemoryChunk(ref int address)
        {
            address = address % Length;

            if (address < VpConsts.RandomAccessMemorySize)
                return _memory;
            address -= VpConsts.RandomAccessMemorySize;
            foreach(var mapped in _mappedMemory)
            {
                if (0 <= address && address < mapped.Length)
                    return mapped;
                address -= mapped.Length;
            }
            throw new MachineException("MemoryController: Address out of Bounds");
        }

        private uint Read(int address, int offset = 0)
        {
            var netAddress = address + offset;
            return MemoryChunk(ref netAddress).GetMemory(netAddress);
        }

        private void Write(int address, uint value, int offset = 0)
        {
            var netAddress = address + offset;
            MemoryChunk(ref netAddress).SetMemory(netAddress, value);
        }
        #endregion
    }
}
