using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : CharacterMovement
{
    [Header("Jump Properties")]
    [SerializeField] private ParticleSystem jumpMuzzle;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private int jumpRate = 4;
    [SerializeField] private int maxJumps;
    private Timer _jumpTimer;
    private bool _canJump;
    private int _currentJump;
    
    private Player _player;

    private InputManager _inputManager;

    #region Unity Event

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle movement input
        _inputManager.Player.Move.performed += MoveOnPerformed;
        _inputManager.Player.Move.canceled += MoveOnCanceled;

        // Handle jump input
        _inputManager.Player.Jump.performed += JumpOnPerformed;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    public override void Awake()
    {
        base.Awake();

        _player = GetComponent<Player>();
    }

    public override void Start()
    {
        base.Start();

        _jumpTimer = new Timer(1f / jumpRate);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_jumpTimer.IsReached()) _canJump = true;
    }

    #endregion

    #region Input Methods

    private void MoveOnPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.NotInProgress || !_player.IsControllable) return;
        InputTypeController.Instance.CheckInputType(context);

        var direction = context.ReadValue<Vector2>().normalized * (PlayerPrefs.GetInt("InvertMovement", 0) == 0 ? 1f : -1f);

        if (direction.x < 0f) direction = Vector2.left;
        else if (direction.x > 0f) direction = Vector2.right;
        else
        {
            StopRunning();
            return;
        }

        if (direction.x * CurrentDirection.x < 0f) StopRunningImmediate();
        StartRunning(direction);
    }

    private void MoveOnCanceled(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.NotInProgress || !_player.IsControllable) return;
        InputTypeController.Instance.CheckInputType(context);

        StopRunning();
    }

    private void JumpOnPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.NotInProgress || !_player.IsControllable) return;
        InputTypeController.Instance.CheckInputType(context);

        Jump();
    }

    #endregion

    private void Jump()
    {
        if (!_canJump || _currentJump >= maxJumps) return;

        _player.Rigidbody2D.velocity = Vector2.zero;
        _player.Rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        Instantiate(jumpMuzzle, (Vector2)transform.position - Vector2.up * 0.5f, Quaternion.identity);
        CameraShaker.Instance.Shake(CameraShakeMode.Micro);

        _jumpTimer = new Timer(1f / jumpRate);
        _canJump = false;
        _currentJump++;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_player.IsGrounded) _currentJump = 0;
    }
}
