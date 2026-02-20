using UnityEngine;
using UnityEngine.SceneManagement;

public class UnfinishedLevels : MonoBehaviour
{
    [SerializeField] private GameObject previous;
    private int lvl1 = 2;
    private int lvl2 = 3;
    private int lvl3 = 4;

    public void Back()
    {
        previous.SetActive(true);
        gameObject.SetActive(false);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(sceneBuildIndex: lvl1);
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene(sceneBuildIndex: lvl2);
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene(sceneBuildIndex: lvl3);
    }
}
