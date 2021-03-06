using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    #region Singleton

    private static GameController _gameControllerInstance;

    public static GameController Instance
    {
        get
        {
            if (_gameControllerInstance == null) _gameControllerInstance = FindObjectOfType<GameController>();
            return _gameControllerInstance;
        }
    }

    #endregion

    public GameState State { get; private set; } = GameState.InProgress;

    [Header("Menus")][SerializeField] private Canvas mainUI;
    [SerializeField] private Menu pauseMenu;
    [SerializeField] private Menu gameOverMenu;
    [SerializeField] private Menu levelCompleteMenu;

    private InputManager _inputManager;

    #region Input Methods

    private void EscapeOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);

        if (State == GameState.InProgress) Pause();
    }

    #endregion

    #region Unity Event

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle game pause input
        _inputManager.Game.Escape.performed += EscapeOnPerformed;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    private void Start()
    {
        EffectsController.Instance.SetDepthOfField(false);
        EffectsController.Instance.SetChromaticAberration(false);
        EffectsController.Instance.SetVignetteIntensity(EffectsController.DefaultVignetteIntensity);

        mainUI.gameObject.SetActive(true);
        SetCursorEnabled(false);
    }

    #endregion

    private static void SetCursorEnabled(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = value;
    }

    #region Game State Methods

    public void Pause()
    {
        Time.timeScale = 0f;
        State = GameState.NotInProgress;
        pauseMenu.SetActive(true);

        EffectsController.Instance.SetDepthOfField(true);
        SetCursorEnabled(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        State = GameState.InProgress;
        pauseMenu.SetActive(false);

        EffectsController.Instance.SetDepthOfField(false);
        SetCursorEnabled(false);
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);

        State = GameState.NotInProgress;
        gameOverMenu.SetActive(true);

        EffectsController.Instance.SetDepthOfField(true);
        SetCursorEnabled(true);
    }

    public IEnumerator LevelComplete()
    {
        yield return new WaitForSeconds(1f);

        State = GameState.NotInProgress;
        levelCompleteMenu.SetActive(true);

        // Save level progress
        var levelProgress = PlayerPrefs.GetInt(Keys.LevelProgress, 1);
        var currentLevel = SceneLoader.Instance.CurrentLevelNum();
        if (levelProgress <= currentLevel) PlayerPrefs.SetInt(Keys.LevelProgress, currentLevel + 1);

        // Save level star
        var levelStar = PlayerPrefs.GetInt($"{Keys.LevelStar}{SceneLoader.Instance.CurrentLevelNum()}", 0);
        var collectedStars = Player.Instance.CollectedStars;
        if (collectedStars > levelStar) PlayerPrefs.SetInt($"{Keys.LevelStar}{SceneLoader.Instance.CurrentLevelNum()}", collectedStars);

        EffectsController.Instance.SetDepthOfField(true);
        SetCursorEnabled(true);
    }

    #endregion

    public IEnumerator SlowMotionEffect(float scale = 0.5f, float duration = 0.25f)
    {
        // Slow down
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        EffectsController.Instance.SetChromaticAberration(true);
        EffectsController.Instance.SetVignetteIntensity(EffectsController.DefaultVignetteIntensity + 0.1f);

        yield return new WaitForSeconds(duration);

        // Back to normal
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        EffectsController.Instance.SetChromaticAberration(false);
        EffectsController.Instance.SetVignetteIntensity(EffectsController.DefaultVignetteIntensity);
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}