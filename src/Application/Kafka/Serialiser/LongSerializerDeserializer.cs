using Confluent.Kafka;
using System.Buffers.Binary;

namespace Application.Kafka.Serialiser;

public class LongSerializerDeserializer : ISerializer<long>, IDeserializer<long>
{
    public byte[] Serialize(long data, SerializationContext context)
    {
        Span<byte> bytes = stackalloc byte[8];
        BinaryPrimitives.WriteInt64BigEndian(bytes, data);
        return bytes.ToArray();
    }

    public long Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull || data.Length != 8)
            throw new ArgumentException("Invalid data length for long deserialization");

        return BinaryPrimitives.ReadInt64BigEndian(data);
    }
}