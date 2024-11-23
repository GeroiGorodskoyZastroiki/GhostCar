using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour
{
    // Синглтон, предоставляющий глобальный доступ к RaceManager
    public static RaceManager Instance { get; private set; }

    // Текущий круг, по умолчанию равен 0
    public sbyte Lap { get; private set; } = 0;

    // Текущее время гонки, округленное до двух знаков
    public float CurrentTime => Mathf.Round(_currentTime * 100f) / 100f;

    // Лучшее время гонки, округленное до двух знаков
    public float BestTime => Mathf.Round(_bestTime * 100f) / 100f;

    // Флаг, указывающий, идет ли гонка
    public bool RaceInProgress { get; private set; } = false;

    private List<Checkpoint> _checkpoints; // Список всех чекпоинтов на сцене
    private int _currentCheckpointIndex = 0; // Индекс текущего чекпоинта
    private float _currentTime; // Время текущей гонки
    private float _bestTime = 0f; // Лучшее время гонки

     // Событие, вызываемое при начале гонки
    public UnityAction OnRaceStarted;
    
    // Событие, вызываемое при окончании гонки
    public UnityAction OnRaceEnded;

    private void Awake()
    {
        // Реализуем паттерн Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Уничтожаем дубликат
            return;
        }
        Instance = this; // Устанавливаем текущий объект как Instance
        DontDestroyOnLoad(gameObject); // Не уничтожаем объект при загрузке новой сцены
    }

    private void Start()
    {
        InitializeCheckpoints(); // Инициализируем список чекпоинтов
    }

    private void Update()
    {
        // Увеличиваем время только если гонка активна
        if (RaceInProgress)
            _currentTime += Time.deltaTime; // Добавляем время, прошедшее с предыдущего кадра
    }

    public void StartRace()
    {
        _currentTime = 0f; // Сбрасываем текущее время
        _currentCheckpointIndex = 0; // Сбрасываем индекс чекпоинта
        RaceInProgress = true; // Устанавливаем флаг начала гонки
        ShowCheckpoint(_currentCheckpointIndex); // Показываем первый чекпоинт
        OnRaceStarted?.Invoke(); // Вызываем событие начала гонки
    }

    public void EndRace()
    {
        // Если текущее время лучше предыдущего лучшего или лучший результат отсутствует, обновляем его
        if (CurrentTime < BestTime || BestTime == 0)
            _bestTime = CurrentTime;

        RaceInProgress = false; // Завершаем гонку
        OnRaceEnded?.Invoke(); // Вызываем событие окончания гонки
    }

    public void RestartRace()
    {
        // Перезагружаем текущую сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.sceneLoaded += OnSceneReloaded; // Подписываемся на событие загрузки сцены
    }

    private void OnSceneReloaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneReloaded; // Отписываемся от события
        InitializeCheckpoints(); // Инициализируем чекпоинты заново
        _currentTime = 0f; // Сбрасываем текущее время
        Lap++; // Увеличиваем номер круга
    }

    private void InitializeCheckpoints()
    {
        // Находим все чекпоинты на сцене и сортируем их по имени
        _checkpoints = FindObjectsOfType<Checkpoint>().OrderBy(c => c.name).ToList();

        // Отключаем все чекпоинты и подписываемся на их события
        foreach (var checkpoint in _checkpoints)
        {
            checkpoint.gameObject.SetActive(false); // Деактивируем чекпоинт
            checkpoint.OnCheckpointReached += CheckpointReached; // Подписываемся на событие пересечения чекпоинта
        }
    }

    private void CheckpointReached(Checkpoint checkpoint)
    {
        HideCheckpoint(_currentCheckpointIndex); // Скрываем текущий чекпоинт
        _currentCheckpointIndex++; // Переходим к следующему чекпоинту

        if (_currentCheckpointIndex < _checkpoints.Count)
            ShowCheckpoint(_currentCheckpointIndex); // Показываем следующий чекпоинт
        else
            EndRace(); // Если чекпоинтов больше нет, завершаем гонку
    }

    private void ShowCheckpoint(int index) =>
            _checkpoints[index].gameObject.SetActive(true); // Активируем чекпоинт

    private void HideCheckpoint(int index) =>
            _checkpoints[index].gameObject.SetActive(false); // Деактивируем чекпоинт
}
