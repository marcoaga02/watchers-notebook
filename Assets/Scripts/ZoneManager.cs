using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Instance { get; private set; }

    [SerializeField] private string initialZoneScene;
    [SerializeField] private string initialPointId;
    [SerializeField] private Transform player;

    private string _currentZoneScene;
    private bool _isTravelling;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Travel(initialZoneScene, initialPointId);
    }

    public void Travel(string targetScene, string targetPointId)
    {
        if (_isTravelling)
        {
            return;
        }

        StartCoroutine(TravelRoutine(targetScene, targetPointId));
    }

    private IEnumerator TravelRoutine(string targetScene, string targetPointId)
    {
        _isTravelling = true;

        if (!string.IsNullOrEmpty(_currentZoneScene))
        {
            yield return SceneManager.UnloadSceneAsync(_currentZoneScene);
        }

        yield return SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
        _currentZoneScene = targetScene;

        var entrance = FindEntrance(targetPointId);
        if (entrance != null)
        {
            player.position = entrance.SpawnPosition;
        }

        _isTravelling = false;
    }

    private ZoneEntrance FindEntrance(string pointId)
    {
        foreach (var entrance in FindObjectsByType<ZoneEntrance>(FindObjectsSortMode.None))
        {
            if (entrance.PointId == pointId)
            {
                return entrance;
            }
        }

        return null;
    }
}
