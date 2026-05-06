using DG.Tweening;
using UnityEngine;
using VolumetricLines;

public class ShipBattleView : MonoBehaviour
{
    public ShipType shipType { get; private set; }
    public FactionType factionType { get; private set; }
    public GameObject model { get; private set; }
    public GameObject laserPrefab;
    public int unitCount = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void ChangeModel(GameObject shipModel, ShipType shipType)
    {
        if (shipModel != null)
        {
            model = Instantiate(shipModel, this.transform);
            this.shipType = shipType;
            //Debug.Log($"changed model to {shipModel}");
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

    public Sequence Shoot(GameObject target, float duration)
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            if (target == null) return;

            GameObject laser = Instantiate(laserPrefab, model.transform.position, Quaternion.identity);

            var line = laser.GetComponent<VolumetricLineBehavior>();
            if (line != null)
                line.LineColor = GetColor(factionType);

            laser.transform.LookAt(target.transform);
            AudioService.CurrentAudioInstance.PlayOneShot(GameManager.audioLib.laserSound, 0.5f);
            laser.transform.DOMove(target.transform.position, duration)
                .OnComplete(() =>
                {
                    if (laser != null)
                        Destroy(laser);
                });
        });

        return seq;
    }

    //public void ChangeColor(Color newColor)
    //{
    //    MeshRenderer[] renderers = model.GetComponentsInChildren<MeshRenderer>();
    //    foreach (var renderer in renderers)
    //    {
    //        foreach(var mat in renderer.materials)
    //        {
    //            mat.color = newColor;
    //        }
    //    }
    //}

    MaterialPropertyBlock mpb;

    public void ChangeColor(Color newColor)
    {
        if (mpb == null)
            mpb = new MaterialPropertyBlock();

        MeshRenderer[] renderers = model.GetComponentsInChildren<MeshRenderer>();

        foreach (var renderer in renderers)
        {
            renderer.GetPropertyBlock(mpb);

            if (renderer.sharedMaterial.HasProperty("_BaseColor"))
                mpb.SetColor("_BaseColor", newColor);
                //mpb.SetColor("_BaseColor", Color.green);
            else
                mpb.SetColor("_Color", newColor); // fallback

            renderer.SetPropertyBlock(mpb);
        }
    }

    public Color GetColor(FactionType faction)
    {
        switch (faction)
        {
            case FactionType.Human:
                return new Color(1, 0.6f, 0.06f);
                break;
            case FactionType.DemiHuman:
                return new Color(0.49f,0.24f,1);
                break;
            case FactionType.IntelligentConstruct:
                return new Color(0.65f,0.65f,0.65f);
                break;
        }
        return Color.white;
    }

    public void setFaction(FactionType faction)
    {
        factionType = faction;
    }

}
