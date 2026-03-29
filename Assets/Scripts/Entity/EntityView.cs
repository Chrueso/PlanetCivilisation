using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EntityView : MonoBehaviour
{
    public Sequence Move(Vector3 position, float modelYValue)
    {
        Vector3 targetPos = new Vector3(position.x, modelYValue, position.z);
        Vector3 direction = (targetPos - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        return DOTween.Sequence()
            .Append(transform.DORotateQuaternion(targetRotation, 0.3f).SetEase(Ease.InOutSine))
            .Append(transform.DOMove(targetPos, 1f).SetEase(Ease.InOutSine));
    }
}