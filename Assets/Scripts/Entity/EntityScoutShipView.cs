using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class EntityScoutShipView : MonoBehaviour
{
    private ObjectPool<EntityScoutShipView> pool;
    private Vector3 prevPos = Vector3.zero;
    public void SetPool(ObjectPool<EntityScoutShipView> pool)
    {
        this.pool = pool;
    }
    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }
    public void Release()
    {
        this.pool.Release(this);
    }
    public Sequence Move(Vector3 position, float modelYValue)
    {
        prevPos = transform.position;
        Vector3 targetPos = new Vector3(position.x, modelYValue, position.z);
        Vector3 direction = (targetPos - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        return DOTween.Sequence()
            .Append(transform.DORotateQuaternion(targetRotation, 0.3f).SetEase(Ease.InOutSine))
            .Append(transform.DOMove(targetPos, 0.3f).SetEase(Ease.InOutSine));
    }

    public Sequence MoveBack(Vector3 position, float modelYValue)
    {
        Vector3 targetPos = new Vector3(prevPos.x, modelYValue, prevPos.z);
        Vector3 returnDirection = (prevPos - targetPos).normalized;
        Quaternion returnRotation = Quaternion.LookRotation(returnDirection);
        return DOTween.Sequence()
            .Append(transform.DORotateQuaternion(returnRotation, 0.3f).SetEase(Ease.InOutSine))
            .Append(transform.DOMove(prevPos, 0.3f).SetEase(Ease.InOutSine));
    }
}
