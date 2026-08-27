using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ZoneEntrance : MonoBehaviour
{
    [SerializeField] private string pointId;
    [SerializeField] private string targetScene;
    [SerializeField] private string targetPointId;
    [Tooltip("Where the player is placed when arriving through this entrance. " +
             "Keep it off the trigger itself, otherwise arriving here immediately " +
             "sends the player back where they came from.")]
    [SerializeField] private Transform spawnPoint;

    public string PointId => pointId;
    public Vector3 SpawnPosition => spawnPoint != null ? spawnPoint.position : transform.position;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<PlayerInput>(out _))
        {
            return;
        }

        ZoneManager.Instance.Travel(targetScene, targetPointId);
    }
}
