using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private Text timeTextbox;
    [SerializeField] private Text profitTextbox;
    
    [SerializeField] private GameObject patronPrefab;
    [SerializeField] private Transform patronsObjectParent;
    [SerializeField] private ClockBehavior clock;

    [SerializeField] private Text panelTextbox;
    [SerializeField] private GameObject panelObject;
    
    private float remainingTimeInRound;

    public static Transform patronsParentReference;
    public static Action OnServeDrink;
    private float profit;

    private void OnEnable()
    {
        MugBehavior.OnCustomerServed += UpdateProfit;
        SpigotBehavior.OnPour += UpdateProfit;
    }

    private void OnDisable()
    {
        MugBehavior.OnCustomerServed -= UpdateProfit;
        SpigotBehavior.OnPour -= UpdateProfit;
    }

    void UpdateProfit(float increment)
    {
        profit += increment;
        profitTextbox.text = $"${profit:N0}";
    }

    void Start()
    {
        profit = Parameters.startCash;
        remainingTimeInRound = Parameters.timePerRound;
        timeTextbox.text = $"{remainingTimeInRound}";
        profitTextbox.text = $"${profit:N0}";

        patronsParentReference = patronsObjectParent;
        panelObject.SetActive(true);
        panelTextbox.text = "Press A to clock in!";
    }

    void QueueNewPatron()
    {
        GameObject go = Instantiate(patronPrefab, patronsObjectParent);
        go.GetComponent<PatronBehavior>().SetPreferredDrink((Parameters.DRINK)UnityEngine.Random.Range(0, 5));
    }

    IEnumerator CreateNewPatronsOverTime()
    {
        float timer = Parameters.newPatronInterval;
        while (true)
        {
            if (patronsObjectParent.childCount < 5)
            {
                QueueNewPatron();
                yield return new WaitForSeconds(Parameters.newPatronInterval);
            }
            yield return null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        if (Input.GetKeyDown(KeyCode.A) && !Parameters.isGameStart)
        {
            Parameters.isGameStart = true;
            remainingTimeInRound = Parameters.timePerRound;
            panelObject.SetActive(false);
            clock.StartTimer();
            StartCoroutine(CreateNewPatronsOverTime());
            return;
        }
        if (Parameters.isGameStart) { 
            remainingTimeInRound -= Time.deltaTime;
            timeTextbox.text = $"{remainingTimeInRound:00}";
            if (remainingTimeInRound <= 0)
            {
                Parameters.isGameStart = false;
                panelObject.SetActive(true);
                panelTextbox.text = "Another day another dollar.";
                clock.StopTimer();
                StopAllCoroutines();
            }

            if (Input.GetKeyDown(KeyCode.A)) OnServeDrink.Invoke();
        }
    }
}
