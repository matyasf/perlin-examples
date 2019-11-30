using System.Collections.Generic;

namespace Display
{
    public class BatchRenderer
    {
        private readonly List<DisplayObject> _drawQueue = new List<DisplayObject>();
        /// <summary>
        /// Queues an element for drawing. If it causes a state change it draws the current batch
        /// and starts a new one.
        /// </summary>
        public void AddToBatchAndDrawIfNeeded(DisplayObject displayObject)
        {
            
        }

        /// <summary>
        /// Draws the current batch. Called automatically when there is a state change and when the
        /// current frame finishes.
        /// </summary>
        public void DrawCurrentBatch()
        {
            
        }
    }
}