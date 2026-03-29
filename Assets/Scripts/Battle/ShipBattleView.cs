using DG.Tweening;
using UnityEngine;

public class ShipBattleView : MonoBehaviour
{
    private ShipType shipType;
    [SerializeField] GameObject shipModel;
    GameObject model;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (shipModel != null)
        {
            model = Instantiate(model, this.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeModel(GameObject shipModel)
    {
        if (shipModel != null)
        {
            if (model != null)
            {
                Destroy(model);
            }
            model = Instantiate(shipModel, this.transform);
        }
    }

    public void Move(Vector3 position, float modelYValue)
    {
        Vector3 targetPos = new Vector3(position.x, modelYValue, position.z);

        Vector3 direction = (targetPos - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.DORotateQuaternion(targetRotation, 0.3f)
                 .SetEase(Ease.InOutSine)
                 .OnComplete(() =>
                 {
                     transform.DOMove(targetPos, 1f).SetEase(Ease.InOutSine);
                 });
    }
}
