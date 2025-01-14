using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOSpecialStun", menuName = "SpecialBehavior/SpecialStun")]
public class SOSpecialStun : SOActionBase
{
    public override bool Condition(Action_MJW action){
        return true;
    }

    public override IEnumerator ExecuteAction(Action_MJW action){
        GameObject map = GameObject.Find("BG+MapManager");
        Entity mainSpecial = action.mainUnit.GetComponent<Entity>();
        Collider mainCollider = action.mainUnit.GetComponent<Collider>();

        // 중앙으로 이동
        while(true){
            Vector3 center = mainCollider.bounds.center;
            if(mainSpecial.isEnemy && (center.x <= map.transform.position.x)
            || !mainSpecial.isEnemy && (center.x >= map.transform.position.x)){
                break;
            }
            action.mainUnit.transform.Translate(new Vector3(-15.0f, 0, 0) * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        GameObject waveObject = Instantiate(actionObject, action.mainUnit.transform);
        
        // 효과 발동
        if(action.audio.clip != null)
        {
            action.audio.Play();
        }

        Transform lane = GameObject.Find("Lane").transform;
        Transform[] allChildren = lane.GetComponentsInChildren<Transform>();
        foreach(Transform child in allChildren){
            if(child.CompareTag("Unit")){
                Unit unit = child.GetComponent<Unit>();
                if(mainSpecial.isEnemy != unit.isEnemy){
                    unit.stunCooldown = action.value;
                }
            }
        }

        yield return new WaitForSeconds(action.value);

        Destroy(waveObject);
        mainSpecial.state = Entity.UnitState.Die;

        yield break;
    }
}
