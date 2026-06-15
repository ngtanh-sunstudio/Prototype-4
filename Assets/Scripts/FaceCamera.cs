using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    void LateUpdate()
    {
        transform.position = player.position + offset;
        transform.rotation = Camera.main.transform.rotation;
    }
}
