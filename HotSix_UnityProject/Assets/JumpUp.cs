using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpUp : MonoBehaviour
{       
    public Transform box;

    private void OnEnable(){
        box.localPosition = new Vector2(0,-Screen.height);
        box.LeanMoveLocalY(0,0.7f).setEaseOutBack();
    }
}