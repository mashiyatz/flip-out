using System;
using System.Collections;
using UnityEngine;

public class SpigotBehavior : MonoBehaviour
{
    [SerializeField] Parameters.DRINK drinkType;
    [SerializeField] private KeyCode keyCode;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private float pourRate = 0.25f; // replace with fill rate of individual glass type
    [SerializeField] private float costPerSec = 1;
    private Coroutine spigotOnCoroutine;

    public static Action<Transform, bool> OnSpigotStateChange;
    public static Action<Parameters.DRINK, float> OnDrinkChange;
    public static Action<float> OnPour;

    [SerializeField] private Sprite tapOn;
    [SerializeField] private Sprite tapOff;
    private SpriteRenderer activeSprite;

    private void OnEnable()
    {
        GameStateManager.OnGameOver += () => { SetSpigotState(false); };
    }

    private void OnDisable()
    {
        GameStateManager.OnGameOver -= () => { SetSpigotState(false); };
    }

    void Start()
    {
        var em = particles.emission;
        em.enabled = false;
        StartCoroutine(SpigotPour());
        costPerSec = Parameters.drinkToCost[drinkType] * pourRate / 2; // decide better way of pricing

        activeSprite = GetComponent<SpriteRenderer>();
    }

    void UpdateSprite(bool isPouring)
    {
        activeSprite.sprite = isPouring ? tapOn : tapOff; 
    }

    void SetSpigotState(bool startPouring) {
        var em = particles.emission;
        if (em.enabled != startPouring)
        {
            em.enabled = startPouring;
            OnSpigotStateChange.Invoke(transform.GetChild(0), startPouring);
            if (em.enabled) OnDrinkChange.Invoke(drinkType, pourRate);
        }
        UpdateSprite(startPouring);
    }

    IEnumerator SpigotPour()
    {
        while (true)
        {
            if (particles.emission.enabled && Parameters.isGameStart)
            {
                OnPour.Invoke(-costPerSec);
                yield return new WaitForSeconds(1.0f);
            }
            yield return null;
        }
    }

    void Update()
    {
        if (Parameters.isGameStart)
        {
            SetSpigotState(Input.GetKey(keyCode));
        }

        
    }
}
