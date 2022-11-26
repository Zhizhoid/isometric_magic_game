using System.Collections;
using System.Collections.Generic;
using Creatures;
using UnityEngine;

namespace Magic {
    public abstract class Spell : ScriptableObject
    {
        [SerializeField] protected int manaCost;

        public abstract void Cast(CastStats castStats);
    }
}
