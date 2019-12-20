using System;
using System.Collections.Generic;
using System.Numerics;
using Veldrid;

namespace Engine
{
    public static class Input
    {
        private static HashSet<Key> _currentlyPressedKeys = new HashSet<Key>();
        private static HashSet<Key> _newKeysThisFrame = new HashSet<Key>();

        private static HashSet<MouseButton> _currentlyPressedMouseButtons = new HashSet<MouseButton>();
        private static HashSet<MouseButton> _newMouseButtonsThisFrame = new HashSet<MouseButton>();
        private static (double timeSinceStart, Vector2 mouseCoords) _mouseDownData;
        public static bool GetKey(Key key)
        {
            return _currentlyPressedKeys.Contains(key);
        }

        public static bool GetKeyDown(Key key)
        {
            return _newKeysThisFrame.Contains(key);
        }

        public static void UpdateFrameInput(InputSnapshot snapshot, double elapsedTimeSinceStart)
        {
            _newKeysThisFrame.Clear();
            _newMouseButtonsThisFrame.Clear();
            for (int i = 0; i < snapshot.KeyEvents.Count; i++)
            {
                KeyEvent ke = snapshot.KeyEvents[i];
                if (ke.Down)
                {
                    KeyDown(ke.Key);
                }
                else
                {
                    KeyUp(ke.Key);
                }
            }
            var mousePosition = snapshot.MousePosition;
            for (int i = 0; i < snapshot.MouseEvents.Count; i++)
            {
                MouseEvent me = snapshot.MouseEvents[i];
                if (me.Down)
                {
                    if (_currentlyPressedMouseButtons.Add(me.MouseButton))
                    {
                        _newMouseButtonsThisFrame.Add(me.MouseButton);
                        KestrelApp.Stage.DispatchMouseDownInternal(me.MouseButton, mousePosition);
                        if (me.MouseButton == MouseButton.Left)
                        {
                            _mouseDownData = (elapsedTimeSinceStart, mousePosition);   
                        }
                    }
                }
                else
                {
                    _currentlyPressedMouseButtons.Remove(me.MouseButton);
                    _newMouseButtonsThisFrame.Remove(me.MouseButton);
                    KestrelApp.Stage.DispatchMouseUpInternal(me.MouseButton, mousePosition);
                    if (me.MouseButton == MouseButton.Left && 
                        elapsedTimeSinceStart -_mouseDownData.timeSinceStart < 0.3 &&
                        Vector2.Distance(mousePosition, _mouseDownData.mouseCoords) < 100) // TODO make this DPI dependent
                    {
//                        KestrelApp.Stage.DispatchMouseClick(me.MouseButton, mousePosition);
                    }
                }
            }
            // TODO handle mouse move and dispatch its event.
        }

        private static void KeyUp(Key key)
        {
            _currentlyPressedKeys.Remove(key);
            _newKeysThisFrame.Remove(key);
        }

        private static void KeyDown(Key key)
        {
            if (_currentlyPressedKeys.Add(key))
            {
                _newKeysThisFrame.Add(key);
            }
        }
    }
}
