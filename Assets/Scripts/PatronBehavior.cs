using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class PatronBehavior : MonoBehaviour
{
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private TextMesh textbox;
    public Parameters.DRINK preferredDrink;
    private Coroutine waitCoroutine = null;

    public static Action OnQueueUpdated;

    private void OnEnable()
    {
        OnQueueUpdated += UpdateTransformBySiblingIndex;
        GameStateManager.OnGameOver += () => { LeaveAfterService(false); };
    }

    private void OnDisable()
    {
        OnQueueUpdated -= UpdateTransformBySiblingIndex;
        GameStateManager.OnGameOver -= () => { LeaveAfterService(false); };
    }

    void Start()
    {
        UpdateTransformBySiblingIndex();
    }

    IEnumerator PatronWaiting()
    {
        yield return new WaitForSeconds(Parameters.patronWaitTime);
        textbox.text = "This place\nsucks!";
        yield return new WaitForSeconds(1.0f);
        LeaveAfterService(false);
        waitCoroutine = null;
    }

    public void SetPreferredDrink(Parameters.DRINK drink)
    {
        preferredDrink = drink;
        // GetComponent<SpriteRenderer>().color = Parameters.drinkToColor[preferredDrink];
        GetComponent<SpriteRenderer>().color = Parameters.drinkToColor[(Parameters.DRINK)UnityEngine.Random.Range(0,5)];
    }

    private void UpdateTransformBySiblingIndex()
    {
        GetComponent<SpriteRenderer>().sortingOrder = -transform.GetSiblingIndex();
        transform.DOMoveX(7 * transform.GetSiblingIndex(), 2.5f).OnComplete(() =>
        {
            if (transform.GetSiblingIndex() < 3)
            {
                speechBubble.SetActive(true);
                textbox.text = $"I'll have a\n{preferredDrink}!";
                waitCoroutine ??= StartCoroutine(PatronWaiting());
            }
        });

        
    }

    public void LeaveAfterService(bool isSatisfied = true)
    {
        if (waitCoroutine != null) StopCoroutine(waitCoroutine);
        speechBubble.SetActive(false);
        transform.DOMoveX(isSatisfied ? -20 : 20, 2.5f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            transform.SetAsLastSibling();
            OnQueueUpdated.Invoke();
            transform.DOKill();
            Destroy(gameObject);
        });
    }
}
