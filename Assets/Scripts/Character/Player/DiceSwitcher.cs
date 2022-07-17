using UnityEngine;
using UnityEngine.InputSystem;

public class DiceSwitcher : MonoBehaviour
{
    [SerializeField] private Transform selector;
    [SerializeField] private Transform[] switches;

    private int _currentIndex;
    public int CurrentValue => _currentIndex + 1;

    private Vector2 _targetSelectorPosition;

    private InputManager _inputManager;

    #region Unity Event

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle direction input
        _inputManager.Game.Direction.performed += DirectionOnPerformed;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    private void Start()
    {
        _targetSelectorPosition = selector.position;
    }

    private void FixedUpdate()
    {
        selector.position = Vector2.Lerp(selector.position, _targetSelectorPosition, 0.5f);
    }

    #endregion

    #region Input Methods

    private void DirectionOnPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.NotInProgress) return;
        InputTypeController.Instance.CheckInputType(context);

        var direction = context.ReadValue<Vector2>();

        if (direction.x < 0f) Select(_currentIndex == 0 ? 5 : _currentIndex - 1);
        else if (direction.x > 0f) Select(_currentIndex == 5 ? 0 : _currentIndex + 1);
    }

    #endregion

    private void Select(int index)
    {
        _targetSelectorPosition = switches[index].position;
        _currentIndex = index;
    }
}
