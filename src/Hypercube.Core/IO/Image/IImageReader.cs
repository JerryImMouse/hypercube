using System.Runtime.InteropServices.ComTypes;

namespace Hypercube.Core.IO.Image;

public interface IImageReader
{
    IImageData Read();
}