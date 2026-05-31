using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("目标")]
    public LaserDoor[] targets;

    [Header("事件")]
    public UnityEvent onAllClear;

    private bool isCleared;

    void Start()
    {
        if (targets == null || targets.Length == 0)
            targets = FindObjectsByType<LaserDoor>(FindObjectsSortMode.None);
    }

    void Update()
    {
        if (isCleared) return;

        foreach (LaserDoor door in targets)
        {
            if (!door.isComplete)
                return;
        }

        isCleared = true;
        onAllClear?.Invoke();
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
