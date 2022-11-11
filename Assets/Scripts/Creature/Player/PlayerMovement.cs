using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Player {

    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 10f;

        private CharacterController ch;
        private Player player;

        private float targetRot;

        private void Start()
        {
            ch = GetComponent<CharacterController>();
            player = GetComponent<Player>();
        }

        public void HandleMovement()
        {
            float camRotation = player.GetCamera().transform.rotation.eulerAngles.y;
            Vector2 moveDir2D = MyMath.RotateVector2(inputMoveDir(), -camRotation * Mathf.Deg2Rad);
            Vector3 motion = new Vector3(moveDir2D.x, 0f, moveDir2D.y) * moveSpeed * Time.deltaTime;

            ch.Move(motion);

            if (moveDir2D.sqrMagnitude != 0f) {
                targetRot = Vector2.SignedAngle(Vector2.up, moveDir2D);
                float lerpedRot = Mathf.LerpAngle(transform.rotation.eulerAngles.y, -targetRot, rotationSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lerpedRot, transform.rotation.eulerAngles.z);
            }
        }

        private Vector2 inputMoveDir()
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");

            return new Vector2(inputX, inputY).normalized;
        }
    }
}
