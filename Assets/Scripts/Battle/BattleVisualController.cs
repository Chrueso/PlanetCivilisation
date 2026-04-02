using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class BattleVisualController : MonoBehaviour
{
    [SerializeField] ShipModelDatabaseSO shipModelDb;
    [SerializeField] GameObject shipBattlePrefab;
    List<GameObject> attackingShipModels;
    List<GameObject> defendingShipModels;

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

        if (attackingShips.TryGetValue(ShipType.Attacker, out int attackerCount))
        {
            Debug.Log($"spawning {attackerCount} ships for attacker faction {attackerFaction}");
            for (int i = 0; i < attackerCount; i++)
            {
                GameObject ship = Instantiate(shipBattlePrefab, targetPlanet.View.transform);
                //make the ship & set its model
                attackingShipModels.Add(ship);
                //change this to attacker faction
                ship.GetComponent<ShipBattleView>().ChangeModel(shipModelDb.GetModel(attackerFaction, ShipType.Attacker),ShipType.Attacker);
                ship.transform.position = targetPlanet.CurrentHex.WorldPosition;
            }
        }


        foreach (var defendingShip in defendingShips)
        {
            Debug.Log($"spawning {defendingShip.Value} {defendingShip.Key} for defender faction {targetPlanet.FactionType}");
            for (int i = 0; i < defendingShip.Value; i++)
            {
                GameObject ship = Instantiate(shipBattlePrefab, targetPlanet.View.transform);
                //make the ship & set its model
                defendingShipModels.Add(ship);
                ship.GetComponent<ShipBattleView>().ChangeModel(shipModelDb.GetModel(targetPlanet.FactionType, defendingShip.Key),defendingShip.Key);
                ship.transform.position = targetPlanet.CurrentHex.WorldPosition;
            }
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
            var type = ship.GetComponent<ShipBattleView>().shipType;
            if (!attackingShipsByType.ContainsKey(type))
                attackingShipsByType[type] = new List<GameObject>();
            attackingShipsByType[type].Add(ship);
        }

        foreach (var ship in defendingShipModels)
        {
            var type = ship.GetComponent<ShipBattleView>().shipType;
            if (!defendingShipsByType.ContainsKey(type))
                defendingShipsByType[type] = new List<GameObject>();
            defendingShipsByType[type].Add(ship);
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
                stepSeq.Append(VisualiseFighting(attackerShip, defenderShip, step.attackerWon, 0.3f));
                stepSeq.Append(VisualiseFighting(attackerShip, defenderShip, step.attackerWon, 0.3f));
                stepSeq.AppendInterval(0.2f);
                groupSeq.Join(stepSeq);
            }

            turnSeq.Append(groupSeq);
        }

        //Sequence fadeSeq = DOTween.Sequence();
        turnSeq.AppendCallback(() =>
        {
            foreach (var ship in attackingShipsByType.Values)
            {
                foreach (var s in ship)
                {
                    //MeshRenderer meshRenderer = s.GetComponentInChildren<MeshRenderer>();
                    //Material mat = meshRenderer.material;
                    //Debug.Log(mat.shader.name);
                    Destroy(s);
                }
            }
            foreach (var ship in defendingShipsByType.Values)
            {
                foreach (var s in ship)
                {
                    //MeshRenderer meshRenderer = s.GetComponentInChildren<MeshRenderer>();
                    //Material mat = meshRenderer.material;
                    //Debug.Log(mat.shader.name);
                    Destroy(s);
                }
            }
        });

        //turnSeq.Append(fadeSeq);
    }

    private Tween VisualiseFighting(GameObject opponent1, GameObject opponent2, bool attackerWon, float delay)
    {
        Sequence seq = DOTween.Sequence();
        //yield return new WaitForSeconds(delay);
        seq.Join(opponent1.transform.DOShakePosition(0.5f, 0.5f).SetEase(Ease.InOutSine));
        seq.Join(opponent2.transform.DOShakePosition(0.5f, 0.5f).SetEase(Ease.InOutSine));
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() =>
        {
            if (attackerWon) Destroy(opponent2); else Destroy(opponent1);
        }
        );

        return seq;
    }
}
