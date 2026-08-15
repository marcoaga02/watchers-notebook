using UnityEngine;

[RequireComponent(typeof(CreatureMover))]
public class PlayerInput : MonoBehaviour
{
    private CreatureMover _mover;

    private void Awake()
    {
        _mover = GetComponent<CreatureMover>();
    }

    private void Update()
    {
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _mover.SetInput(input);
    }
}
