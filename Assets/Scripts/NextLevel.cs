using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public string nextLevelName;
    public int nextLevelValue;
    public void loadNextLevel()
    {
        PlayerPrefs.SetInt("LevelReached", nextLevelValue);
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevelName);
       
    }
}
