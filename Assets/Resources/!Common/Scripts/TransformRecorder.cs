using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class TransformRecorder : MonoBehaviour
{
    // Список для хранения записанных позиций Transform
    private List<Vector3> _recordedPositions = new List<Vector3>();

    // Список для хранения записанных вращений Transform
    private List<Quaternion> _recordedRotations = new List<Quaternion>();

    // Флаг, указывающий, идет ли запись
    private bool _recording = false;

    // Событие, которое вызывается при завершении записи, передает списки позиций и вращений
    public UnityAction<List<Vector3>, List<Quaternion>> OnTransformRecorded;

    // Подписываемся на события начала и конца гонки из RaceManager
    private void Start()
    {
        RaceManager.Instance.OnRaceStarted += StartRecording;
        RaceManager.Instance.OnRaceEnded += StopRecording;
    }

    // Отписываемся от событий, чтобы избежать утечек памяти, когда объект отключен
    private void OnDisable()
    {
        RaceManager.Instance.OnRaceStarted -= StartRecording;
        RaceManager.Instance.OnRaceEnded -= StopRecording;
    }

    // В процессе записи добавляем текущую позицию и вращение Transform в соответствующие списки
    private void Update()
    {
        if (_recording)
        {
            _recordedPositions.Add(transform.position); // Записываем текущую позицию
            _recordedRotations.Add(transform.rotation); // Записываем текущее вращение
        }
    }

    // Начинаем запись данных Transform, очищая предыдущие записи
    private void StartRecording()
    {
        _recording = true;
        _recordedPositions.Clear(); // Очищаем список позиций
        _recordedRotations.Clear(); // Очищаем список вращений
    }

    // Завершаем запись данных Transform и вызываем событие с записанными данными
    private void StopRecording()
    {
        _recording = false;
        OnTransformRecorded?.Invoke(_recordedPositions, _recordedRotations); // Вызываем событие
    }
}
