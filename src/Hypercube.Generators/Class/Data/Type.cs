﻿using JetBrains.Annotations;

namespace Hypercube.Generators.Class.Data;

[PublicAPI]
public struct Type
{
    public string Name;
    public bool Array;
    public int PointerLevel;
    
    public string ArrayString => Array ? "[]" : string.Empty;
    public string PointerString => string.Concat(Enumerable.Repeat("*", PointerLevel));

    public override string ToString()
    {
        return $"{Name}{PointerString}{ArrayString}";
    }
}