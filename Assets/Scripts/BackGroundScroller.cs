using UnityEngine;

public class BackGroundScroller : MonoBehaviour
{
    private Vector2 startPos;
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallaxEffect;

    private void Start()
    {
        startPos = transform.position; 
    }

    private void LateUpdate()
    {
        Vector2 distance = cam.transform.position * parallaxEffect;

        transform.position = new Vector3(startPos.x + distance.x, startPos.y + distance.y, transform.position.z);
    }
}
