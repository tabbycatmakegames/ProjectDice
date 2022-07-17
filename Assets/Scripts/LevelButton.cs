using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int levelNum;
    private Image _layer;

    private StarDisplay _miniStarDisplay;

    #region Unity Event

    private void Awake()
    {
        _layer = GetComponentsInChildren<Image>(true)[1];
        _miniStarDisplay = GetComponentInChildren<StarDisplay>();
    }

    private void Start()
    {
        var levelProgress = PlayerPrefs.GetInt(Keys.LevelProgress, 1);
        _layer.gameObject.SetActive(levelNum > levelProgress);

        _miniStarDisplay.UpdateDisplay(PlayerPrefs.GetInt($"{Keys.LevelStar}{levelNum}", 0));
    }

    #endregion
}
