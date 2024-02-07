using UnityEngine;

namespace QBuild
{
    public interface IDamageable
    {
        void Knockback(Vector3 direction, float power);
        
        void Damage(int damage);
    }
}