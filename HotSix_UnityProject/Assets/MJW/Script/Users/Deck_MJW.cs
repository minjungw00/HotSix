using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum UnitTag{
    Unit,
    Special
}

[System.Serializable]
public struct UnitID{
    public UnitTag unitTag;
    public int id;

    public override bool Equals(object obj){
        UnitID target = (UnitID)obj;
        return unitTag == target.unitTag && id == target.id;
    }
    public override int GetHashCode(){
        return (unitTag, id).GetHashCode();
    }
}

[System.Serializable]
public class Deck_MJW
{
    public List<UnitID> unitIDs;

    public Deck_MJW(){
        unitIDs = new List<UnitID>();
        for(int i = 0; i < 4; ++i){
            UnitID ids = new()
            {
                unitTag = UnitTag.Unit,
                id = 2 * i + 1
            };
            unitIDs.Add(ids);
        }
        unitIDs.Add(new UnitID(){ unitTag = UnitTag.Special, id = 1 });
    }
}
