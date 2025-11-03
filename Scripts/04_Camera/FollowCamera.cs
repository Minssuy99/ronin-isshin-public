using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = cam.position.x;
        transform.position = pos;
    }
}
