using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Tools;

namespace VProcessor.Hardware
{
    public class MemoryBus
    {
        private Random rand; 
        private Int32 cycle;
        private MemoryController controller;
        private Processor processor;
        private MemoryConnector connector;

        public MemoryBus() : this(null, null)
        {
            
        }

        public MemoryBus(Processor processor, MemoryController controller)
        {
            this.rand = new Random();
            this.cycle = 0;
            this.controller = controller;
            this.processor = processor;
            this.connector = this.processor.GetMemoryConnector();
        }

        public void SetProcessorReference(Processor processor)
        {
            this.processor = processor;
        }

        public void SetDDRMemoryControllerReference(MemoryController controller)
        {
            this.controller = controller;
        }

        private void IssueCommand()
        {
            this.cycle = this.rand.Next(Settings.RandomAccessMemorySpeed, Settings.RandomAccessMemorySpread + Settings.RandomAccessMemorySpeed);
        }

        private void Fetch()
        {
            if(this.cycle <= 0)
            {
                this.connector.Value = this.controller.Read(this.connector.Address + this.connector.Offset);
                this.connector.Command = MemoryConnector.Received;
            }
        }

        private void Store()
        {
            if (this.cycle <= 0)
            {
                this.controller.Write(this.connector.Address + this.connector.Offset, this.connector.Value);
                this.connector.Command = MemoryConnector.Idle;
            }
        }

        public void Tick()
        {
            if(this.cycle > 0) this.cycle--;
            switch(this.connector.Command)
            {
                case MemoryConnector.Store:
                    this.Store();
                    break;
                case MemoryConnector.Fetch:
                    this.Fetch();
                    break;
            }
        }
    }
}
