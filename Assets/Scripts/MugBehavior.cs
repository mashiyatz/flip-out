using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class MugBehavior : MonoBehaviour
{
    [SerializeField] Transform spriteFill;
    [SerializeField] SpriteRenderer sprite;
    private float fillRate;
    private float fillPercent;
    private Coroutine fillupCoroutine = null;
    private Parameters.DRINK servedDrinkType;

    public static Action<float> OnCustomerServed;

    private void OnEnable()
    {
        SpigotBehavior.OnSpigotStateChange += UpdateMugPlacement;
        SpigotBehavior.OnDrinkChange += ChangeFillColor;
        GameStateManager.OnServeDrink += ServeCustomer;
        GameStateManager.OnGameOver += () => { UpdateFill(0); };
    }

    private void OnDisable()
    {
        SpigotBehavior.OnSpigotStateChange -= UpdateMugPlacement;
        SpigotBehavior.OnDrinkChange -= ChangeFillColor;
        GameStateManager.OnServeDrink -= ServeCustomer;
        GameStateManager.OnGameOver -= () => { UpdateFill(0); };
    }

    private void UpdateMugPlacement(Transform spigot, bool isPouring)
    {
        if (isPouring && fillupCoroutine == null) fillupCoroutine = StartCoroutine(MugFill());
        else if (!isPouring && fillupCoroutine != null)
        {
            StopCoroutine(fillupCoroutine);
            fillupCoroutine = null;
        }
        Vector2 newPos = (Vector2)spigot.position + Vector2.down * 2 + Vector2.right * 0.5f;
        if ((Vector2)transform.position != newPos) UpdateFill(0);
        transform.position = newPos;
    }

    private void ChangeFillColor(Parameters.DRINK drink, float pourRate)
    {
        fillRate = pourRate;
        servedDrinkType = drink;
        sprite.color = Parameters.drinkToColor[servedDrinkType];
    }

    IEnumerator MugFill()
    {
        float pourTime = 0;
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            UpdateFill(fillPercent + fillRate);
            pourTime += 1;
            Debug.Log($"pouring for {pourTime}s");
        }
    }

    private void ServeCustomer()
    {
        PatronBehavior nextPatron = GameStateManager.patronsParentReference.GetChild(0).GetComponent<PatronBehavior>();
        if (nextPatron.preferredDrink == servedDrinkType && fillPercent == 1) OnCustomerServed.Invoke(20);
        // else OnCustomerServed.Invoke(-20);
        UpdateFill(0);

        nextPatron.LeaveAfterService();
    }

    private void UpdateFill(float fill)
    {   
        fillPercent = Mathf.Min(fill, 1);
        float fillPosition = Mathf.Lerp(-1, 0, fill);
        spriteFill.localPosition = new Vector2(0, fillPosition);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
