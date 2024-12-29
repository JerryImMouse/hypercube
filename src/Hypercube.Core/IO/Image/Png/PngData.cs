namespace Hypercube.Core.IO.Image.Png;

public struct PngData : IImageData
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int Channels { get; set; }
    public byte[] Data { get; set; }
    
    public PngReader.InterlaceMethod InterlaceMethod { get; set; }
}