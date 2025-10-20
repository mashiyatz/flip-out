using System;
using System.Collections;
using UnityEngine;

public class MugBehavior : MonoBehaviour
{
    [SerializeField] Transform spriteFill;
    private float fillPercent;
    private Coroutine fillupCoroutine = null;
    private DrinkTypes.TYPE servedDrinkType;

    public static Action<float> OnCustomerServed;

    private void OnEnable()
    {
        SpigotBehavior.OnSpigotStateChange += UpdateMugPlacement;
        GameStateManager.OnServeDrink += ServeCustomer;
    }

    private void OnDisable()
    {
        SpigotBehavior.OnSpigotStateChange -= UpdateMugPlacement;
        GameStateManager.OnServeDrink -= ServeCustomer;
    }

    private void UpdateMugPlacement(Transform spigot, DrinkTypes.TYPE drinkType, bool isPouring)
    {
        if (isPouring && fillupCoroutine == null) fillupCoroutine = StartCoroutine(MugFill());
        else if (!isPouring && fillupCoroutine != null)
        {
            StopCoroutine(fillupCoroutine);
            fillupCoroutine = null;
        }
        Vector2 newPos = (Vector2)spigot.position + Vector2.down * 7;
        if ((Vector2)transform.position != newPos) UpdateFill(0);
        transform.position = newPos;
        servedDrinkType = drinkType;
    }

    IEnumerator MugFill()
    {
        float pourTime = 0;
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            UpdateFill(fillPercent + 0.25f);
            pourTime += 1;
            Debug.Log($"pouring for {pourTime}s");
        }
    }

    private void ServeCustomer()
    {
        PatronBehavior nextPatron = GameStateManager.patronQueue.Dequeue();
        if (nextPatron.preferredDrink == servedDrinkType) OnCustomerServed.Invoke(20);
        else OnCustomerServed.Invoke(-20);
        UpdateFill(0);
        Destroy(nextPatron.gameObject);
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
