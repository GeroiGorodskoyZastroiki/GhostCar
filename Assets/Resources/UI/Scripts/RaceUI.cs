using UnityEngine;

public class RaceUI : MonoBehaviour
{
    public void StartRace()
    {
        RaceManager.Instance.StartRace();
        gameObject.SetActive(false);
    }
}
