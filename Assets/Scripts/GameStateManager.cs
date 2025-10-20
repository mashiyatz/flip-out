using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private Text timeTextbox;
    [SerializeField] private Text profitTextbox;
    [SerializeField] private float timePerRound;
    [SerializeField] private GameObject patronPrefab;
    [SerializeField] private Transform patronsObjectParent;
    [SerializeField] private float newPatronInterval;
    private float remainingTimeInRound;
    private bool roundStart = false;

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
        profitTextbox.text = $"${profit}";
    }

    void Start()
    {
        profit = 1000;
        remainingTimeInRound = timePerRound;
        timeTextbox.text = $"{remainingTimeInRound}";
        profitTextbox.text = $"${profit}";
        patronQueue = new Queue<PatronBehavior>();
        QueueNewPatron();
    }

    void QueueNewPatron()
    {
        GameObject go = Instantiate(patronPrefab, patronsObjectParent);
        go.GetComponent<PatronBehavior>().preferredDrink = (DrinkTypes.TYPE)UnityEngine.Random.Range(0, 3);
        patronQueue.Enqueue(go.GetComponent<PatronBehavior>());
    }

    IEnumerator CreateNewPatronsOverTime()
    {
        float timer = newPatronInterval;
        while (true)
        {
            yield return new WaitForSeconds(newPatronInterval);
            QueueNewPatron();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.A) && !roundStart)
        {
            roundStart = true;
            StartCoroutine(CreateNewPatronsOverTime());
            return;
        }
        if (roundStart) { 
            remainingTimeInRound -= Time.deltaTime;
            timeTextbox.text = $"{remainingTimeInRound:00}";
            if (remainingTimeInRound <= 0)
            {
                roundStart = false;
                Debug.Log("Game Over!");
                StopAllCoroutines();
            }

            if (Input.GetKeyDown(KeyCode.A)) OnServeDrink.Invoke();
        }
    }
}
