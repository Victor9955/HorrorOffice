using System;
using UnityEngine;

public class FileChoice : MonoBehaviour
{

    public Action<int> OnChooseEvent;

    private int _id;
    public void Init(File file)
    {
        MeshRenderer meshRend = GetComponent<MeshRenderer>();
        meshRend.material.color = file.FileColor;
        _id = file.FileID;
    }


    private void OnMouseDown()
    {
        OnChooseEvent?.Invoke(_id);
    }
}
