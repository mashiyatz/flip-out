using DG.Tweening;
using System;
using UnityEngine;

public class PatronBehavior : MonoBehaviour
{
    public Parameters.DRINK preferredDrink;
    private Sequence waitSequence;

    public static Action OnQueueUpdated;

    private void OnEnable()
    {
        OnQueueUpdated += UpdateTransformBySiblingIndex;
    }

    private void OnDisable()
    {
        OnQueueUpdated -= UpdateTransformBySiblingIndex;
    }

    void Start()
    {
        waitSequence = DOTween.Sequence();
        waitSequence.AppendInterval(Parameters.patronWaitTime);
        waitSequence.AppendCallback(() => {
            LeaveAfterService(false);
        });

        UpdateTransformBySiblingIndex();
    }

    public void SetPreferredDrink(Parameters.DRINK drink)
    {
        preferredDrink = drink;
        GetComponent<SpriteRenderer>().color = Parameters.drinkToColor[preferredDrink];
    }

    private void UpdateTransformBySiblingIndex()
    {
        transform.localScale = Mathf.Pow(0.95f, transform.GetSiblingIndex()) * Vector2.one;
        GetComponent<SpriteRenderer>().sortingOrder = -transform.GetSiblingIndex();
        transform.DOMoveX(7 * transform.GetSiblingIndex(), 2.5f);
    }

    public void LeaveAfterService(bool isSatisfied = true)
    {
        waitSequence.Kill();
        transform.DOMoveX(isSatisfied ? -20 : 20, 2.5f).OnComplete(() =>
        {
            transform.SetAsLastSibling();
            OnQueueUpdated.Invoke();
            transform.DOKill();
            Destroy(gameObject);
        });
    }

    void Update()
    {
        
    }
}
