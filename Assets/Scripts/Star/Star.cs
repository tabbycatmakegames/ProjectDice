using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private ParticleSystem sparkPrefab;

    public void OnCollected()
    {
        Instantiate(sparkPrefab, transform.position, Quaternion.identity);
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        GameController.Instance.StartCoroutine(GameController.Instance.SlowMotionEffect());

        Destroy(gameObject);
    }
}
