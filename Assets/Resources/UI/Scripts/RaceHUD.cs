using UnityEngine;
using TMPro;

public class RaceHUD : MonoBehaviour
{
    public TMP_Text lap;
    public TMP_Text bestTime;
    public TMP_Text currentTime;

    void Update()
    {
        lap.text = "Lap: " + RaceManager.Instance.Lap;
        bestTime.text = "Best Time: " + RaceManager.Instance.BestTime;
        currentTime.text = "Current Time: " + RaceManager.Instance.CurrentTime;
    }
}
