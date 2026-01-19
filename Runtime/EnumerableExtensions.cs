#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

namespace Neonalig.Extensions
{
    public static class EnumerableExtensions
    {
        #region Select

        public static IEnumerable<TOut> SelectNotNull<TIn, TOut>([InstantHandle] this IEnumerable<TIn> source, [InstantHandle] Func<TIn, TOut?> selector) where TOut : class
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            foreach (var item in source)
            {
                var selected = selector(item);
                if (selected != null)
                    yield return selected;
            }
        }

        public delegate bool TryGetValueDelegate<in TIn, TOut>(TIn input, [NotNullWhen(true)] out TOut? output);
        public static IEnumerable<TOut> SelectNotNull<TIn, TOut>([InstantHandle] this IEnumerable<TIn> source, [InstantHandle] TryGetValueDelegate<TIn, TOut> tryGetValue) where TOut : class
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (tryGetValue == null) throw new ArgumentNullException(nameof(tryGetValue));

            foreach (var item in source)
            {
                if (tryGetValue(item, out var selected))
                    yield return selected;
            }
        }

        #endregion

        #region Random Element Selection

        /// <summary>
        /// Returns a random element from the given list.
        /// </summary>
        /// <remarks>Uses <see cref="UnityEngine.Random"/> for random selection.<br/>
        /// If deterministic random selection is needed, consider calling <see cref="UnityEngine.Random.InitState(int)"/> with a specific seed before calling this method (it's generally recommended to store the previous state and restore it after use).</remarks>
        /// <param name="collection">The list to select a random element from.</param>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <returns>A random element from the list.</returns>
        /// <exception cref="ArgumentException">Thrown if the list is null or empty.</exception>
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        public static T GetRandom<T>([InstantHandle] this IReadOnlyList<T> collection)
        {
            if (collection == null || collection.Count == 0)
                throw new ArgumentException("Collection cannot be null or empty", nameof(collection));

            int randomIndex = Random.Range(0, collection.Count);
            return collection[randomIndex];
        }

        /// <inheritdoc cref="GetRandom{T}(IReadOnlyList{T})"/>
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        public static T GetRandom<T>([InstantHandle] this IList<T> collection)
        {
            if (collection == null || collection.Count == 0)
                throw new ArgumentException("Collection cannot be null or empty", nameof(collection));

            int randomIndex = Random.Range(0, collection.Count);
            return collection[randomIndex];
        }

        /// <inheritdoc cref="GetRandom{T}(IReadOnlyList{T})"/>
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        public static T GetRandom<T>(this T[] array) => GetRandom((IReadOnlyList<T>)array);

        /// <inheritdoc cref="GetRandom{T}(IReadOnlyList{T})"/>
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        public static T GetRandom<T>([InstantHandle] this ICollection<T> collection)
        {
            if (collection == null || collection.Count == 0)
                throw new ArgumentException("Collection cannot be null or empty", nameof(collection));

            int randomIndex = Random.Range(0, collection.Count);
            return collection.ElementAt(randomIndex);
        }

        /// <inheritdoc cref="GetRandom{T}(IReadOnlyList{T})"/>
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        public static T GetRandom<T>([InstantHandle] this IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentException("Collection cannot be null", nameof(collection));

            IEnumerator<T> enumerator = collection.GetEnumerator();
            using (enumerator)
            {
                return GetRandom(enumerator);
            }
        }

        /// <inheritdoc cref="GetRandom{T}(IReadOnlyList{T})"/>
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        public static T GetRandom<T>([InstantHandle] this IEnumerator<T> enumerator)
        {
            if (!enumerator.MoveNext())
                throw new ArgumentException("Collection cannot be empty", nameof(enumerator));

            int count = 1;
            T result = enumerator.Current;

            // Use reservoir sampling to select a random element (uniform distribution despite only having a single pass)
            while (enumerator.MoveNext())
            {
                count++;
                if (Random.Range(0, count) == 0)
                {
                    result = enumerator.Current;
                }
            }

            return result;
        }

        #endregion

        #region Difference

        /// <summary>
        /// Calculates the difference between two collections, returning the elements that were added and removed.
        /// </summary>
        /// <param name="source">The original collection.</param>
        /// <param name="other">The collection to compare against.</param>
        /// <param name="added">The elements that were added in the other collection.</param>
        /// <param name="removed">The elements that were removed from the source collection.</param>
        /// <typeparam name="T">The type of elements in the collections.</typeparam>
        /// <exception cref="ArgumentNullException">Thrown if either collection is null.</exception>
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        public static void GetDifference<T>([InstantHandle] this IEnumerable<T> source, [InstantHandle] IEnumerable<T> other, out List<T> added, out List<T> removed)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (other == null) throw new ArgumentNullException(nameof(other));

            var sourceSet = new HashSet<T>(source);
            var otherSet = new HashSet<T>(other);

            added = otherSet.Except(sourceSet).ToList();
            removed = sourceSet.Except(otherSet).ToList();
        }

        #endregion

        #region Index Of

        /// <summary>
        /// Finds the index of the first element equivalent to the specified value in the enumerable.
        /// </summary>
        /// <param name="source">The enumerable to search.</param>
        /// <param name="value">The value to locate in the enumerable.</param>
        /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
        /// <returns>The zero-based index of the first occurrence of the value within the enumerable, or -1 if not found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the source enumerable is null.</exception>
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        public static int IndexOf<T>([InstantHandle] this IEnumerable<T> source, [CanBeNull] T value)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            switch (source)
            {
                case T[] array:
                    return Array.IndexOf(array, value);
                case IList<T> list:
                    return list.IndexOf(value);
                case IReadOnlyList<T> readOnlyList:
                {
                    var comparer = EqualityComparer<T>.Default;
                    for (int i = 0; i < readOnlyList.Count; i++)
                    {
                        if (comparer.Equals(readOnlyList[i], value))
                            return i;
                    }
                    return -1;
                }
                default:
                {
                    int index = 0;
                    var comparer = EqualityComparer<T>.Default;
                    foreach (var item in source)
                    {
                        if (comparer.Equals(item, value))
                            return index;
                        index++;
                    }
                    return -1;
                }
            }
        }

        #endregion
    }
}
