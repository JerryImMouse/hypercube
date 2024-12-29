using System.Buffers.Binary;

namespace Hypercube.Utilities.Extensions;

public static class BinaryReaderExtension
{
    public static uint ReadUIntBigEndian(this BinaryReader reader)
    {
        return BinaryPrimitives.ReadUInt32BigEndian(reader.ReadBytes(4));
    }
    
    public static int ReadIntBigEndian(this BinaryReader reader)
    {
        return BinaryPrimitives.ReadInt32BigEndian(reader.ReadBytes(4));
    }
}