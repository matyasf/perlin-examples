using System;
using Perlin.Geom;

namespace Codecool.Snake
{
    /// <summary>
    /// Utility functions
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Converts a direction in degrees (0...360) to x and y coordinates.
        /// The length of this vector is the second parameter
        /// </summary>
        /// <returns>the direction vector</returns>
        /// <param name="directionInDegrees">Direction in degrees</param>
        /// <param name="length">length</param>
        public static Point DirectionToVector(float directionInDegrees, float length)
        {
            float directionInRadians = directionInDegrees / 180 * (float)Math.PI;
            Point heading = new Point(length * (float)Math.Sin(directionInRadians), -length * (float)Math.Cos(directionInRadians));
            return heading;
        }
    }
}