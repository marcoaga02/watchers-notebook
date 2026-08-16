using UnityEngine;

[RequireComponent(typeof(CreatureMover))]
[RequireComponent(typeof(PlayerInput))]
public class PossessionController : MonoBehaviour
{
    [SerializeField] private float spawnDistance = 1f;

    // TODO: remove this field, testCreaturePrefab and the Input.GetKeyDown check
    // in Update() once the real evocation panel exists; Evoke() will be called
    // only from the panel button, with the creature chosen there.
    [Header("Test evocation (remove once the panel exists)")]
    [SerializeField] private KeyCode testEvokeKey = KeyCode.E;
    [SerializeField] private GameObject testCreaturePrefab;

    private CreatureMover _playerMover;
    private PlayerInput _playerInput;
    private CreatureMover _possessed;
    private bool _hasLeftGround;

    private void Awake()
    {
        _playerMover = GetComponent<CreatureMover>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (_possessed == null)
        {
            if (testCreaturePrefab != null && Input.GetKeyDown(testEvokeKey))
            {
                Evoke(testCreaturePrefab);
            }
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

    public void Evoke(GameObject creaturePrefab)
    {
        if (_possessed != null)
        {
            return;
        }

        var spawnPosition = _playerMover.transform.position + (Vector3)(_playerMover.Facing * spawnDistance);
        var instance = Instantiate(creaturePrefab, spawnPosition, Quaternion.identity);
        _possessed = instance.GetComponent<CreatureMover>();
        _hasLeftGround = false;

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
