using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LinkDotNet.StringBuilder;

/// <summary>
/// Represents a List based on the <see cref="Span{T}"/> type.
/// </summary>
/// <typeparam name="T">Any struct.</typeparam>
[StructLayout(LayoutKind.Auto)]
[SkipLocalsInit]
internal ref struct TypedSpanList<T>
    where T : struct
{
    private Span<T> _buffer;
    private int _count;

    /// <summary>
    /// Initializes a new instance of the <see cref="TypedSpanList{T}"/> struct.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TypedSpanList()
    {
        _buffer = GC.AllocateUninitializedArray<T>(8);
        _count = 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly ReadOnlySpan<T> AsSpan() => _buffer[.._count];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T value)
    {
        if (_count >= _buffer.Length)
        {
            Grow();
        }

        _buffer[_count] = value;
        _count++;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Grow(int capacity = 0)
    {
        var currentSize = _buffer.Length;
        var newSize = capacity > 0 ? capacity : currentSize * 2;
        var rented = GC.AllocateUninitializedArray<T>(newSize);
        _buffer.CopyTo(rented);
        _buffer = rented;
    }
}