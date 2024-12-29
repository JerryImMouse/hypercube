using System.Buffers.Binary;

namespace Hypercube.Core.IO.Image.Png;

public sealed partial class PngReader : IImageReader
{
    private readonly byte[] _pngHeader = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

    private readonly BinaryReader _reader;

    public PngReader(Stream stream)
    {
        _reader = new BinaryReader(stream);
    }

    public IImageData Read()
    {
        if (!ValidatePngHeader())
            throw new InvalidDataException("Invalid PNG magic.");

        var imgDat = new PngData
        {
            Data = []
        };

        while (_reader.BaseStream.Position < _reader.BaseStream.Length)
        {
            var chunk = ReadChunk();

            if (chunk.Type == ChunkType.IEND)
                break;

            if (chunk.Type == ChunkType.IDAT)
            {
                var decompressedData = DecompressIDAT(chunk);
                var oldData = imgDat.Data;
                imgDat.Data = new byte[oldData.Length + decompressedData.Length];
                oldData.CopyTo(imgDat.Data, 0);
                decompressedData.CopyTo(imgDat.Data, oldData.Length);
                continue;
            }

            if (chunk.Type == ChunkType.IHDR)
                DisassembleIHDR(ref imgDat, chunk);
        }
        
        DecodeData(ref imgDat);

        return imgDat;
    }

    private bool ValidatePngHeader()
    {
        for (var i = 0; i < _pngHeader.Length; i++)
        {
            if (_pngHeader[i] != _reader.ReadByte())
                return false;
        }

        return true;
    }

    // TODO: Add Adam7 impl
    private void DecodeData(ref PngData imgData)
    {
        switch (imgData.InterlaceMethod)
        {
            case InterlaceMethod.None:
            {
                DecodeInterlaceNone(ref imgData);
                break;
            }
            case InterlaceMethod.Adam7:
            {
                DecodeInterlaceAdam7(ref imgData);
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // TODO: Little optimization here, pls
    private void DecodeInterlaceNone(ref PngData imgData)
    {
        var bytesPerScanline = imgData.Width * imgData.Channels;
        var data = new byte[imgData.Width * imgData.Channels * imgData.Height];

        var currentRowStartByteAbsolute = 1;
        var dataIdx = 0;

        for (var rowIndex = 0; rowIndex < imgData.Height; rowIndex++)
        {
            var filterType = (FilterType)imgData.Data[currentRowStartByteAbsolute - 1];
            var previousRowStartByteAbsolute = rowIndex > 0 ? (rowIndex - 1) * bytesPerScanline : -1;
            var rowStartIdx = currentRowStartByteAbsolute;
            var endIdx = rowStartIdx + bytesPerScanline;

            for (var currentByteAbsolute = rowStartIdx; currentByteAbsolute < endIdx; currentByteAbsolute++)
            {
                var rowByteIndex = currentByteAbsolute - rowStartIdx;
                ReverseFilter(imgData.Data, filterType, previousRowStartByteAbsolute, rowStartIdx, currentByteAbsolute, rowByteIndex, imgData.Channels);
                data[dataIdx++] = imgData.Data[currentByteAbsolute];
            }
            currentRowStartByteAbsolute = endIdx + 1;
        }

        imgData.Data = data;
    }

    private void DecodeInterlaceAdam7(ref PngData imgData)
    {
        throw new NotImplementedException();
    }

    private static void ReverseFilter(byte[] data, FilterType filterType, int previousRowStart, int rowStart, int curByte, int rowByteIndex, int bytesPerPixel)
    {
        switch (filterType)
        {
            case FilterType.Up:
            {
                if (previousRowStart >= 0)
                {
                    var higher = previousRowStart + rowByteIndex;
                    data[curByte] += data[higher];
                }

                break;
            }
            case FilterType.Sub:
            {
                var leftIndex = rowByteIndex - bytesPerPixel;
                if (leftIndex >= 0)
                {
                    data[curByte] += data[rowStart + leftIndex];
                }

                break;
            }
            case FilterType.Average:
                data[curByte] += (byte)((GetLeftByteValue(data, rowByteIndex, bytesPerPixel, rowStart) + GetAboveByteValue(data, previousRowStart, rowByteIndex)) / 2);
                break;
            case FilterType.Paeth:
            {
                var left = GetLeftByteValue(data, rowByteIndex, bytesPerPixel, rowStart);
                var above = GetAboveByteValue(data, previousRowStart, rowByteIndex);
                var upperLeft = GetAboveLeftByteValue(data, previousRowStart, rowByteIndex, bytesPerPixel);
                data[curByte] += PaethValue(left, above, upperLeft);
                break;
            }
            case FilterType.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(filterType), filterType, null);
        }
    }
    
    private static byte GetLeftByteValue(byte[] data, int rowByteIndex, int bytesPerPixel, int rowStart)
    {
        var leftIndex = rowByteIndex - bytesPerPixel;
        var leftValue = leftIndex >= 0 ? data[rowStart + leftIndex] : (byte)0;
        return leftValue;
    }

    private static byte GetAboveByteValue(byte[] data, int previousRowStart, int rowByteIndex)
    {
        var upIndex = previousRowStart + rowByteIndex;
        return upIndex >= 0 ? data[upIndex] : (byte)0;
    }

    private static byte GetAboveLeftByteValue(byte[] data, int previousRowStart, int rowByteIndex, int bytesPerPixel)
    {
        var index = previousRowStart + rowByteIndex - bytesPerPixel;
        return index < previousRowStart || previousRowStart < 0 ? (byte)0 : data[index];
    }
    private static byte PaethValue(byte left, byte above, byte upperLeft)
    {
        var p = left + above - upperLeft;
        var pa = Math.Abs(p - left);
        var pb = Math.Abs(p - above);
        var pc = Math.Abs(p - upperLeft);

        if (pa <= pb && pa <= pc)
        {
            return left;
        }

        return pb <= pc ? above : upperLeft;
    }
}