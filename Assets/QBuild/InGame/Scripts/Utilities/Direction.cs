using System;
using UnityEngine;

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

    public enum DirectionFRBL
    {
        None = 0,
        Forward = 1,
        Right = 2,
        Back = 3,
        Left = 4,
    }

    public static class DirectionFRBLExtension
    {
        public static DirectionFRBL VectorToDirectionFRBL(Vector3 vector)
        {
            var angle = Vector3.SignedAngle(vector, Vector3.forward, Vector3.up);
            if (angle < 0)
            {
                angle += 360;
            }
            var angleInt = (int) angle;
            var dir = angleInt switch
            {
                0 => DirectionFRBL.Forward,
                90 => DirectionFRBL.Left,
                180 => DirectionFRBL.Back,
                270 => DirectionFRBL.Right,
                _ => DirectionFRBL.None
            };
            
            
            //前後45度以内なら方向を返す
            angle -= 45;
            if (angle < 0)
            {
                angle += 360;
            }
            var index = Mathf.CeilToInt(angle / 90.0f);
            
            var dir2 = index switch
            {
                0 => DirectionFRBL.Forward,
                1 => DirectionFRBL.Left,
                2 => DirectionFRBL.Back,
                3 => DirectionFRBL.Right,
                4 => DirectionFRBL.Forward,
                _ => DirectionFRBL.None
            };
            
            if (dir2 == DirectionFRBL.None) Debug.Log($"angle:{angle} angleInt:{angleInt} dir:{dir} vector:{vector}");
            return dir2;
        }

        public static Vector3 ToVector3(this DirectionFRBL dir)
        {
            return dir switch
            {
                DirectionFRBL.Forward => Vector3.forward,
                DirectionFRBL.Right => Vector3.right,
                DirectionFRBL.Back => Vector3.back,
                DirectionFRBL.Left => Vector3.left,
                _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
            };
        }

        public static DirectionFRBL TurnLeft(this DirectionFRBL dir)
        {
            return dir switch
            {
                DirectionFRBL.Forward => DirectionFRBL.Left,
                DirectionFRBL.Left => DirectionFRBL.Back,
                DirectionFRBL.Back => DirectionFRBL.Right,
                DirectionFRBL.Right => DirectionFRBL.Forward,
                _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
            };
        }

        public static DirectionFRBL TurnRight(this DirectionFRBL dir)
        {
            return dir switch
            {
                DirectionFRBL.Forward => DirectionFRBL.Right,
                DirectionFRBL.Right => DirectionFRBL.Back,
                DirectionFRBL.Back => DirectionFRBL.Left,
                DirectionFRBL.Left => DirectionFRBL.Forward,
                _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
            };
        }
        
        public static DirectionFRBL Turn180(this DirectionFRBL dir)
        {
            return dir switch
            {
                DirectionFRBL.Forward => DirectionFRBL.Back,
                DirectionFRBL.Right => DirectionFRBL.Left,
                DirectionFRBL.Back => DirectionFRBL.Forward,
                DirectionFRBL.Left => DirectionFRBL.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
            };
        }
    }
}