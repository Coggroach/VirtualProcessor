namespace VProcessor.Hardware.Interfacing
{
    internal interface IConnectable
    {
        void Send(IPacket packet);
        IPacket Receive();
        void Connect(Bus bus);
    }
}
