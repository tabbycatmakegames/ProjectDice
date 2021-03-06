using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    #region Singleton

    private static SceneLoader _sceneLoaderInstance;

    public static SceneLoader Instance
    {
        get
        {
            if (_sceneLoaderInstance == null) _sceneLoaderInstance = FindObjectOfType<SceneLoader>();
            return _sceneLoaderInstance;
        }
    }

    #endregion

    private Animator _cameraAnimator;
    private static readonly int OutroTrigger = Animator.StringToHash("outro");
    [SerializeField] private AnimationClip cameraOutroAnimationClip;

    private string _sceneToLoad = "";

    #region Unity Event

    private void Awake()
    {
        if (Camera.main is { }) _cameraAnimator = Camera.main.GetComponent<Animator>();
    }

    #endregion

    public int CurrentLevelNum()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        var sceneNum = sceneName.Substring(5, 2);
        return int.Parse(sceneNum);
    }

    private IEnumerator Load()
    {
        // Load scene in background but don't allow transition
        var asyncOperation = SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;

        // Play camera animation
        _cameraAnimator.SetTrigger(OutroTrigger);

        // Wait for camera animation to complete
        yield return new WaitForSeconds(cameraOutroAnimationClip.averageDuration);

        // Allow transition to new scene
        asyncOperation.allowSceneActivation = true;
    }

    public void Load(string scene)
    {
        Time.timeScale = 1f;
        _sceneToLoad = scene;
        StartCoroutine(Load());
    }

    public void LoadLevel(int levelNum)
    {
        var levelProgress = PlayerPrefs.GetInt(Keys.LevelProgress, 1);
        if (levelProgress < levelNum) return;

        Load($"Level0{levelNum}");
    }

    public void LoadNextLevel()
    {
        var nextLevelNum = CurrentLevelNum() + 1;
        if (nextLevelNum > 5) return;

        LoadLevel(nextLevelNum);
    }

    public void Restart()
    {
        // Reload current active scene
        Load(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}