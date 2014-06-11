namespace DataStructures
{
    /// <summary>
    /// Interface that may be used to mark immutable classes and structs.
    /// </summary>
    /// <remarks>
    /// ReadOnly is different from Immutable.
    /// <para>
    /// A ReadOnly interface indicates the object cannot be changed through the interface,
    /// but could be changed by other actors, that have access to the underlying object data.
    /// </para>
    /// <para>
    /// An immutable interface indicates that an object is guaranteed never to change.
    /// It must be impossible for that to happen.
    /// Of course it is always possible to abuse an immutable object,
    /// using reflection for example... but this could break a lot of things that rely on the immutable behavior.
    /// </para>
    /// </remarks>
    public interface IImmutable
    {
    }
}