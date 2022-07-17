using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }
    public Collider2D Collider2D { get; private set; }

    protected CharacterMovement _characterMovement;
    private CharacterResources _characterResources;

    private bool _isDead;
    [SerializeField] protected ParticleSystem explosionPrefab;

    public bool IsFlipped { get; private set; }
    public SpriteRenderer mainSprite;

    [Header("Ground Properties")]
    [SerializeField] private float groundRaycastDistance = 0.51f;
    public virtual bool IsGrounded { get; protected set; }

    #region Unity Event

    public virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();

        _characterMovement = GetComponent<CharacterMovement>();
        _characterResources = GetComponent<CharacterResources>();
    }

    public virtual void Start()
    {
    }

    public virtual void FixedUpdate()
    {
        if (transform.position.y <= -20f) Die();
    }

    #endregion

    #region Damage & Death

    public virtual void TakeDamage(float damage)
    {
        _characterResources.Health -= damage;
    }

    public virtual void Die()
    {
        if (_isDead) return;

        _isDead = true;

        if (explosionPrefab) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    #endregion

    public virtual void SetFlipped(bool value)
    {
        mainSprite.transform.localScale = new Vector2(value ? -1f : 1f, 1f);
        IsFlipped = value;
    }

    public virtual void KnockBack(Vector2 direction, float force)
    {
        if (!Rigidbody2D) return;
        if (_characterMovement) _characterMovement.StopRunningImmediate();

        Rigidbody2D.velocity = Vector2.zero;
        Rigidbody2D.AddForce(direction * force, ForceMode2D.Impulse);
    }

    protected virtual void DetectGrounded()
    {
        IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance);
    }
}
