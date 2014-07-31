using JetBrains.Annotations;
using System;

namespace DataStructures.Monads
{
    public static class OptionExtensions
    {
        public static TResult Match<T, TResult>(
            [NotNull] this IOption<T> option,
            [NotNull] Func<T, TResult> whenSome,
            [NotNull] Func<TResult> whenNone)
        {
            if (option == null)
                throw new ArgumentNullException("option");

            if (whenSome == null)
                throw new ArgumentNullException("whenSome");

            if (whenNone == null)
                throw new ArgumentNullException("whenNone");

            if (option == None<T>.Instance)
                return whenNone();

            return whenSome(option.Value);
        }

        /// <summary>
        /// Gets the value of something, or returns the default value of the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the option value.</typeparam>
        /// <param name="option">Option to match.</param>
        /// <returns>A value returned by the delegate when the option has something; otherwise the default value of the result type.</returns>
        public static T GetSomeOrDefault<T>(
            [NotNull] this IOption<T> option)
        {
            if (option == null)
                throw new ArgumentNullException("option");

            if (option == None<T>.Instance)
                return default(T);

            return option.Value;
        }

        /// <summary>
        /// Matches an option that is something, or returns the default value of the type <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="T">Type of the option value.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="option">Option to match.</param>
        /// <param name="whenSome">Delegate that gets the result from an option with something.</param>
        /// <returns>A value returned by the delegate when the option has something; otherwise the default value of the result type.</returns>
        public static TResult MatchSomeOrDefault<T, TResult>(
            [NotNull] this IOption<T> option,
            [NotNull] Func<T, TResult> whenSome)
        {
            if (option == null)
                throw new ArgumentNullException("option");

            if (whenSome == null)
                throw new ArgumentNullException("whenSome");

            if (option == None<T>.Instance)
                return default(TResult);

            return whenSome(option.Value);
        }

        /// <summary>
        /// Matches an option that is something, executing an action in this case.
        /// Otherwise does nothing.
        /// </summary>
        /// <typeparam name="T">Type of the option value.</typeparam>
        /// <param name="option">Option to match.</param>
        /// <param name="whenSome">Delegate that gets the result from an option with something.</param>
        public static void MatchSomeOrDefault<T>(
            [NotNull] this IOption<T> option,
            [NotNull] Action<T> whenSome)
        {
            if (option == null)
                throw new ArgumentNullException("option");

            if (whenSome == null)
                throw new ArgumentNullException("whenSome");

            if (option == None<T>.Instance)
                return;

            whenSome(option.Value);
        }
    }
}