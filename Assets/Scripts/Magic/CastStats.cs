using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    public class CastStats
    {
        public Vector3 castPosition;
        public Vector3 castPoint;
        public int casterID;
        public ManaController casterManaController;

        public Vector3 GetCastDirection()
        {
            return new Vector3(castPoint.x - castPosition.x, 0f, castPoint.z - castPosition.z).normalized;
        }
    }
}
