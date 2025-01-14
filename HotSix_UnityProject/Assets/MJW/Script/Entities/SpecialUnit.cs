using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialUnit : Entity
{
    #region Properties

    public SpecialUnitData unitData;
    public Action_MJW actionBehavior;

    #endregion


    #region Methods

    public void Init(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if(transform.GetComponent<Animator>() == null){
            anim = transform.GetChild(0).transform.GetComponent<Animator>();
        }
        else{
            anim = transform.GetComponent<Animator>();
        }

        state = UnitState.Idle;

        id = unitData.entityInfos.id;
        level = gameManager.userInfo.userSpecialUnitInfo[id].level;

        actionBehavior = unitData.actionBehavior;
        actionBehavior.audio = gameObject.GetComponent<AudioSource>();

        if(unitData.audio != null)
        {
            actionBehavior.audio.clip = unitData.audio;
        }

        actionBehavior.mainUnit = transform.gameObject;
        actionBehavior.value = unitData.actionBehavior.value + (unitData.actionBehavior.upgradeValue * (level - 1));
        actionBehavior.range = unitData.actionBehavior.range;
        actionBehavior.cooldown = unitData.actionBehavior.cooldown;
    }

    #endregion


    #region Monobehavior Callbacks

    void Awake()
    {
        Init();
    }
    
    private void Start()
    {
        AudioSource audio;
        if (gameObject.TryGetComponent<AudioSource>(out audio))
        {
            GameManager.instance.soundEffects.Add(audio);
            audio.volume = GameManager.instance.SoundEffectVolume;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(state == UnitState.Die){
            StartCoroutine(Die(1.0f));
        }
        else if(state == UnitState.Action){
            StartCoroutine(actionBehavior.action.ExecuteAction(actionBehavior));
            state = UnitState.Idle;
        }
    }

    #endregion
}
