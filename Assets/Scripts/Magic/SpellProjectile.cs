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
            if (castStats.casterManaController.UseMana(manaCost)) {
                Projectile instance = Instantiate(projectile, castStats.castPosition, Quaternion.identity);
                instance.SetCasterID(castStats.casterID);
                instance.SetDirection(castStats.GetCastDirection());
            }
        }
    }
}
