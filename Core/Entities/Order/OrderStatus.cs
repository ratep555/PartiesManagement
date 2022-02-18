using System.Runtime.Serialization;

namespace Core.Entities
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending Payment")]
        PendingPayment,

        [EnumMember(Value = "Received Pament")]
        ReceivedPayment,

        [EnumMember(Value = "Failed Payment")]
        FailedPayment,

        [EnumMember(Value = "Not yet Shipped")]
        NotYetShipped,
        
        [EnumMember(Value = "Shipped")]
        Shipped,

        [EnumMember(Value = "Delivered")]
        Delivered
    }
}



