﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was copied here by a tool: Masb.NuGet.Multiple.Targeting.Tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is copied again by the tool.
// </auto-generated>
//------------------------------------------------------------------------------
#if portable && (net40)
namespace System.Collections.Generic
{
    public interface IReadOnlyList<out T> :
        IEnumerable<T>
    {
        T this[int index] { get; }
        int Count { get; }
    }
}
#endif