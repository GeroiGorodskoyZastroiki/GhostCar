using UnityEngine;
using System.Collections.Generic;

public class GhostController : MonoBehaviour
{
    // Список для хранения записанных позиций призрака
    public List<Vector3> RecordedPositions;
    
    // Список для хранения записанных вращений призрака
    public List<Quaternion> RecordedRotations;
    
    // Индекс текущей позиции в списке для воспроизведения
    private int _currentIndex = 0;
    
    // Флаг, указывающий, воспроизводится ли движение призрака
    private bool _replaying = false;

    // Метод инициализации с передачей записанных позиций и вращений
    public void Initialize(List<Vector3> positions, List<Quaternion> rotations)
    {
        RecordedPositions = positions;
        RecordedRotations = rotations;
    }

    // Метод, который подписывается на событие начала гонки
    private void Start() =>
        RaceManager.Instance.OnRaceStarted += StartReplay;

    // Отписка от события при отключении компонента
    private void OnDisable() =>
        RaceManager.Instance.OnRaceStarted -= StartReplay;

    // Метод, который запускает воспроизведение движений призрака
    private void StartReplay()
    {
        _replaying = true;
        _currentIndex = 0;
    }

    // Обновление позиции и вращения призрака в каждом кадре во время воспроизведения
    private void Update()
    {
        if (_replaying)
        {
            if (_currentIndex < RecordedPositions.Count)
            {
                // Устанавливаем позицию и вращение призрака на основе текущего индекса
                transform.position = RecordedPositions[_currentIndex];
                transform.rotation = RecordedRotations[_currentIndex];
                
                // Увеличиваем индекс для следующего кадра
                _currentIndex++;
            }
            else Destroy(gameObject); // Путь пройден, уничтожаем призрака
        }
    }
}
