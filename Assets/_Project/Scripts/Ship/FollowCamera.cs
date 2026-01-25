using UnityEngine;

namespace FallingStar.Ship
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(10f, 18f, 0f);
        [SerializeField] private float smooth = 10f;


        private void LateUpdate()
        {
            if (target == null)
            {
                Debug.LogError("Camera is missing a target. Please assign a target in the inspector before entering play mode.");
                return;
            }
            Vector3 desiered = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiered, smooth * Time.deltaTime);
            transform.LookAt(target.position);
        }
    }
}
