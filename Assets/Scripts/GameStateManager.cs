using DG.Tweening;
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
    public static Action OnServeDrink; // keep index of drink
    public static Action OnGameOver;
    private float profit;
    private float roundStartProfit;

    public static SpigotBehavior activeSpigot;
    [SerializeField] private Transform activeSpaceMarker;

    private void OnEnable()
    {
        MugBehavior.OnCustomerServed += UpdateProfit;
        SpigotBehavior.OnPour += UpdateProfit;
        SpigotBehavior.OnNewActiveSpigot += SetActiveSpigot;
    }

    private void OnDisable()
    {
        MugBehavior.OnCustomerServed -= UpdateProfit;
        SpigotBehavior.OnPour -= UpdateProfit;
        SpigotBehavior.OnNewActiveSpigot += SetActiveSpigot;
    }

    void UpdateProfit(float increment)
    {
        profit += increment;
        profitTextbox.text = $"${profit:N0}";
        DOTween.Sequence()
            .Append(profitTextbox.DOColor(increment > 0 ? Color.green : Color.red, 0.1f))
            .Append(profitTextbox.DOColor(Color.white, 0.5f)).SetEase(Ease.OutSine);

    }

    void Start()
    {
        profit = roundStartProfit = Parameters.startCash;
        remainingTimeInRound = Parameters.timePerRound;
        timeTextbox.text = $"{remainingTimeInRound}";
        profitTextbox.text = $"${profit:N0}";

        patronsParentReference = patronsObjectParent;
        panelObject.SetActive(true);
        panelTextbox.text = "Another day, another dollar.";
    }

    void QueueNewPatron()
    {
        GameObject go = Instantiate(patronPrefab, patronsObjectParent);
        go.GetComponent<PatronBehavior>().SetPreferredDrink((Parameters.DRINK)UnityEngine.Random.Range(0, 5));
    }

    IEnumerator CreateNewPatronsOverTime()
    {
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

    void StartRound()
    {
        Parameters.isGameStart = true;
        remainingTimeInRound = Parameters.timePerRound;
        panelObject.SetActive(false);
        clock.StartTimer();
        StartCoroutine(CreateNewPatronsOverTime());
        roundStartProfit = profit;
    }

    public void SetActiveSpigot(SpigotBehavior spigot)
    {
        activeSpigot = spigot;
        activeSpaceMarker.position = (Vector2)spigot.transform.GetChild(0).position + Vector2.down * 2;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        if (Input.GetKeyDown(KeyCode.Space) && !Parameters.isGameStart)
        {
            StartRound();
            return;
        }
        if (Parameters.isGameStart) { 
            remainingTimeInRound -= Time.deltaTime;
            timeTextbox.text = $"{remainingTimeInRound:00}";
            if (remainingTimeInRound <= 0)
            {
                Parameters.isGameStart = false;
                panelObject.SetActive(true);
                panelTextbox.text = $"Net profit: ${(profit - roundStartProfit):N0}\nI hate my job.";
                clock.StopTimer();
                StopAllCoroutines();
                OnGameOver.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Space) && activeSpigot != null)
            {
                activeSpigot.mug.GetComponent<MugBehavior>().ServeCustomer();
                //OnServeDrink.Invoke();
            }
        }
    }
}
