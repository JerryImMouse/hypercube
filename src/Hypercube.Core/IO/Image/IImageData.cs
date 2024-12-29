namespace Hypercube.Core.IO.Image;

public interface IImageData
{
    int Width { get; }
    int Height { get; }
    int Channels { get; }
    byte[] Data { get; }
}