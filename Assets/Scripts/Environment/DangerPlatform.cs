using UnityEngine;

public class DangerPlatform : Platform
{
    [SerializeField] private float damage;
    [SerializeField] private float knockBackForce;
    [SerializeField] private ParticleSystem bloodSplashPrefab;

    public override void Awake()
    {
        base.Awake();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            var point = other.contacts[0].point;
            var player = other.transform.GetComponent<Player>();
            var direction = ((Vector2)player.transform.position - point).normalized;

            player.KnockBack(direction, knockBackForce);
            player.TakeDamage(damage);

            Instantiate(bloodSplashPrefab, point, Quaternion.identity).transform.up = direction;
            CameraShaker.Instance.Shake(CameraShakeMode.Normal);
        }
    }
}
