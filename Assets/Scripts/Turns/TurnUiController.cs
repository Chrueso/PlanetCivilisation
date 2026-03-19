using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnUiController : MonoBehaviour
{

    private TurnManager turnManager;

    void Awake()
    {
       
    }

    private void OnDisable()
    {
        if (turnManager != null)
        {
            turnManager.OnTurnStart -= HandleTurnStartUI;
            turnManager.OnTurnEnd -= HandleTurnEndUI;

        }
    }

    private void Start()
    {
        turnManager = GameManager.Instance.turnManager;
        turnManager.OnTurnStart += HandleTurnStartUI;
        turnManager.OnTurnEnd += HandleTurnEndUI;

    }

    private void HandleTurnStartUI()
    {
        StartCoroutine(UpdateResourceTimer());
        idk();
    }

    private void HandleTurnEndUI()
    {
        StartCoroutine(UpdateResourceTimer());
        
    }

    private IEnumerator UpdateResourceTimer()
    {
        yield return new WaitForSeconds(1.0f);
        UpdateResourceVisuals();
    }

    //js get rid of ts once the ui is fixed
    private void UpdateResourceVisuals()
    {
       
    }

    //js get rid of ts once the ui is fixed
    private void idk()
    {
        
    }


}
