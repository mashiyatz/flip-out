using System;
using System.Collections;
using UnityEngine;

public class SpigotBehavior : MonoBehaviour
{
    [SerializeField] Parameters.DRINK drinkType;
    [SerializeField] private KeyCode keyCode;
    [SerializeField] private ParticleSystem particles;
    //private Coroutine spigotOnCoroutine;

    public static Action<Transform, Parameters.DRINK, bool> OnSpigotStateChange; 

    void Start()
    {
        GetComponentInChildren<SpriteRenderer>().color = Parameters.drinkToColor[drinkType];
    }

    void SetSpigotState(bool startPouring) {
        var em = particles.emission;
        if (startPouring && !em.enabled) { em.enabled = true; OnSpigotStateChange.Invoke(transform, drinkType, true); }
        else if (!startPouring && em.enabled) { em.enabled = false; OnSpigotStateChange.Invoke(transform, drinkType, false); }
        
    }

    // deprecated
    //IEnumerator SpigotPour()
    //{
    //    float pourTime = 0;
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(1.0f);
    //        pourTime += 1;
    //        Debug.Log($"pouring for {pourTime}s");
    //    }
        
    //}

    void Update()
    {
        if (Parameters.isGameStart) SetSpigotState(Input.GetKey(keyCode));
        
    }
}
