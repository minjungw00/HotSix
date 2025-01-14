using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOArrowAttack", menuName = "ActionBehavior/ArrowAttack")]
public class SOArrowAttack : SOActionBase
{
    public override bool Condition(Action_MJW action){
        action.targetObjects = FindTarget(action);
        if(action.targetObjects.Count > 0){
            action.targetPosition = action.targetObjects[0].transform.position;
            return true;
        }
        return false;
    }

    public override IEnumerator ExecuteAction(Action_MJW action){
        yield return new WaitForSeconds(action.cooldown * 0.66f);
        Shoot(action);
        if (action.mainUnit.GetComponent<Unit>().state != Unit.UnitState.Die && action.audio.clip != null)
        {
            action.audio.Play();
        }
        yield break;
    }

    public void Shoot(Action_MJW action){
        if(action.targetObjects.Count > 0){
            if(action.targetObjects[0] != null){
                action.targetPosition = action.targetObjects[0].transform.position;
            }
        }

        GameObject pInstance = Instantiate(actionObject);
        Projectile pScript = pInstance.GetComponent<Projectile>();

        pInstance.transform.SetParent(action.mainUnit.transform.parent);

        Vector3 startPos = action.mainUnit.transform.position + new Vector3(action.mainUnit.GetComponent<Unit>().isEnemy ? -0.5f : 0.5f, 0.5f, 0);
        Vector3 endPos = action.targetPosition;
        Vector3 midPos = (startPos + endPos) / 2.0f;
        midPos.y += System.Math.Abs(endPos.x - startPos.x) * 0.45f;

        //Debug.Log(startPos + " " + midPos + " " + endPos);

        pScript.SetPos(startPos, midPos, endPos);
        pScript.action.lane = pInstance.transform.parent.gameObject;
        pScript.action.duration = (System.Math.Abs(endPos.x - startPos.x) + 0.1f) / 8.0f;
        pScript.action.value = action.value;
        pScript.action.isEnemy = action.mainUnit.GetComponent<Unit>().isEnemy;
        pScript.isTurning = false;
        pScript.isEnemy = pScript.action.isEnemy;
        pScript.action.isEnemy = pScript.isEnemy;
        
        pScript.isActive = true;
    }
}
