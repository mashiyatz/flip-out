using DG.Tweening;
using UnityEngine;

public class ClockBehavior : MonoBehaviour
{
    [SerializeField] Transform minuteHand;
    [SerializeField] Transform hourHand;

    void Start()
    {
        hourHand.eulerAngles = new(0, 0, -120f); // i.e., 4pm
        minuteHand.eulerAngles = Vector3.zero;
    }

    public void StartTimer()
    {
        float minuteStart = minuteHand.localEulerAngles.z;
        float hourStart = hourHand.localEulerAngles.z;

        // (4x)s/hr * 8hr = 90s --> 90 / 32 s
        hourHand.DORotate(new Vector3(0, 0, hourStart - 7.5f), Parameters.timePerRound / 32f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);

        
        minuteHand.DORotate(new Vector3(0, 0, minuteStart - 90f), Parameters.timePerRound / 32f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
    }

    public void StopTimer()
    {
        minuteHand.DOKill();
        hourHand.DOKill();
    }
}
