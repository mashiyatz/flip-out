using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class PatronBehavior : MonoBehaviour
{
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private TextMesh textbox;
    public Parameters.DRINK preferredDrink;
    //private Sequence waitSequence;
    private Coroutine waitCoroutine;

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
        waitCoroutine = StartCoroutine(PatronWaiting());
    }

    IEnumerator PatronWaiting()
    {
        while (transform.GetSiblingIndex() > 3) yield return null;
        yield return new WaitForSeconds(Parameters.patronWaitTime);
        textbox.text = "This place\nsucks!";
        yield return new WaitForSeconds(1.0f);
        LeaveAfterService(false);
    }

    public void SetPreferredDrink(Parameters.DRINK drink)
    {
        preferredDrink = drink;
        // GetComponent<SpriteRenderer>().color = Parameters.drinkToColor[preferredDrink];
        GetComponent<SpriteRenderer>().color = Parameters.drinkToColor[(Parameters.DRINK)UnityEngine.Random.Range(0,5)];
    }

    private void UpdateTransformBySiblingIndex()
    {
        //transform.localScale = Mathf.Pow(0.95f, transform.GetSiblingIndex()) * Vector2.one;
        GetComponent<SpriteRenderer>().sortingOrder = -transform.GetSiblingIndex();
        transform.DOMoveX(7 * transform.GetSiblingIndex(), 2.5f).OnComplete(() =>
        {
            if (transform.GetSiblingIndex() < 3)
            {
                speechBubble.SetActive(true);
                textbox.text = $"I'll have a\n{preferredDrink}!";
            }
        });
    }

    public void LeaveAfterService(bool isSatisfied = true)
    {
        StopCoroutine(waitCoroutine);
        speechBubble.SetActive(false);
        transform.DOMoveX(isSatisfied ? -20 : 20, 2.5f).SetEase(Ease.OutQuad).OnComplete(() =>
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
