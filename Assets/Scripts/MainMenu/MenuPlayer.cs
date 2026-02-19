using UnityEngine;

public class MenuPlayer : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        PlayerHealth.instance.isDead = true;
    }
}
