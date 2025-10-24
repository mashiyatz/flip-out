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
    public static Action<SpigotBehavior> OnNewActiveSpigot;

    [SerializeField] private Sprite tapOn;
    [SerializeField] private Sprite tapOff;
    private SpriteRenderer activeSprite;

    [SerializeField] private GameObject mugPrefab;
    public GameObject mug = null; 

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
            if (em.enabled)
            {
                if (mug == null)
                {
                    mug = Instantiate(mugPrefab, transform);
                    mug.transform.position = (Vector2)transform.GetChild(0).position + Vector2.down * 2 + Vector2.right * 0.5f;
                }
            } else
            {
                OnNewActiveSpigot.Invoke(this);
            }

            mug.GetComponent<MugBehavior>().UpdateMugPlacement(startPouring);
            mug.GetComponent<MugBehavior>().ChangeFillColor(drinkType, pourRate);
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
