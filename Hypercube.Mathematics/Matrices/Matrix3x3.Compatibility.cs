﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Hypercube.Mathematics.Matrices;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public partial struct Matrix3x3
{
    /*
     * OpenTK Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OpenTK.Mathematics.Matrix3(Matrix3x3 matrix3)
    {
        return new OpenTK.Mathematics.Matrix3(matrix3.Row0, matrix3.Row1, matrix3.Row2);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Matrix3x3(OpenTK.Mathematics.Matrix3 matrix3)
    {
        return new Matrix3x3(matrix3.Row0, matrix3.Row1, matrix3.Row2);
    }
    
    /*
     * OpenToolkit Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OpenToolkit.Mathematics.Matrix3(Matrix3x3 matrix3)
    {
        return new OpenToolkit.Mathematics.Matrix3(matrix3.Row0, matrix3.Row1, matrix3.Row2);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Matrix3x3(OpenToolkit.Mathematics.Matrix3 matrix3)
    {
        return new Matrix3x3(matrix3.Row0, matrix3.Row1, matrix3.Row2);
    }
}