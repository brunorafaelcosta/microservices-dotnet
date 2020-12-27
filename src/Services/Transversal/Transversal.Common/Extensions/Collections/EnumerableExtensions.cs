using System;
using System.Collections.Generic;
using System.Linq;

namespace Transversal.Common.Extensions.Collections
{
    /// <summary>
    /// Provides additional methods to <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        #region Flatten

        /// <summary>
        /// Flattens the <see cref="IEnumerable{T}"/> and his <see cref="IEnumerable{T}"/> child
        /// into a single <see cref="IEnumerable{T}"/> regardless of their depth
        /// </summary>
        /// <typeparam name="T">Type of elements in the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="enumerator"><see cref="IEnumerable{T}"/> object</param>
        /// <param name="childEnumeratorFunc">Method to resolve the <see cref="IEnumerable{T}"/> child</param>
        /// <returns>Flattened <see cref="IEnumerable{T}"/></returns>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> enumerator, Func<T, IEnumerable<T>> childEnumeratorFunc)
        {
            return enumerator.SelectMany(c => childEnumeratorFunc(c).Flatten(childEnumeratorFunc)).Concat(enumerator);
        }

        /// <summary>
        /// Flattens the <see cref="IEnumerable{T}"/> and his <see cref="IEnumerable{T}"/> child
        /// into a single <see cref="IEnumerable{T}"/> regardless of their depth but returns only the distinct elements
        /// </summary>
        /// <typeparam name="T">Type of elements in the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="enumerator"><see cref="IEnumerable{T}"/> object</param>
        /// <param name="childEnumeratorFunc">Method to resolve the <see cref="IEnumerable{T}"/> child</param>
        /// <returns>Distinct elements from the flattened <see cref="IEnumerable{T}"/></returns>
        public static IEnumerable<T> DistinctFlatten<T>(this IEnumerable<T> enumerator, Func<T, IEnumerable<T>> childEnumeratorFunc)
        {
            return enumerator.Flatten(childEnumeratorFunc).Distinct();
        }

        #endregion

        #region Topological sort

        /// <summary>
        /// Sorts the <see cref="IEnumerable{T}"/> so that an element must be preceded
        /// by all the elements in the <see cref="IEnumerable{T}"/> child
        /// </summary>
        /// <see cref="http://www.codeproject.com/Articles/869059/Topological-sorting-in-Csharp"/>
        /// <seealso cref="http://en.wikipedia.org/wiki/Topological_sorting"/>
        /// <typeparam name="T">Type of elements in the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="enumerator"><see cref="IEnumerable{T}"/> object</param>
        /// <param name="childEnumeratorFunc">Method to resolve the <see cref="IEnumerable{T}"/> child</param>
        /// <returns>Topological sorted <see cref="IEnumerable{T}"/></returns>
        public static IEnumerable<T> TopologicalSort<T>(this IEnumerable<T> enumerator, Func<T, IEnumerable<T>> childEnumeratorFunc)
        {
            var sorted = new List<T>();
            var visited = new Dictionary<T, bool>();

            foreach (var element in enumerator)
            {
                TopologicalSort_Visit(element, childEnumeratorFunc, sorted, visited);
            }

            return sorted;
        }

        /// <summary>
        /// Resolves the element and its children elements
        /// </summary>
        /// <typeparam name="T">Type of elements in the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="element">Element to resolve</param>
        /// <param name="childEnumeratorFunc">Method to resolve the <see cref="IEnumerable{T}"/> child</param>
        /// <param name="sorted">List with the sortet elements</param>
        /// <param name="visited">Dictionary with the visited elements</param>
        private static void TopologicalSort_Visit<T>(T element, Func<T, IEnumerable<T>> childEnumeratorFunc, List<T> sorted, Dictionary<T, bool> visited)
        {
            bool inProcess;
            var alreadyVisited = visited.TryGetValue(element, out inProcess);

            if (alreadyVisited)
            {
                if (inProcess)
                {
                    throw new InvalidOperationException("Cyclic dependency found.");
                }
            }
            else
            {
                visited[element] = true;

                var dependencies = childEnumeratorFunc(element);
                if (dependencies != null)
                {
                    foreach (var dependency in dependencies)
                    {
                        TopologicalSort_Visit(dependency, childEnumeratorFunc, sorted, visited);
                    }
                }

                visited[element] = false;
                sorted.Add(element);
            }
        }

        #endregion
    }
}
