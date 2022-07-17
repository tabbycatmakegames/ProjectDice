using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    #region Singleton

    private static Player _playerInstance;

    public static Player Instance
    {
        get
        {
            if (_playerInstance == null) _playerInstance = FindObjectOfType<Player>();
            return _playerInstance;
        }
    }

    #endregion

    public PlayerCombat PlayerCombat { get; set; }

    public bool IsControllable { get; set; } = true;

    [Header("Dice Properties")]
    [SerializeField] private SpriteRenderer diceFace;
    [SerializeField] private Sprite[] diceSprites;
    public int CurrentDice { get; set; }
    [SerializeField] private DiceSwitcher diceSwitcher;

    private Portal _portal;
    private static readonly int EnterPortalAnimationTrigger = Animator.StringToHash("enterPortal");

    public int CollectedStars { get; set; }
    private StarDisplay _starDisplay;

    private InputManager _inputManager;

    #region Unity Events

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle dice switching input
        _inputManager.Player.Switch.started += SwitchOnStarted;
        _inputManager.Player.Switch.canceled += SwitchOnCanceled;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    public override void Awake()
    {
        base.Awake();

        PlayerCombat = GetComponent<PlayerCombat>();

        _starDisplay = FindObjectOfType<StarDisplay>(true);
    }

    public override void Start()
    {
        base.Start();

        SetDice(1);
        diceSwitcher.gameObject.SetActive(false);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        DetectGrounded();
        if (_portal) transform.position = Vector2.Lerp(transform.position, _portal.transform.position, 0.5f);
    }

    #endregion

    #region Input Methods

    private void SwitchOnStarted(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.NotInProgress) return;
        InputTypeController.Instance.CheckInputType(context);

        SetSwitchMode(true);
    }

    private void SwitchOnCanceled(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.NotInProgress) return;
        InputTypeController.Instance.CheckInputType(context);

        SetSwitchMode(false);
        SetDice(diceSwitcher.CurrentValue);
    }

    #endregion

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }

    public override void Die()
    {
        base.Die();

        CameraShaker.Instance.Shake(CameraShakeMode.Normal);
        GameController.Instance.StartCoroutine(GameController.Instance.SlowMotionEffect());
        GameController.Instance.StartCoroutine(GameController.Instance.GameOver());
    }

    public void SetDice(int diceNumber)
    {
        CurrentDice = diceNumber;
        diceFace.sprite = diceSprites[diceNumber - 1];
    }

    private void SetSwitchMode(bool value)
    {
        GameController.Instance.SetTimeScale(value ? 0.05f : 1f);
        EffectsController.Instance.SetDepthOfField(value);
        diceSwitcher.gameObject.SetActive(value);

        IsControllable = !value;
    }

    public void EnterPortal(Portal portal)
    {
        _characterMovement.StopRunningImmediate();
        IsControllable = false;
        Animator.SetTrigger(EnterPortalAnimationTrigger);
        Collider2D.enabled = false;

        _portal = portal;
    }

    public void CollectStar(Star star)
    {
        CollectedStars++;
        _starDisplay.UpdateDisplay(CollectedStars);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {
            var portal = other.GetComponent<Portal>();

            EnterPortal(portal);
            portal.OnEntered();
        }
        else if (other.CompareTag("Star"))
        {
            var star = other.GetComponent<Star>();

            CollectStar(star);
            star.OnCollected();
        }
    }
}
