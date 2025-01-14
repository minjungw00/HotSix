using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Localization.Settings;

public class SpawnCard_MJW : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Properties

    public GameManager gameManager;
    public LaneSpawnManager_MJW laneManager;
    public CameraMove_HJH cameraMove;
    
    public GameObject unitPrefab;
    private GameObject tempObject;

    public UnitID unitID;
    public int cost;
    public Image backgroundImage;
    public Image unitImage;
    public TextMeshProUGUI costText;

    public float curCooldown;
    public float maxCooldown;

    public bool isColorChange = false;

    private Vector3 mousePosition;

    #endregion


    #region Methods

    public void SetUnit(UnitID unitID){
        this.unitID = unitID;
        int level = 0;

        if(unitID.unitTag == UnitTag.Unit){
            level = gameManager.userInfo.userUnitInfo[unitID.id].level;
            unitPrefab = gameManager.unitPrefabManager.unitPrefabs.playerUnitPrefabs[unitID.id];
            cost = gameManager.playerUnitTable.unitData[unitID.id].entityInfos.cost;
            maxCooldown = gameManager.playerUnitTable.unitData[unitID.id].entityInfos.cooldown;

            unitImage.sprite = gameManager.unitImages.playerUnitImages[unitID.id].inGame_Icon;
            backgroundImage.sprite = gameManager.unitImages.playerUnitImages[unitID.id].inGame_Icon;
        }
        else if(unitID.unitTag == UnitTag.Special){
            level = gameManager.userInfo.userSpecialUnitInfo[unitID.id].level;
            unitPrefab = gameManager.unitPrefabManager.unitPrefabs.specialUnitPrefabs[unitID.id];
            cost = gameManager.specialUnitTable.specialUnitData[unitID.id].entityInfos.cost;
            maxCooldown = gameManager.specialUnitTable.specialUnitData[unitID.id].entityInfos.cooldown;

            unitImage.sprite = gameManager.unitImages.specialUnitImages[unitID.id].inGame_Icon;
            backgroundImage.sprite = gameManager.unitImages.specialUnitImages[unitID.id].inGame_Icon;
        }

        costText.text = cost.ToString();
    }

    public void CountCooldowns(float time){
        curCooldown -= time;
        if(curCooldown < 0.0f) curCooldown = 0.0f;
        unitImage.fillAmount = 1.0f - (curCooldown / maxCooldown);
    }

    public void ChangeColor(bool darker){
        RectTransform[] allChildren = transform.GetComponentsInChildren<RectTransform>();
        foreach(RectTransform child in allChildren){
            if(child.TryGetComponent<Image>(out var image))
            {
                Color tmp = image.color;
                if(darker){
                    tmp -= new Color(0.5f, 0.5f, 0.5f, 0);
                }
                else{
                    tmp += new Color(0.5f, 0.5f, 0.5f, 0);
                }
                image.color = tmp;
            }
        }
    }

    #endregion


    #region Monobehavior Callbacks

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        laneManager = GameObject.Find("LaneManager").GetComponent<LaneSpawnManager_MJW>();
        cameraMove = GameObject.Find("Camera").GetComponent<CameraMove_HJH>();
        backgroundImage = transform.Find("BackGround").GetComponent<Image>();
        unitImage = transform.Find("Image").GetComponent<Image>();
        costText = transform.Find("CostText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        CountCooldowns(Time.deltaTime);
    }

    public void OnPointerDown(PointerEventData eventData){
        if(curCooldown > 0.0f || laneManager.moneyManager.money < cost) return;
        isColorChange = true;
        ChangeColor(isColorChange);
    }

    public void OnPointerUp(PointerEventData eventData){
        if(isColorChange){
            isColorChange = false;
            ChangeColor(isColorChange);
        }
        
    }

    public void OnBeginDrag(PointerEventData eventData){
        if(curCooldown > 0.0f || laneManager.moneyManager.money < cost) return;

        tempObject = Instantiate(unitPrefab);
        Entity tempUnit = tempObject.GetComponent<Entity>();

        tempUnit.isActive = false;
        tempObject.transform.Rotate(new Vector3(0, 180.0f, 0));

        tempObject.tag = "Untagged";

        mousePosition = Input.mousePosition;
        mousePosition.z = -5.0f;

        tempObject.transform.position = Camera.main.ScreenToWorldPoint(mousePosition + new Vector3(0.0f, 0.0f, 10.0f));

        /*
        // 유닛 색상 변경
        Transform[] allChildren = tempObject.GetComponentsInChildren<Transform>();
        foreach(Transform child in allChildren){
            SpriteRenderer sprite = child.GetComponent<SpriteRenderer>();
            if(sprite != null){
                Color tmp = sprite.color;
                tmp -= new Color(0.5f, 0.5f, 0.5f, 0);
                sprite.color = tmp;
            }
        }
        */

        cameraMove.isActive = false;
    }

    public void OnDrag(PointerEventData eventData){
        if(curCooldown > 0.0f || laneManager.moneyManager.money < cost || tempObject == null) return;

        mousePosition = Input.mousePosition;
        mousePosition.z = -5.0f;

        tempObject.transform.position = Camera.main.ScreenToWorldPoint(mousePosition + new Vector3(0.0f, 0.0f, 10.0f));
    }

    public void OnEndDrag(PointerEventData eventData){
        if(curCooldown > 0.0f || laneManager.moneyManager.money < cost || tempObject == null) return;

        cameraMove.isActive = true;

        GameObject lane = laneManager.CheckUnitToLane(tempObject);
        
        if(unitID.unitTag == UnitTag.Unit){
            if(lane != null){
                if(unitID.unitTag == UnitTag.Unit){
                    laneManager.SpawnPlayerUnit(lane, unitID);
                }
                curCooldown = maxCooldown;
            }
        }
        else if(unitID.unitTag == UnitTag.Special){
            laneManager.SpawnSpecialUnit(unitID);
            curCooldown = maxCooldown;
        }

        Destroy(tempObject);
    }

    #endregion
}
