using System;
using System.Collections.Generic;
using System.Numerics;
using Engine.Display;
using Engine.Geom;
using Veldrid;

namespace Engine
{
    /// <summary>
    /// Internal class used in rendering.
    /// </summary>
    internal class BatchRenderer
    {
        private DeviceBuffer _vertexBuffer;
        private readonly List<DisplayObject> _drawQueue = new List<DisplayObject>();
        internal uint DrawCount;
        private Stack<RenderState> _renderStates = new Stack<RenderState>();
        
        public BatchRenderer()
        {
            _vertexBuffer = KestrelApp.DefaultGraphicsDevice.ResourceFactory.CreateBuffer(
                new BufferDescription(1000, BufferUsage.VertexBuffer | BufferUsage.Dynamic));
            _renderStates.Push(new RenderState());
        }
        
        /// <summary>
        /// Called from each render call. Only things in the render queue are rendered
        /// </summary>
        public void AddToRenderQueue(DisplayObject displayObject)
        {
            _drawQueue.Add(displayObject);
        }
        
        /// <summary>
        /// Called when everything is added to the queue once per frame
        /// </summary>
        public void RenderQueue()
        {
            GraphicsDevice gd = KestrelApp.DefaultGraphicsDevice;
            float width = gd.MainSwapchain.Framebuffer.Width;
            float height = gd.MainSwapchain.Framebuffer.Height;
            gd.UpdateBuffer(
                KestrelApp.KestrelPipeline.OrthoBuffer,
                0,
//                Matrix4x4.CreateOrthographicOffCenter(0, width, 0, height, 0, 1));
                Matrix4x4.CreateOrthographicOffCenter(0, width, height, 0, 0, 1));

            EnsureBufferSize((uint)_drawQueue.Count * DisplayObject.QuadVertex.VertexSize);
            MappedResourceView<DisplayObject.QuadVertex> writeMap = gd.Map<DisplayObject.QuadVertex>(_vertexBuffer, MapMode.Write);
            for (int i = 0; i < _drawQueue.Count; i++)
            {
//                writeMap[i] = _drawQueue[i].GpuVertex;
                writeMap[i] = _drawQueue[i].AbsoluteVertex;
            }
            gd.Unmap(_vertexBuffer);
            var cl = KestrelApp.CommandList;
            cl.SetPipeline(KestrelApp.KestrelPipeline.Pipeline);
            cl.SetVertexBuffer(0, _vertexBuffer);
            cl.SetGraphicsResourceSet(0, KestrelApp.KestrelPipeline.OrthoSet);
            DrawCount = 0;
            for (int i = 0; i < _drawQueue.Count;)
            {
                uint batchStart = (uint)i;
                ResourceSet rs = _drawQueue[i].ResSet;
                cl.SetGraphicsResourceSet(1, rs);
                // + textField needs here an extra UpdateBuffer call?
                // cl.UpdateBuffer(_textBuffer, 0, toDraw[0].GpuVertex);
                uint batchSize = 0;
                do
                {
                    i += 1;
                    batchSize += 1;
                }
                while (i < _drawQueue.Count && _drawQueue[i].ResSet == rs);
                DrawCount++;
                cl.Draw(4, batchSize, 0, batchStart); // it writes different batches into the same buffer!!
            }
            _drawQueue.Clear();
        }
        
        private void EnsureBufferSize(uint size)
        {
            if (_vertexBuffer.SizeInBytes < size)
            {
                _vertexBuffer.Dispose();
                _vertexBuffer = KestrelApp.DefaultGraphicsDevice.ResourceFactory.CreateBuffer(
                    new BufferDescription(size, BufferUsage.VertexBuffer | BufferUsage.Dynamic));
            }
        }

        public RenderState PushRenderState(float alpha, Matrix2D matrix2D)
        {
            var rs = new RenderState();
            rs.CopyFrom(_renderStates.Peek());
            rs.Alpha *= alpha;
            rs.TransformModelviewMatrix(matrix2D);
            _renderStates.Push(rs);
            return rs;
        }

        public void PopRenderState()
        {
            _renderStates.Pop();
        }
    }
}