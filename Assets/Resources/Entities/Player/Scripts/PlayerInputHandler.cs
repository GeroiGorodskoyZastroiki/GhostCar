using UnityEngine;
using UnityEngine.InputSystem;
using Ashsvp;

public class PlayerInputHandler : MonoBehaviour
{
    public SimcadeVehicleController Controller;

    private void Start() => // Получаем контроллер машины
        Controller = GetComponent<SimcadeVehicleController>(); 

    public void OnDrive(InputAction.CallbackContext context)
    {
        // Если гонка не активна, ничего не делаем
        if (!IsRaceActive()) return;

        // Передаем значения ускорения и поворота в контроллер
        Controller.RawAccelerationInput = context.ReadValue<Vector2>().y;
        Controller.RawSteerInput = context.ReadValue<Vector2>().x;
    }

    public void OnBrake(InputAction.CallbackContext context)
    {
        // Если гонка не активна, ничего не делаем
        if (!IsRaceActive()) return;

        // Устанавливаем значение торможения
        Controller.RawBrakeInput = context.performed ? 1 : 0;
    }

    private bool IsRaceActive() => // Проверяем, активна ли гонка
        RaceManager.Instance != null && RaceManager.Instance.RaceInProgress;
}

