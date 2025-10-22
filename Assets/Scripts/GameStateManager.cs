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
    
    private float remainingTimeInRound;

    public static Action OnServeDrink;
    public static Queue<PatronBehavior> patronQueue;
    private float profit;

    private void OnEnable()
    {
        MugBehavior.OnCustomerServed += UpdateProfit;
    }

    private void OnDisable()
    {
        MugBehavior.OnCustomerServed -= UpdateProfit;
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
        patronQueue = new Queue<PatronBehavior>();
        QueueNewPatron();

        Debug.Log("Press A to Start!"); 
    }

    void QueueNewPatron()
    {
        GameObject go = Instantiate(patronPrefab, patronsObjectParent);
        go.GetComponent<PatronBehavior>().preferredDrink = (Parameters.DRINK)UnityEngine.Random.Range(0, 3);
        patronQueue.Enqueue(go.GetComponent<PatronBehavior>());
    }

    IEnumerator CreateNewPatronsOverTime()
    {
        float timer = Parameters.newPatronInterval;
        while (true)
        {
            yield return new WaitForSeconds(Parameters.newPatronInterval);
            if (patronQueue.Count < 5) QueueNewPatron();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.A) && !Parameters.isGameStart)
        {
            Parameters.isGameStart = true;
            StartCoroutine(CreateNewPatronsOverTime());
            return;
        }
        if (Parameters.isGameStart) { 
            remainingTimeInRound -= Time.deltaTime;
            timeTextbox.text = $"{remainingTimeInRound:00}";
            if (remainingTimeInRound <= 0)
            {
                Parameters.isGameStart = false;
                Debug.Log("Game Over!");
                StopAllCoroutines();
            }

            if (Input.GetKeyDown(KeyCode.A)) OnServeDrink.Invoke();
        }
    }
}
