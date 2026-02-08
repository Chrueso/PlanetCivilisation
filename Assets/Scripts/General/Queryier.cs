using System.Collections;
using UnityEngine;

public class Queryier : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        print(TouchscreenHandler.main);
    }
    void Start()
    {
        StartCoroutine(IJustPrintStuff());
    }

    private IEnumerator IJustPrintStuff()
    {
        while (true)
        {
            print(TouchscreenHandler.main);
            yield return new WaitForSeconds(2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
