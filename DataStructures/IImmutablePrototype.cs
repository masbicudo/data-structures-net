namespace DataStructures
{
    /// <summary>
    /// Interface for objects that can be used as a model for the construction of an immutable object.
    /// </summary>
    /// <typeparam name="TImmutable">Type of the immutable that is represented by this prototype object.</typeparam>
    public interface IImmutablePrototype<out TImmutable>
        where TImmutable : IImmutable
    {
        /// <summary>
        /// Builds an immutable version of this object.
        /// </summary>
        /// <returns>Immutable version of this object.</returns>
        TImmutable ToImmutable();
    }
}