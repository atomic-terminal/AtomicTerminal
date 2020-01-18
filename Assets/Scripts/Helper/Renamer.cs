using System;
using UnityEngine;

public class Renamer : MonoBehaviour
{
    [ContextMenu("Rename")]
    private void Rename()
    {
        for (int i = 0; i < this.parent.childCount; i++)
        {
            if (this.parent.GetChild(i).name.Contains(this.fromName) || this.toAppend)
            {
                if (this.toAppend)
                {
                    this.parent.GetChild(i).name = this.toName + this.parent.GetChild(i).name;
                }
                else
                {
                    this.parent.GetChild(i).name = this.parent.GetChild(i).name.Replace(this.fromName, this.toName);
                }
                if (this.parent.GetChild(i).childCount > 0)
                {
                    this.CheckChilds(this.parent.GetChild(i));
                }
            }
        }
    }

    private void CheckChilds(Transform t)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            if (t.GetChild(i).name.Contains(this.fromName) || this.toAppend)
            {
                if (this.toAppend)
                {
                    t.GetChild(i).name = this.toName + t.GetChild(i).name;
                }
                else
                {
                    t.GetChild(i).name = t.GetChild(i).name.Replace(this.fromName, this.toName);
                }
                this.CheckChilds(t.GetChild(i));
            }
        }
    }

    public bool toAppend;

    public string fromName;

    public string toName;

    public Transform parent;
}
