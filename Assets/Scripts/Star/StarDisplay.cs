using UnityEngine;
using UnityEngine.UI;

public class StarDisplay : MonoBehaviour
{
    private Image[] _icons;

    #region Unity Event

    private void Awake()
    {
        _icons = GetComponentsInChildren<Image>();
    }

    #endregion

    public void UpdateDisplay(int star)
    {
        for (var i = 0; i < star; i++) _icons[i].gameObject.SetActive(true);
        for (var i = star; i < 3; i++) _icons[i].gameObject.SetActive(false);
    }
}
