using System;
using Unity.Properties;
using UnityEngine;

[Serializable]
public class FileChoiceData
{
    public int _id;
    public Color _color;
}

public class FileChoice : MonoBehaviour, IDropContainer
{

    public Action<int> OnChooseEvent;

    private int _id;

    public void Init(FileChoiceData file)
    {
        MeshRenderer meshRend = GetComponent<MeshRenderer>();
        meshRend.material.color = file._color;
        _id = file._id;
    }
    public bool Drop<T>(T dropped) where T : Draggable
    {
        EmployeeFile file = dropped as EmployeeFile;
        if (file == null) throw new Exception("Bruh that aint no File");
        bool match = file.FileID == _id;
        Utils.BigText(match ? "Match !" : "No match..." );
        // Call fileSorting if true;
        return match;
    }

}
