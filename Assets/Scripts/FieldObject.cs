using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: IFieldObject를 활용할 필요가 있는지 확인해보고, 필요없다고 판단되면 없애고, IFieldObject 파일을 지울 것
/// <summary>
/// 모든 필드 상의 오브젝트는 FieldObject를 상속받게 될 것입니다.
/// </summary>
public class FieldObject : MonoBehaviour, IFieldObject
{
    public int HP { get; set; }
}
