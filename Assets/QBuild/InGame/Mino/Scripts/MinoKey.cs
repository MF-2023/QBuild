using System;

namespace QBuild.Mino
{
    /// <summary>
    /// Minoを管理するのに利用するキー
    /// </summary>
    public struct MinoKey : IEquatable<MinoKey>
    {
        public readonly long Key;

        public static MinoKey NullMino => new MinoKey(-1);

        public MinoKey(long key)
        {
            Key = key;
        }

        public static bool operator ==(MinoKey left, MinoKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MinoKey left, MinoKey right)
        {
            return !(left == right);
        }

        public bool Equals(MinoKey other)
        {
            return Key == other.Key;
        }

        public override bool Equals(object obj)
        {
            return obj is MinoKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}