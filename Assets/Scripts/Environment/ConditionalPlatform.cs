using UnityEngine;

public class ConditionalPlatform : Platform
{
    private Conditioner _conditioner;

    [SerializeField] private Color solidColor;
    [SerializeField] private Color transparentColor;
    private SpriteRenderer _sprite;

    private Collider2D _collider;
    private int _collideLayer;
    private int _nonCollideLayer;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();
        
        _conditioner = GetComponent<Conditioner>();
        _collider = GetComponent<Collider2D>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        _collideLayer = LayerMask.NameToLayer("PlayerCollide");
        _nonCollideLayer = LayerMask.NameToLayer("PlayerNonCollide");
    }

    private void FixedUpdate()
    {
        if (!Player.Instance) return;
        
        var eval = _conditioner.Evaluate(Player.Instance.CurrentDice);
        // _collider.enabled = eval;
        gameObject.layer = eval ? _collideLayer : _nonCollideLayer;
        _sprite.color = eval ? solidColor : transparentColor;
    }

    #endregion
}
