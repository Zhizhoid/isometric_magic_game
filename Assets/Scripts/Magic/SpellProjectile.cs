using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    [CreateAssetMenu(fileName = "SpellProjectile", menuName = "Magic/Spells/ProjectileSpell")]
    public class SpellProjectile : Spell
    {
        [SerializeField] private Projectile projectile;
        public override void Cast(CastStats castStats)
        {
            projectile.direction = castStats.castDirection;
            projectile.casterID = castStats.casterID;

            Instantiate(projectile, castStats.castPosition, Quaternion.identity);
        }
    }
}
