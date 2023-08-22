using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOInstantAttack", menuName = "ActionBehavior/InstantAttack")]
public class SOInstantAttack : SOActionBase
{
    public override bool Condition(Action_MJW action){
        action.targetObjects = FindTarget(action);
        return action.targetObjects.Count > 0;
    }

    public override IEnumerator ExecuteAction(Action_MJW action){
        yield return new WaitForSeconds(action.cooldown * 0.5f);
        if (action.mainUnit != null) 
        {
            Attack(action);
            if(action.audio.clip != null)
            {
                action.audio.Play();
            }
            
        }
        yield break;
    }

    public void Attack(Action_MJW action){
        Unit unit = action.mainUnit.GetComponent<Unit>();
        if(action.targetObjects.Count == 0) return;
        foreach(GameObject t in action.targetObjects){
            if(t == null) continue;
            if(t.CompareTag("Unit")){
                t.GetComponent<Unit>().GetDamage(action.value);
            }
            else if(t.CompareTag("Tower")){
                if(unit.isEnemy){
                    action.towerManager.playerTowerHP -= action.value;
                }
                else{
                    action.towerManager.enemyTowerHP -= action.value;
                }
            }
        }
    }
}