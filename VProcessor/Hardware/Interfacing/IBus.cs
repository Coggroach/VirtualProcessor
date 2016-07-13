namespace VProcessor.Hardware.Interfacing
{
    internal abstract class Bus
    {
        protected IConnectable Connect1;
        protected IConnectable Connect2;

        public void Connect(IConnectable con1, IConnectable con2)
        {
            Connect1 = con1;
            Connect2 = con2;

            Connect1.Connect(this);
            Connect2.Connect(this);
        }        
    }
}
