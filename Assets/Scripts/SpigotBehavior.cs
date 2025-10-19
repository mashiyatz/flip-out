using System.Collections;
using UnityEngine;

public class SpigotBehavior : MonoBehaviour
{
    [SerializeField] private KeyCode keyCode;
    [SerializeField] private GameObject barrelContentPrefab;
    private Coroutine spigotOnCoroutine;

    void Start()
    {
        
    }

    void SetSpigotState(bool startPouring) {
        if (startPouring && spigotOnCoroutine == null) spigotOnCoroutine = StartCoroutine(SpigotPour());
        else if (!startPouring && spigotOnCoroutine != null)
        {
            StopCoroutine(spigotOnCoroutine);
            Debug.Log("pour stops");
            spigotOnCoroutine = null;
        }
    }

    IEnumerator SpigotPour()
    {
        float pourTime = 0;
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            GameObject go = Instantiate(barrelContentPrefab, transform);
            go.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-4, 0);
            pourTime += 1;
            Debug.Log($"pouring for {pourTime}s");
        }
        
    }

    void Update()
    {
        SetSpigotState(Input.GetKey(keyCode));
        
    }
}
