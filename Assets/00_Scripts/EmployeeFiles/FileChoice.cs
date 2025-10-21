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

    private int _id;
    public int Id => _id;
    public bool isOpen;
    private MeshRenderer _meshRend;
    public MeshRenderer MeshRend
    {
        get
        {
            if (_meshRend == null)
            {
                _meshRend = GetComponent<MeshRenderer>();

            }
            return _meshRend;
        }
    }
    public Color MeshMatColor => MeshRend.material.color;

    public void Init(FileChoiceData file)
    {
        _meshRend.material.color = file._color;
        _id = file._id;
    }
    public bool Drop<T>(T dropped) where T : Draggable
    {
        EmployeeFile file = dropped as EmployeeFile;
        if (file == null) throw new Exception("Bruh that aint no File");
        bool match = file.FileID == Id;
        // Call fileSorting if true;
        return match;
    }
    public bool IsOpen()
    {
        return isOpen;
    }

}
