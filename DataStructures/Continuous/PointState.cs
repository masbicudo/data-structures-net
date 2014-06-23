namespace DataStructures.Continuous
{
    /// <summary>
    /// Represents the three possible states of a point in an interval composed of two points.
    /// </summary>
    public enum PointState
    {
        /// <summary>
        /// The point is open, and as so extends to the infinity.
        /// </summary>
        Open,

        /// <summary>
        /// The point is closed and negative, and as so it is not in the interval.
        /// </summary>
        Excluded,

        /// <summary>
        /// The point is closed and positive, and as so it is in the interval.
        /// </summary>
        Included,
    }
}