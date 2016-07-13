using System.Collections.Generic;
using VProcessor.Hardware.Components;
using VProcessor.Hardware.Interfacing;
using VProcessor.Hardware.Peripherals;
using VProcessor.Tools;

namespace VProcessor.Hardware.Interrupts
{
    public class InterruptController : IPeripheral
    {
        private readonly Register _irr;
        private readonly Register _isr;
        private readonly Register _imr;

        private readonly Channel _interruptChannel;
        private readonly IList<IPeripheral> _peripherals;

        private readonly uint[] _addresses;

        public InterruptController(Channel interrupt)
        {
            _irr = new Register();
            _isr = new Register();
            _imr = new Register();
            _interruptChannel = interrupt;
            _peripherals = new List<IPeripheral>();
            _addresses = new uint[VpConsts.VectoredInterruptControllerSize];
            _imr.Value = ~(uint)0;
        }

        public void Request(uint irq)
        {
            if ((irq & _imr.Value) != irq)
                return;

            _irr.Value |= irq;
        }

        private void Service(byte bitPos) => _isr.ClrBit(bitPos);

        private byte RetrieveNextRequest()
        {
            byte i = 0;
            var value = _irr.Value;

            if (value == 0) return VpConsts.VectoredInterruptControllerAddress + 1;

            while(value > 1)
            {
                value >>= 1;
                i++;
            }
            return i;
        }

        private void Mask() => _imr.Value = ~(_irr.Value | _isr.Value);

        public InterruptPacket CreateInterruptRequest()
        {
            var bitPos = RetrieveNextRequest();

            if (bitPos > VpConsts.VectoredInterruptControllerAddress)
                return null;

            _irr.ClrBit(bitPos);
            _isr.SetBit(bitPos);
            Mask();

            return new InterruptPacket
            {
                Address = _addresses[bitPos],
                Request = InterruptPacketRequest.Irq
            };
        }

        public void RegisterPeripheral(IPeripheral peripheral) => _peripherals.Add(peripheral);

        private uint RequestInput()
        {
            var request = (uint) 0;
            for(var i = 0; i < _peripherals.Count; i++)
            {
                if (!_peripherals[i].Trigger()) continue;
                request |= (uint)(1 << i);
                _imr.SetBit((byte)i);
            }
            return request;
        }
        public void Tick()
        {
            Request(RequestInput());
            foreach (var peri in _peripherals)
                peri.Tick();                            
            _interruptChannel.Push(CreateInterruptRequest());        
        }

        public bool Trigger()
        {
            return false;
        }

        public void Reset()
        {

        }

        public uint GetMemory(int index)
        {
            return GetMemory((uint)index);
        }

        public uint GetMemory(uint index)
        {
            return _addresses[index];
        }

        public void SetMemory(int index, uint value)
        {
            _addresses[index] = value;
        }

        public int Length
        {
            get { return VpConsts.VectoredInterruptControllerSize; }
        }

        public bool HasMemory
        {
            get { return true; }
        }
    }
}
