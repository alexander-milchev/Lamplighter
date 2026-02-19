using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [Header("Drag and drop the door to connect to below")]
    [SerializeField] private GameObject door;

    public void DoorToggle()
    {
        door.SetActive(!door.activeInHierarchy);
    }
}
