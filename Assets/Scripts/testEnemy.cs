using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Magic;

public class testEnemy : MonoBehaviour
{
    [SerializeField] private Spell spell;
    [SerializeField] private float spellDelay = 0.5f;
    [SerializeField][Range(0f, 360f)] private float fireAngle = 0f;

    private CastStats castStats = new CastStats();
    private float lastCastTime = 0f;

    private void Start()
    {
        castStats.casterID = gameObject.GetInstanceID();
        castStats.casterManaController = GetComponent<ManaController>();
    }

    private void Update()
    {
        if(Time.time >= lastCastTime+spellDelay)
        {
            castStats.castPosition = transform.position;

            Vector2 castPoint2D = MyMath.RotateVector2(Vector2.right, fireAngle * Mathf.Deg2Rad);
            castStats.castPoint = transform.position + new Vector3(castPoint2D.x, 0f, castPoint2D.y);

            spell.Cast(castStats);
            lastCastTime = Time.time;
        }
    }
}
