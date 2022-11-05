using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Player {

    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private Camera cam;

        private CharacterController ch;

        private void Start()
        {
            ch = GetComponent<CharacterController>();
        }

        public void HandleMovement()
        {
            Vector2 moveDir2D = MyMath.RotateVector2(inputMoveDir(), -cam.transform.rotation.eulerAngles.y * Mathf.Deg2Rad);
            Vector3 motion = new Vector3(moveDir2D.x, 0f, moveDir2D.y) * moveSpeed * Time.deltaTime;

            ch.Move(motion);
        }

        private Vector2 inputMoveDir()
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");

            return new Vector2(inputX, inputY).normalized;
        }
    }
}
