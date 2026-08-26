using UnityEngine;

[RequireComponent(typeof(CreatureMover))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    private CreatureMover _playerMover;
    private CreatureMover _target;

    private void Awake()
    {
        _playerMover = GetComponent<CreatureMover>();
        _target = _playerMover;
    }

    public void SetTarget(CreatureMover mover)
    {
        _target = mover;
    }

    private void Update()
    {
        if (PanelManager.Instance.IsAnyPanelOpen)
        {
            _target.SetInput(Vector2.zero);
            return;
        }

        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _target.SetInput(input);

        // sprint is a Man-only trait, never applies while controlling a possessed creature
        if (_target == _playerMover)
        {
            _target.SetSprinting(Input.GetKey(sprintKey));
        }
    }
}
