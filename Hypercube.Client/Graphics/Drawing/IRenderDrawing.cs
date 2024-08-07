﻿using Hypercube.Client.Graphics.Texturing;
using Hypercube.Shared.Math;
using Hypercube.Shared.Math.Box;
using Hypercube.Shared.Math.Matrix;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Client.Graphics.Drawing;

public interface IRenderDrawing
{
    void DrawTexture(ITexture texture, Vector2 position);
    void DrawTexture(ITexture texture, Vector2 position, Color color);
    void DrawTexture(ITexture texture, Box2 quad, Box2 uv);
    void DrawTexture(ITexture texture, Box2 quad, Box2 uv, Color color);
    void DrawTexture(ITextureHandle texture, Vector2 position, Color color);
    void DrawTexture(ITextureHandle texture, Vector2 position, Color color, Matrix4X4 model);
    void DrawTexture(ITextureHandle texture, Box2 quad, Box2 uv, Color color);
    void DrawTexture(ITextureHandle texture, Box2 quad, Box2 uv, Color color, Matrix4X4 model);
}