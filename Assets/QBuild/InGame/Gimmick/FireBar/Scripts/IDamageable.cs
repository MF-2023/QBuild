using UnityEngine;

namespace QBuild
{
    public interface IDamageable
    {
        /// <summary>
        /// ノックバックする
        /// </summary>
        /// <param name="direction">ノックバックする向き</param>
        /// <param name="power">ノックバックする力</param>
        /// <param name="isForce">無効化中でも行う</param>
        void Knockback(Vector3 direction, float power, bool isForce = false);

        void Damage(int damage, bool isForce = false);
    }
}