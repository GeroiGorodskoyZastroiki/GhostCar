using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    // Событие, которое вызывается, когда игрок достигает контрольной точки.
    // Передаёт текущую контрольную точку как аргумент.
    public UnityAction<Checkpoint> OnCheckpointReached;

    // Метод, вызываемый при въезде в чекпоинт.
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, принадлежит ли объект с триггером игроку.
        if (other.transform.root.CompareTag("Player"))
            // Если игрок достиг контрольной точки, вызываем событие OnCheckpointReached.
            OnCheckpointReached?.Invoke(this);
    }
}
