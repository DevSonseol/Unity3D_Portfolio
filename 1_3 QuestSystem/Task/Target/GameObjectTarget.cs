using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Target/GameObject", fileName = "Target_")]
public class GameObjectTarget : TaskTarget
{
    [SerializeField] private GameObject value;
    public override object Value => value;

    public override bool IsEqual(object target)
    {
        var targetAsGO = target as GameObject;
        if (targetAsGO == null)
            return false;
        return targetAsGO.name.Contains(value.name);//프리팹이나 게임씬에 존재하는 오브젝트 일 수 있기 때문에 이름으로 비교
    }
}
