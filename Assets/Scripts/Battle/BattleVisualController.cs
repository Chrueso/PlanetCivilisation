using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class BattleVisualController : MonoBehaviour
{
    [SerializeField] ShipModelDatabaseSO shipModelDb;
    [SerializeField] GameObject shipBattlePrefab;
    [SerializeField] int shipMergeThreshold;
    List<GameObject> attackingShipModels;
    List<GameObject> defendingShipModels;
    List<GameObject> allShipModels;

    public void setupBattle(Dictionary<ShipType, int> attackingShips, Dictionary<ShipType, int> defendingShips, FactionType attackerFaction, PlanetData targetPlanet, List<BattleStep> battleSteps)
    {
        spawnShips(attackingShips, defendingShips, attackerFaction, targetPlanet);

        float radius = targetPlanet.View.ShapeSettings.PlanetRadius * 2;

        Sequence fanSeq = DOTween.Sequence();
        fanSeq.Join(fanOutShips(attackingShipModels, targetPlanet, radius, 0, 180));
        fanSeq.Join(fanOutShips(defendingShipModels, targetPlanet, radius, 190, 350));

        fanSeq.OnComplete(() =>
        {
            pickTargets(attackingShipModels, defendingShipModels, battleSteps);
        });
    }

    void spawnShips(Dictionary<ShipType, int> attackingShips, Dictionary<ShipType, int> defendingShips, FactionType attackerFaction, PlanetData targetPlanet)
    {
        attackingShipModels = new();
        defendingShipModels = new();
        allShipModels = new();

        if (attackingShips.TryGetValue(ShipType.Attacker, out int attackerCount))
        {
            int groupedAttackerCount = attackerCount / shipMergeThreshold;
            int remainderAttackerCount = attackerCount % shipMergeThreshold;

            Debug.Log($"spawning {attackerCount} ships for attacker faction {attackerFaction}");

            spawnShipGroup(groupedAttackerCount, attackerFaction, ShipType.Attacker, shipMergeTier.grouped, attackingShipModels, targetPlanet);
            if(remainderAttackerCount > 0) spawnShipGroup(remainderAttackerCount, attackerFaction, ShipType.Attacker, shipMergeTier.single, attackingShipModels, targetPlanet);
        }


        foreach (var defendingShip in defendingShips)
        {
            int groupedDefenderCount = defendingShip.Value / shipMergeThreshold;
            int remainderDefenderCount = defendingShip.Value % shipMergeThreshold;

            Debug.Log($"spawning {defendingShip.Value} {defendingShip.Key} for defender faction {targetPlanet.FactionType}");

            spawnShipGroup(groupedDefenderCount, targetPlanet.FactionType, defendingShip.Key, shipMergeTier.grouped, defendingShipModels, targetPlanet);
            if (remainderDefenderCount > 0) spawnShipGroup(remainderDefenderCount, targetPlanet.FactionType, defendingShip.Key, shipMergeTier.single, defendingShipModels, targetPlanet);
        }
    }

    Tween fanOutShips(List<GameObject> ships, PlanetData targetPlanet, float radius, float startAngle, float endAngle)
    {
        int count = ships.Count;
        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < count; i++)
        {
            float t = count == 1 ? 0.5f : (float)i / (count - 1);
            float angle = Mathf.Lerp(startAngle, endAngle, t);

            Vector3 shipPosition = GetOrbitPosition(targetPlanet, radius, angle);

            GameObject ship = ships[i];

            ships[i].TryGetComponent<ShipBattleView>(out ShipBattleView shipVisual);

            // rotate to face planet
            Vector3 dir = (targetPlanet.CurrentHex.WorldPosition - shipPosition).normalized;

            if (shipVisual != null) seq.Join(shipVisual.Move(shipPosition));
            seq.Join(shipVisual.TurnTo(dir));
        }

        return seq;
    }

    Vector3 GetOrbitPosition(PlanetData targetPlanet, float radius, float angle)
    {
        Vector3 center = targetPlanet.CurrentHex.WorldPosition;
        float rad = angle * Mathf.Deg2Rad;
        float finalRadius = targetPlanet.View.ShapeSettings.PlanetRadius + radius;

        float x = Mathf.Cos(rad) * finalRadius;
        float z = Mathf.Sin(rad) * finalRadius;

        return new Vector3(center.x + x, center.y, center.z + z);
    }

    void pickTargets(List<GameObject> attackingShips, List<GameObject> defendingShips, List<BattleStep> battleSteps)
    {
        Dictionary<ShipType, List<GameObject>> attackingShipsByType = new();
        Dictionary<ShipType, List<GameObject>> defendingShipsByType = new();

        foreach (var ship in attackingShipModels)
        {
            ShipBattleView shipVisual = ship.GetComponent<ShipBattleView>();
            ShipType type = shipVisual.shipType;

            for(int i = 0; i < shipVisual.unitCount; i++)
            {
                if (!attackingShipsByType.ContainsKey(type))
                {
                    attackingShipsByType[type] = new List<GameObject>();
                }
                attackingShipsByType[type].Add(ship);
            }
        }

        foreach (var ship in defendingShipModels)
        {
            ShipBattleView shipVisual = ship.GetComponent<ShipBattleView>();
            ShipType type = shipVisual.shipType;

            for (int i = 0; i < shipVisual.unitCount; i++)
            {
                if (!defendingShipsByType.ContainsKey(type))
                {
                    defendingShipsByType[type] = new List<GameObject>();
                }
                defendingShipsByType[type].Add(ship);
            }
        }


        Sequence turnSeq = DOTween.Sequence();
        int groupSize = (int)Mathf.Round(battleSteps.Count/2);

        for (int i = 0; i < battleSteps.Count; i+=groupSize)
        {
            Sequence groupSeq = DOTween.Sequence();

            for(int j = 0; j < groupSize && i+j < battleSteps.Count; j++)
            {
                BattleStep step = battleSteps[i + j];
                Sequence stepSeq = DOTween.Sequence();

                Debug.Log($"Attacker {step.attackerType} vs Defender {step.defenderType} - Attacker Won: {step.attackerWon}");
                GameObject attackerShip = attackingShipsByType[step.attackerType][0];
                GameObject defenderShip = defendingShipsByType[step.defenderType][^1];

                if (step.attackerWon) defendingShipsByType[step.defenderType].Remove(defenderShip); else attackingShipsByType[step.attackerType].Remove(attackerShip);

                //make the attacking ship look at the defending ship
                Vector3 dirAttack = (defenderShip.transform.position - attackerShip.transform.position).normalized;

                stepSeq.Join(attackerShip.GetComponent<ShipBattleView>().TurnTo(dirAttack));
                stepSeq.Join(defenderShip.GetComponent<ShipBattleView>().TurnTo(-dirAttack));
                stepSeq.Append(VisualiseFighting(attackerShip, defenderShip, step.attackerWon, 0.2f));
                stepSeq.Append(VisualiseFighting(attackerShip, defenderShip, step.attackerWon, 0.2f));
                stepSeq.AppendInterval(0.1f);
                //groupSeq.Append(stepSeq);
                groupSeq.Insert(j * 0.15f, stepSeq);
            }

            turnSeq.Append(groupSeq);
        }

        Sequence fadeSeq = BuildFadeSequence(allShipModels,0.5f);
        turnSeq.Append(fadeSeq);

    }

    private Tween VisualiseFighting(GameObject opponent1, GameObject opponent2, bool attackerWon, float delay)
    {
        ShipBattleView opponent1Visual = opponent1.GetComponent<ShipBattleView>();
        ShipBattleView opponent2Visual = opponent2.GetComponent<ShipBattleView>();

        Sequence seq = DOTween.Sequence();
        //yield return new WaitForSeconds(delay);
        seq.Join(opponent1Visual.Shoot(opponent2, 0.5f));
        seq.Join(opponent2Visual.Shoot(opponent1, 0.5f));
        seq.Append(opponent1.transform.DOShakePosition(0.5f, 0.5f).SetEase(Ease.InOutSine));
        seq.Join(opponent2.transform.DOShakePosition(0.5f, 0.5f).SetEase(Ease.InOutSine));
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() =>
        {
            if (attackerWon)
            {

                ShipBattleView visual = opponent2.GetComponent<ShipBattleView>();
                visual.unitCount--;

                if(visual.unitCount == 0) Destroy(visual.gameObject);
            }
            else
            {
                ShipBattleView visual = opponent1.GetComponent<ShipBattleView>();
                visual.unitCount--;

                if (visual.unitCount == 0) Destroy(visual.gameObject);
            }
        }
        );

        return seq;
    }

    private Sequence BuildFadeSequence(List<GameObject> ships, float fadeDuration)
    {
        Sequence seq = DOTween.Sequence();

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();

        foreach (var ship in ships)
        {
            if (ship == null) continue;
            MeshRenderer[] renderers = ship.GetComponentsInChildren<MeshRenderer>();

            foreach (var r in renderers)
            {
                r.GetPropertyBlock(mpb);
                Color startColor = mpb.GetColor("_BaseColor"); // or _Color fallback

                // Animate the color using DOTween
                seq.Join(DOTween.To(() => startColor, x =>
                {
                    mpb.SetColor("_BaseColor", x);
                    r.SetPropertyBlock(mpb);
                }, new Color(0, 0, 0, 0), fadeDuration));
            }
        }

        foreach (var ship in ships)
        {
            if (ship == null) continue;
            seq.AppendCallback(() => Destroy(ship));
        }

        return seq;
    }

    private void spawnShipGroup(int spawnCount, FactionType faction , ShipType type, shipMergeTier shipTier, List<GameObject> associatedList, PlanetData targetPlanet)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject ship = Instantiate(shipBattlePrefab, targetPlanet.View.transform);
            //make the ship & set its model
            associatedList.Add(ship);
            allShipModels.Add(ship);
            //change this to attacker faction

            ShipBattleView shipVisual = ship.GetComponent<ShipBattleView>();

            shipVisual.setFaction(faction);
            shipVisual.ChangeModel(shipModelDb.GetModel(faction, type, shipTier), type);
            shipVisual.ChangeColor(shipVisual.GetColor(faction));

            switch(shipTier)
            {
                case shipMergeTier.single:
                    shipVisual.unitCount = 1;
                    break;
                case shipMergeTier.grouped:
                    shipVisual.unitCount = shipMergeThreshold;
                    break;
            }

            if (shipVisual.unitCount > 1) shipVisual.model.transform.localScale *= 1.5f;

            ship.transform.position = targetPlanet.CurrentHex.WorldPosition;
        }
    }
}
