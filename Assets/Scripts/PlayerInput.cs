using UnityEngine;

[RequireComponent(typeof(CreatureMover))]
public class PlayerInput : MonoBehaviour
{
    private CreatureMover _target;

    private void Awake()
    {
        _target = GetComponent<CreatureMover>();
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
    }
}
