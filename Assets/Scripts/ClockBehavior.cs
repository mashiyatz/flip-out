using DG.Tweening;
using UnityEngine;

public class ClockBehavior : MonoBehaviour
{
    [SerializeField] Transform minuteHand;
    [SerializeField] Transform hourHand;

    void Start()
    {
        hourHand.eulerAngles = new(0, 0, -120f);
        minuteHand.eulerAngles = Vector3.zero;
        StartTimer();
    }

    public void StartTimer()
    {
        float minuteStart = minuteHand.localEulerAngles.z;
        float hourStart = hourHand.localEulerAngles.z;

        hourHand.DORotate(new Vector3(0, 0, hourStart - 7.5f), 5, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);

        minuteHand.DORotate(new Vector3(0, 0, minuteStart - 90f), 5, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);


    }

    public void StopTimer()
    {
        minuteHand.DOKill();
        hourHand.DOKill();
    }

    void Update()
    {
        
    }
}
