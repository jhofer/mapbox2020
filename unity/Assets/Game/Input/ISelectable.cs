using UnityEngine;

public interface ISelectable
{
     void Select();
     bool IsSelected { get; }
}