using DG.Tweening;
using UnityEngine;

public class ShipBattleView : MonoBehaviour
{
    public ShipType shipType { get; private set; }
    public GameObject model { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void ChangeModel(GameObject shipModel, ShipType shipType)
    {
        if (shipModel != null)
        {
            model = Instantiate(shipModel, this.transform);
            this.shipType = shipType;
        }
    }

    public Tween Move(Vector3 position, float modelYValue = 30)
    {
        Vector3 targetPos = new Vector3(position.x, modelYValue, position.z);

        Vector3 direction = (targetPos - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        Sequence seq = DOTween.Sequence();

        seq.Append(
            transform.DORotateQuaternion(targetRotation, 0.3f)
                .SetEase(Ease.InOutSine));
        seq.Append(
            transform.DOMove(targetPos, 1f)
                .SetEase(Ease.InOutSine));

        return seq;
    }

    public Tween TurnTo(Vector3 dir, float duration = 0.3f)
    {
        Quaternion rot = Quaternion.LookRotation(dir);
        return transform.DORotateQuaternion(rot, duration).SetEase(Ease.InOutSine);
    }
}
