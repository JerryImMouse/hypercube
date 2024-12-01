﻿using JetBrains.Annotations;

namespace Hypercube.Generators.Class.Data;

[PublicAPI]
public struct Variable
{
    public string Name;
    public Type Type;
    
    public override string ToString()
    {
        return $"{Type} {Name}";
    }
}