using System.Runtime.Serialization;

namespace Core.Entities
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,

        [EnumMember(Value = "Processing")]
        Processing,

        [EnumMember(Value = "Complete")]
        Complete,

        [EnumMember(Value = "Cancelled")]
        Cancelled
    }
}