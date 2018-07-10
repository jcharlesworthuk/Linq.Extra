using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LinqExtra
{
    public static class EnumerableExtensions
    {

        /// <summary>
        /// Pages the results using Skip() and Take()
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable`1 to return elements from.</param>
        /// <param name="pageNumber">Page number (starts from 1)</param>
        /// <param name="pageSize">Maxinum number of records per page</param>
        /// <returns></returns>
        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int pageNumber, int pageSize) 
            => source.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        /// <summary>
        /// Returns the source enumerable with the supplied action invoked on each item.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable`1 to return elements from.</param>
        /// <param name="action">An action to invoke on each item</param>
        /// <returns></returns>
        public static IEnumerable<T> WithAction<T>(this IEnumerable<T> source, Action<T> action) 
            => source.Select(x => { action(x); return x; });


        /// <summary>
        /// Forces the collection to enumerate and does not return a value, Similar to IList.ForEach() without any operations
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable`1 to enumerate.</param>
        public static void Enumerate<T>(this IEnumerable<T> source)
            => source.ToList();

        /// <summary>
        /// Returns alternate elements of this and the supplied collections.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable`1 to return elements from.</param>
        /// <param name="other">An alternative collection of any length to interweave this collection with</param>
        /// <returns></returns>
        public static IEnumerable<T> Interweave<T>(this IEnumerable<T> source, IEnumerable<T> other)
        {
            using (var enumeratorA = source.GetEnumerator())
            using (var enumeratorB = other.GetEnumerator())
            {
                while (enumeratorA.MoveNext())
                {
                    yield return enumeratorA.Current;
                    if (enumeratorB.MoveNext())
                        yield return enumeratorB.Current;
                }

                while (enumeratorB.MoveNext())
                    yield return enumeratorB.Current;
            }
        }

        /// <summary>
        /// Returns a smaller collection consisting of every X elements in the source.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable`1 to return elements from.</param>
        /// <param name="every">The number of elements to skip on each iteration</param>
        /// <returns></returns>
        public static IEnumerable<T> EveryOther<T>(this IEnumerable<T> source, int every = 2)
        {
            return source.Select((item, index) => new {item, index}).Where(indexed => (indexed.index + 1) % every == 0).Select(indexed => indexed.item);
        }

        /// <summary>
        /// Creates a Collection<T> from an IEnumerable<T>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable`1 to return elements from.</param>
        /// <returns></returns>
        public static Collection<T> ToCollection<T>(this IEnumerable<T> source) 
            => new Collection<T>(source.ToList());

        /// <summary>
        /// Returns only the elements that are not null
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable`1 to return elements from.</param>
        /// <returns></returns>
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> source) 
            where T : class 
            => source.Where(x => x != null);

        /// <summary>
        /// Returns the duplicate items
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable`1 to return elements from.</param>
        /// <returns></returns>
        public static IEnumerable<T> Duplicates<T>(this IEnumerable<T> source)
            where T : struct, IEquatable<T>
            => source.Where(x => source.Count(y => y .Equals(x)) > 1).Distinct();

        /// <summary>
        /// Returns the duplicate items using the specified comparer
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable`1 to return elements from.</param>
        /// <param name="comparer"></param>
        /// <returns>The first instance of each duplicate set</returns>
        public static IEnumerable<T> Duplicates<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer) 
            => source.GroupBy(comparer.GetHashCode).Where(x => x.Count() > 1).SelectMany(x => x).Distinct(comparer);

        /// <summary>
        /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable`1 to return elements from.</param>
        /// <param name="compareTo"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static T FirstOrDefault<T>(this IEnumerable<T> source, T compareTo, IEqualityComparer<T> comparer) 
            => source.FirstOrDefault(x => comparer.Equals(x, compareTo));

        /// <summary>
        /// Returns a single, specific element of a sequence of values, or a default value if no such element is found.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable`1 to return elements from.</param>
        /// <param name="compareTo"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static T SingleOrDefault<T>(this IEnumerable<T> source, T compareTo, IEqualityComparer<T> comparer) 
            => source.SingleOrDefault(x => comparer.Equals(x, compareTo));

        /// <summary>
        /// Returns a single, specific element of a sequence of values, or a default value if no such element is found.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable`1 to return elements from.</param>
        /// <param name="compareTo"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static T LastOrDefault<T>(this IEnumerable<T> source, T compareTo, IEqualityComparer<T> comparer) 
            => source.LastOrDefault(x => comparer.Equals(x, compareTo));

        /// <summary>
        /// Projects each element of a sequence to an IEnumerable<T> and flattens the resulting two sequences into one sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <typeparam name="TResultA"></typeparam>
        /// <typeparam name="TResultB"></typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable`1 to return elements from.</param>
        /// <param name="selectorA"></param>
        /// <param name="selectorB"></param>
        /// <returns></returns>
        public static IEnumerable<TResultB> SelectManyMany<T, TResultA, TResultB>(this IEnumerable<T> source, Func<T, IEnumerable<TResultA>> selectorA, Func<TResultA, IEnumerable<TResultB>> selectorB) 
            => source.SelectMany(selectorA).SelectMany(selectorB);

        /// <summary>
        /// Performs a SelectMany() but returning a tuple containing both the parent and the child element.
        /// </summary>
        /// <typeparam name="T1">The type of the elements of source</typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable`1 to return elements from.</param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<T1, T2>> Expand<T1, T2>(this IEnumerable<T1> source, Func<T1, IEnumerable<T2>> selector) 
            => source.SelectMany(selector, (t1, t2) => new Tuple<T1, T2>(t1, t2));

        /// <summary>
        /// Returns distinct elements from a sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable`1 to return elements from.</param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, TProperty>(this IEnumerable<T> source, Func<T, TProperty> selector)
            where TProperty : struct, IEquatable<TProperty> 
            => source.Distinct(new DistinctPropertyComparer<T, TProperty>(selector));


        /// <summary>
        /// Takes a collection and breaks it up into many smaller chunks.
        /// </summary>
        /// <typeparam name="T">The type of the contents.</typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable`1 to chunk.</param>
        /// <param name="chunkSize">The size to chunk into</param>
        /// <returns>A collection of collections.</returns>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var batch = new List<T> { enumerator.Current };
                    int index = 1;
                    while (index++ < chunkSize && enumerator.MoveNext())
                    {
                        batch.Add(enumerator.Current);
                    }

                    yield return batch;
                }
            }
        }

        /// <summary>
        /// Returns true if this enumerable is null or empty
        /// </summary>
        /// <typeparam name="T">Collection item type</typeparam>
        /// <param name="source">Source collection, or null</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        /// <summary>
        /// Property comparer for value types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        private class DistinctPropertyComparer<T, TProperty> : EqualityComparer<T>
            where TProperty : struct, IEquatable<TProperty>
        {
            private readonly Func<T, TProperty> selector;
            public DistinctPropertyComparer(Func<T, TProperty> selector)
            {
                this.selector = selector;
            }

            public override bool Equals(T x, T y)
            {
                return selector(x).Equals(selector(y));
            }

            public override int GetHashCode(T obj)
            {
                return selector(obj).GetHashCode();
            }
        }


    }
}
