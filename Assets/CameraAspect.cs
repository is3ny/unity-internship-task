using UnityEngine;

public class CameraAspect : MonoBehaviour
{
    public float width = 834f;
    public float height = 340f;

    void Awake ()
    {
        Camera.main.aspect = width / height;
    }
}
