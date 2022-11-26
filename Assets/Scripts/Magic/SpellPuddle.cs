using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    [CreateAssetMenu(fileName = "SpellPuddle", menuName = "Magic/Spells/PuddleSpell")]
    public class SpellPuddle : Spell
    {
        [SerializeField] private Puddle puddle;
        public override void Cast(CastStats castStats)
        {
            Ray ray = new Ray(castStats.castPoint + Vector3.up*0.001f, Vector3.down);
            if( Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, LayerMask.GetMask("Ground")) ) {
                if (castStats.casterManaController.UseMana(manaCost)) {
                    Instantiate(puddle, hit.point, Quaternion.identity);
                }
            }
        }
    }
}
