using UnityEngine;

public class RestartGame : MonoBehaviour
{
    public void loadCurrentScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }
}
