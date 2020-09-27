using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHandler : BaseSingleton<SelectionHandler>
{

    public ISelectable selectedObject;

    public void Select(ISelectable selectable)
    {
        this.selectedObject = selectable;

    }

    public bool IsSelected(ISelectable selectable)
    {
        return this.selectedObject == selectable;
    }

    public void ClearSelection()
    {
        this.selectedObject = null;
    }

    public T GetSelection<T>() where T:  class, ISelectable
    {
        return selectedObject as T;
    }
}
