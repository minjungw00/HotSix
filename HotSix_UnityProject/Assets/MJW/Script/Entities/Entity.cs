using System.Collections;
using System.Collections.Generic;   
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Properties

    [System.Serializable]
    public enum UnitState{
        Idle,
        Move,
        Action,
        Stun,
        Die
    };

    public GameManager gameManager;
    public Animator anim;

    public UnitState state;
    public int id;
    public int level;
    public bool isEnemy;

    public bool isActive = true;

    #endregion


    #region Methods

    public IEnumerator Die(float totalTime){
        gameObject.GetComponent<Collider>().enabled = false;

        int count = transform.childCount;
            
        for(int i = 0; i < count; ++i){
            GameObject child = transform.GetChild(i).gameObject;
            if(child.name != "Sprite") Destroy(child);
            --i;
            --count;
        }

        float time = 0.0f;
        // 유닛이 점점 투명해지도록 하위 스프라이트 투명도 조절
        Transform[] allChildren = transform.gameObject.GetComponentsInChildren<Transform>();
        while(time < totalTime){
            time += Time.deltaTime / totalTime;

            foreach(Transform child in allChildren){
                if(child == null) continue;
                if(child.TryGetComponent(out SpriteRenderer sprite))
                {
                    Color color = sprite.color;
                    Color temp = color;
                    temp.a = 0.0f;
                    sprite.color = Color.Lerp(color, temp, time);
                }
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        Destroy(gameObject);
        yield break;
    }

    #endregion
}
