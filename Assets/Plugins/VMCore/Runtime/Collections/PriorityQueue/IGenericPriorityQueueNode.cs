namespace VMFramework.Core
{
    public interface IGenericPriorityQueueNode<TPriority>
    {
        /// <summary>
        /// The Priority to insert this node at.
        /// Cannot be manually edited - see queue.Enqueue() and queue.UpdatePriority() instead
        /// Just explicit implement this property to make it clear that it is read-only
        /// </summary>
        TPriority Priority { get; set; }

        /// <summary>
        /// Represents the current position in the queue
        /// Just explicit implement this property to make it clear that it is read-only
        /// </summary>
        int QueueIndex { get; set; }

        /// <summary>
        /// Represents the order the node was inserted in
        /// Just explicit implement this property to make it clear that it is read-only
        /// </summary>
        long InsertionIndex { get; set; }
    }
}