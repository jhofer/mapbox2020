using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class Building : MonoBehaviour, IEntity, ISelectable
{
    private Outline outline;
    private bool highLight=false;

    // Start is called before the first frame update
    // Start is called before the first frame update
    void Start()
    {
        this.outline = this.GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Select()
    {
        this.highLight = !this.highLight;
        if (this.highLight)
        {
            Camera.main.GetComponent<OutlineEffect>().AddOutline(outline);
        }
        else
        {
            Camera.main.GetComponent<OutlineEffect>().RemoveOutline(outline);
        }
    }
}
