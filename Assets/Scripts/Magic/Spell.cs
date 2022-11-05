using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic {
    public abstract class Spell : ScriptableObject
    {
        [SerializeField] private int manaCost;

        public abstract void Cast(CastStats castStats);
    }
}
