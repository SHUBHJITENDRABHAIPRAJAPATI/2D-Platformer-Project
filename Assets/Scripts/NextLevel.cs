using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public string nextLevelName;
    public void loadNextLevel()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevelName);
       
    }
}
