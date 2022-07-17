using UnityEngine;

public class Portal : MonoBehaviour
{
    public void OnEntered()
    {
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        GameController.Instance.StartCoroutine(GameController.Instance.LevelComplete());
        GameController.Instance.StartCoroutine(GameController.Instance.SlowMotionEffect());
    }
}
