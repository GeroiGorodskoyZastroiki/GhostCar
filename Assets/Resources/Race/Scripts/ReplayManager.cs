using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayManager : MonoBehaviour
{
    public static ReplayManager Instance { get; private set; }

    [SerializeField] private GameObject GhostPrefab;

    private List<Vector3> RecordedPositions = new List<Vector3>();
    private List<Quaternion> RecordedRotations = new List<Quaternion>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneReloaded;
        FindFirstObjectByType<TransformRecorder>().OnTransformRecorded += HandleTransformRecorded;
    }

    
    private void OnSceneReloaded(Scene scene, LoadSceneMode mode) 
    {
        FindFirstObjectByType<TransformRecorder>().OnTransformRecorded += HandleTransformRecorded;
        SpawnGhost();
    }

    private void SpawnGhost()
    {
        if (RecordedPositions.Count > 0 && RecordedRotations.Count > 0)
        {
            var ghost = Instantiate(GhostPrefab, RecordedPositions[0], RecordedRotations[0]);
            var ghostController = ghost.GetComponent<GhostController>();
            ghostController.Initialize(RecordedPositions, RecordedRotations);
        }
    }

    private void HandleTransformRecorded(List<Vector3> positions, List<Quaternion> rotations)
    {
        if (Mathf.Approximately(RaceManager.Instance.BestTime, RaceManager.Instance.CurrentTime))
        {
            RecordedPositions = new List<Vector3>(positions);
            RecordedRotations = new List<Quaternion>(rotations);
        }

        RaceManager.Instance.RestartRace();
    }
}
