using System;

namespace QBuild.Utilities
{
    public enum Direction
    {
        None = 10,
        East = 1,
        North = 2,
        West = 3,
        South = 0,
    }

    public static class DirectionExtension
    {
        public static Direction TurnLeft(this Direction dir)
        {
            return dir switch
            {
                Direction.East => Direction.North,
                Direction.North => Direction.West,
                Direction.West => Direction.South,
                Direction.South => Direction.East,
                _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
            };
        }
        
        public static Direction TurnRight(this Direction dir)
        {
            return dir switch
            {
                Direction.East => Direction.South,
                Direction.North => Direction.East,
                Direction.West => Direction.North,
                Direction.South => Direction.West,
                _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
            };
        }
    }
}