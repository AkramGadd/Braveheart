using UnityEngine;
using Assets.Scripts; // <-- This fixes the missing reference

public class Boss : MonoBehaviour
{
    private Damageable damageable;
    private bool timerStopped = false;

    void Start()
    {
        damageable = GetComponent<Damageable>();
    }

    void Update()
    {
        if (!timerStopped && damageable != null && damageable.Health <= 0)
        {
            float finalTime = FindObjectOfType<Timer>().GetElapsedTime();
            NetworkManager.Instance.UpdateScoreIfBetter(finalTime);
            FindObjectOfType<Timer>().StopTimer();
            timerStopped = true;
            NetworkManager.Instance.UpdateScoreIfBetter(finalTime);
            Overlays loader = FindObjectOfType<Overlays>();
            loader.LevelFinished();
        }
    }
}
