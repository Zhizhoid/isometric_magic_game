using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Creature
{
    public class Creature : MonoBehaviour
    {
        public void TakeDamage()
        {
            Debug.Log(gameObject.name + " took damage");
        }
    }
}
