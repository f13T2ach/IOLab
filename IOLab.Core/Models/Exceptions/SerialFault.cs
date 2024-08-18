namespace IOLab.Core.Models.Exceptions
{
    class SerialFault : Exception
    {
        public SerialFault() { }

        public SerialFault(string message)
            : base(message)
        {
            throw new Exception(message);
        }

        public SerialFault(string message, Exception innerException)
            : base(message, innerException)
        {

        }

    }
}
