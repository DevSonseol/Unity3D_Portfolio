using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(menuName = "Quest/Category" , fileName = "Category_")]
public class Category : SerializedScriptableObject ,IEquatable<Category>
{
    [SerializeField] private string codeName;
    [SerializeField] private string displayName;

    public string CodeName => codeName;
    public string DisplayName => displayName;

    #region Operator
    public bool Equals(Category other)
    {
        if (other is null)
            return false;
        if(ReferenceEquals(other,this))
            return true;
        if (GetType() != other.GetType())
            return false;
        return codeName == other.CodeName;
    }

    public override int GetHashCode() => (CodeName, DisplayName).GetHashCode();

    public override bool Equals(object other) => Equals(other as Category);

    public static bool operator == (Category lhs, string rhs)
    {
        if (lhs is null)
            return ReferenceEquals(rhs, null);
        return lhs.codeName == rhs || lhs.DisplayName == rhs;
    }

    public static bool operator !=(Category lhs, string rhs) => !(lhs == rhs);
    //category.CodeName == "Kill" x
    //category == "Kill"  O
    #endregion
}
