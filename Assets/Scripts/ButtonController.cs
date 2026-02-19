using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [Header("Drag and drop the door(s) to connect to below")]
    [SerializeField] private GameObject[] doors;

    public void DoorToggle()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(!door.activeInHierarchy);
        }
    }
}
