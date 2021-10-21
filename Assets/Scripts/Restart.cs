using UnityEngine.SceneManagement;

public static class Restart
{
    public static void RestartLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
