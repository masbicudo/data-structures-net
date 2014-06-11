namespace DataStructures.Immutable
{
    /// <summary>
    /// Interface used to indicate whether an object is mutable or immutable.
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
    /// <para>
    /// To guarantee that an object implementing `IMayBeImmutable` is immutable,
    /// all the accesses to the object must be synchronized.
    /// Different threads must all see the most recent value of the `IsImmutable`,
    /// and the inner workings of the object must always respect the most recent value.
    /// </para>
    /// <para>
    /// After an object changes from mutable to immutable state,
    /// it should be impossible to revert this action.
    /// </para>
    /// </remarks>
    public interface IMayBeImmutable
    {
        /// <summary>
        /// Gets a value indicating whether this object is immutable or not.
        /// </summary>
        bool IsImmutable { get; }
    }
}