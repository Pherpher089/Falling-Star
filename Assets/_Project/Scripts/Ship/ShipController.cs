using FallingStar.GameStates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FallingStar.Ship
{
    [RequireComponent(typeof(Rigidbody))]
    public class ShipController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float thrust = 18f;
        [SerializeField] private float turnTorque = 12f;
        [SerializeField] private float breakLinearDamping = 2.5f;
        [SerializeField] private float breakAngularDamping = 4.0f;


        [Header("Space Feel")]
        [SerializeField, Range(0f, 5f)] private float linearDamping = 0.4f;
        [SerializeField, Range(0f, 5f)] private float angularDamping = 1.0f;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
        }

        private void FixedUpdate()
        {
            GameStateManager gsm = GameStateManager.Instance;
            if (gsm != null && gsm.CurrentState != GameStateId.Expedition) return;

            float forward = 0f;
            float turn = 0f;
            bool isBreaking = false;

            Keyboard keyboard = Keyboard.current;
            if (keyboard != null)
            {
                if (keyboard.wKey.isPressed) forward += 1f;
                if (keyboard.sKey.isPressed) forward -= 1f;

                if (keyboard.aKey.isPressed) turn -= 1f;
                if (keyboard.dKey.isPressed) turn += 1f;

                isBreaking = keyboard.spaceKey.isPressed;
            }

            // Breakding



            // Thrust forward in the ship's facing direction
            if (Mathf.Abs(forward) > 0.01f)
            {
                Vector3 force = transform.forward * (forward * thrust);
                rb.AddForce(force, ForceMode.Acceleration);
            }

            if (Mathf.Abs(turn) > 0.01f)
            {
                Vector3 torque = Vector3.up * (turn * turnTorque);
                rb.AddTorque(torque, ForceMode.Acceleration);
            }

            float ld = isBreaking ? breakLinearDamping : linearDamping;
            float ad = isBreaking ? breakAngularDamping : angularDamping;

            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, ld * Time.fixedDeltaTime);

            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, ad * Time.fixedDeltaTime);
        }
    }
}
