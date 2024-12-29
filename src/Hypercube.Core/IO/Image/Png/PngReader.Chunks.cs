using System.Buffers.Binary;
using System.IO.Compression;
using System.Text;
using Hypercube.Utilities.Extensions;

namespace Hypercube.Core.IO.Image.Png;

public sealed partial class PngReader
{
    private PngChunk ReadChunk()
    {
        var chunkLength = _reader.ReadIntBigEndian();
        var chunkName = Encoding.UTF8.GetString(_reader.ReadBytes(4));
        var chunkType = chunkName switch
        {
            "IHDR" => ChunkType.IHDR,
            "IDAT" => ChunkType.IDAT,
            "IEND" => ChunkType.IEND,
            "PLTE" => throw new NotImplementedException(), // TODO: Implement PLTE
            "tRNS" => ChunkType.tRNS,
            "sRGB" => ChunkType.sRGB,
            "gAMA" => ChunkType.gAMA,
            "cHRM" => ChunkType.cHRM,
            "iCCP" => ChunkType.iCCP,
            "sBIT" => ChunkType.sBIT,
            "bKGD" => ChunkType.bKGD,
            "hIST" => ChunkType.hIST,
            "tEXt" => ChunkType.tEXt,
            "zTXt" => ChunkType.zTXt,
            "iTXt" => ChunkType.iTXt,
            "eXIf" => ChunkType.eXIf,
            "tIME" => ChunkType.tIME,
            "pHYs" => ChunkType.pHYs,
            "sPLT" => ChunkType.sPLT,
            _ => throw new InvalidDataException($"Unknown chunk type: {chunkName}")
        };

        // IEND should be 0 always
        if (chunkType == ChunkType.IEND && chunkLength != 0)
            throw new InvalidDataException("IEND length should be 0");
        
        var chunkData = _reader.ReadBytes(chunkLength);
        var chunk = new PngChunk(chunkData, chunkType, chunkLength);
        
        // skip CRC
        _reader.ReadBytes(4);
        
        return chunk;
    }

    // ReSharper disable once InconsistentNaming
    private void DisassembleIHDR(ref PngData dat, PngChunk chunk)
    {
        var width = BinaryPrimitives.ReadUInt32BigEndian(chunk.Data[..4]);
        var height = BinaryPrimitives.ReadUInt32BigEndian(chunk.Data[4..8]);
        var bitDepth = chunk.Data[8];
        var colourType = chunk.Data[9];
        var compressionMethod = chunk.Data[10];
        var filterType = chunk.Data[11];
        var interlaceMethod = chunk.Data[12];
        
        // validate methods
        if (compressionMethod != 0)
            throw new InvalidDataException("Unsupported compression method");
        
        // validate colourType
        if ((colourType == 6 || colourType == 2) && (bitDepth != 8 && bitDepth != 16))
            throw new InvalidDataException("Invalid bitDepth for colour type");
        


        var channels = colourType == 6 ? 4 : 3;
        dat.Channels = channels;
        dat.Width = (int)width;
        dat.Height = (int)height;
        dat.InterlaceMethod = (InterlaceMethod)interlaceMethod;
    }
    
    // ReSharper disable once InconsistentNaming
    private byte[] DecompressIDAT(PngChunk chunk)
    {
        if (chunk.Type != ChunkType.IDAT)
            throw new InvalidDataException("I can decompress IDAT chunk only");

        var span = chunk.Data[2..]; // first 2 bytes is a zlib compression flags according to RFC1950
        using var compressed = new MemoryStream(span);
        using var deflate = new DeflateStream(compressed, CompressionMode.Decompress);
        using var temp = new MemoryStream();
        deflate.CopyTo(temp);
        return temp.ToArray();
    }

    // TODO: Add CRC check
    public sealed class PngChunk
    {
        public byte[] Data { get; set; }
        public ChunkType Type { get; set; }
        public int Length { get; set; }

        public PngChunk(byte[] data, ChunkType type, int length)
        {
            Data = data;
            Type = type;
            Length = length;            
        }
    }

    public enum ChunkType
    {
        IHDR,   // Image header
        IDAT,   // Image data
        IEND,   // End of the PNG stream
        PLTE,   // Palette (used for indexed color images)
        tRNS,   // Transparency (optional, used with PLTE)
        sRGB,   // Standard RGB color space
        gAMA,   // Image gamma
        cHRM,   // Chromaticity (color-related info)
        iCCP,   // ICC Profile
        sBIT,   // Significant bits
        bKGD,   // Background color
        hIST,   // Histogram (frequency of colors)
        tEXt,   // Textual data (uncompressed)
        zTXt,   // Compressed textual data
        iTXt,   // International textual data
        eXIf,   // Exchangeable image file format (Exif)
        tIME,   // Last-modified time
        pHYs,   // Physical pixel dimensions
        sPLT    // Suggested palette
    }


    public enum FilterType
    {
        None,
        Sub = 1,
        Up = 2,
        Average = 3,
        Paeth = 4
    }

    public enum InterlaceMethod
    {
        None,
        Adam7
    }
}