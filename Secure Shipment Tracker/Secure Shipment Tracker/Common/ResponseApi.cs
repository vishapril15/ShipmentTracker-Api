namespace Secure_Shipment_Tracker.Common
{
    public class ResponseApi<T>
    {
        public string Result { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
