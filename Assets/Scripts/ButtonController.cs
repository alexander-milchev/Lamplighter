using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [Header("Drag and drop the door(s) to connect to below")]
    [SerializeField] private GameObject[] doors;
    private Color onColor = new Color(190f/255f, 148f/255f, 39f/255f, 1);
    private Color offColor = new Color(1, 1, 1, 1);
    private bool isOn = false;

    public void DoorToggle()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(!door.activeInHierarchy);
        }
        isOn = !isOn;
        ToggleColour();
    }

    private void ToggleColour()
    {
        if (isOn)
        {
            gameObject.GetComponent<SpriteRenderer>().color = onColor;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = offColor;
        }
    }
}
