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
        // Y는 고정, X만 따라가기
        Vector3 pos = transform.position;
        pos.x = cam.position.x;
        transform.position = pos;
    }
}
