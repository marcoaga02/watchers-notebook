using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreatureMover))]
[RequireComponent(typeof(PlayerInput))]
public class PossessionController : MonoBehaviour
{
    [SerializeField] private float spawnDistance = 1f;
    [SerializeField] private KeyCode cancelKey = KeyCode.Escape;

    private CreatureMover _playerMover;
    private PlayerInput _playerInput;
    private CreatureMover _possessed;
    private bool _hasLeftGround;

    public bool IsPossessing => _possessed != null;

    private void Awake()
    {
        _playerMover = GetComponent<CreatureMover>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (_possessed == null)
        {
            return;
        }

        if (Input.GetKeyDown(cancelKey))
        {
            Return();
            return;
        }

        var required = TerrainProbe.Instance.GetRequiredCapability(_possessed.transform.position);
        if (required != null)
        {
            _hasLeftGround = true;
        }
        else if (_hasLeftGround)
        {
            Return();
        }
    }

    public void Evoke(GameObject creaturePrefab, IEnumerable<KeyValuePair<CapabilitySigil, Behavior>> bindings)
    {
        if (_possessed != null)
        {
            return;
        }

        var spawnPosition = _playerMover.transform.position + (Vector3)(_playerMover.Facing * spawnDistance);
        var instance = Instantiate(creaturePrefab, spawnPosition, Quaternion.identity);
        _possessed = instance.GetComponent<CreatureMover>();
        _hasLeftGround = false;

        if (bindings != null)
        {
            var creature = instance.GetComponent<Creature>();
            foreach (var binding in bindings)
            {
                creature.Bind(binding.Key, binding.Value);
            }
        }

        _playerMover.SetControlEnabled(false);
        _playerInput.SetTarget(_possessed);
        CameraFollow.Instance.target = _possessed.transform;
    }

    private void Return()
    {
        // TODO: play the Death animation on the possessed instance before destroying it,
        // once the clip is wired up.
        var returnPosition = _possessed.transform.position;
        Destroy(_possessed.gameObject);
        _possessed = null;

        _playerMover.transform.position = returnPosition;
        _playerMover.SetControlEnabled(true);
        _playerInput.SetTarget(_playerMover);
        CameraFollow.Instance.target = _playerMover.transform;
    }
}
