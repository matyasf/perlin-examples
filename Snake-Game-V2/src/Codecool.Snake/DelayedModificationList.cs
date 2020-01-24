using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Codecool.Snake
{
    /// <summary>
    /// A special container which maintains a list of objects. If any modification (addition, removal)
    /// is done, it won't happen until the doPendingModifications() method is called
    /// </summary>
    public class DelayedModificationList<T>
    {
        private LinkedList<T> _objects = new LinkedList<T>();
        private LinkedList<T> _newObjects = new LinkedList<T>(); // Holds game objects created in this frame.
        private LinkedList<T> _oldObjects = new LinkedList<T>(); // Holds game objects that will be destroyed this frame.

        /// <summary>
        /// Adds an element to the end of the list
        /// </summary>
        /// <param name="obj">The object to add</param>
        public void Add(T obj)
        {
            _newObjects.AddLast(obj);
        }

        /// <summary>
        /// Adds all elements from the list to the end of this list.
        /// </summary>
        /// <param name="objs">The objects to add</param>
        public void AddAll(IEnumerable<T> objs)
        {
            foreach (T obj in objs)
            {
                Add(obj);
            }
        }

        /// <summary>
        /// Queues an object for removing
        /// </summary>
        /// <param name="obj">The object to remove</param>
        public void Remove(T obj)
        {
            _oldObjects.AddLast(obj);
        }

        /// <summary>
        /// Gets this object as a List.
        /// </summary>
        public IImmutableList<T> List => _objects.ToImmutableList();

        /// <summary>
        /// Gets a value indicating whether this list is empty.
        /// </summary>
        /// <returns>True if empty</returns>
        public bool IsEmpty
        {
            get
            {
                if (_newObjects.Count > 0 || _objects.Count > 0)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        ///  All the modifications (Add, Remove) is pending until you call this method
        /// </summary>
        public void DoPendingModifications()
        {
            foreach (var item in _newObjects)
            {
                _objects.AddLast(item);
            }

            _newObjects.Clear();
            foreach (var item in _oldObjects)
            {
                _objects.Remove(item);
            }

            _oldObjects.Clear();
        }

        /// <summary>
        /// Gets the last element of this list
        /// </summary>
        public T Last
        {
            get
            {
                if (_newObjects.Count > 0) return _newObjects.Last();
                if (_objects.Count > 0) return _objects.Last();
                return default; // null
            }
        }

        /// <summary>
        /// Removes every element from this list.
        /// </summary>
        public void Clear()
        {
            _objects.Clear();
            _newObjects.Clear();
            _oldObjects.Clear();
        }
    }
}