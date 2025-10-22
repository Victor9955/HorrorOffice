using System;
using Unity.Properties;
using UnityEngine;

[Serializable]
public class BinderData
{
    public Color color = Color.white;
}

public class FileBinder : MonoBehaviour, IDropContainer
{

    [SerializeField] private int _id;
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
    public Color MeshMatColor
    {
        get => MeshRend.material.color;
        private set => MeshRend.material.color = value;
    }

    public void Init(int id, BinderData data = null)
    {
        _id = id;
        if (data == null) return; // managing data like binder color etc
        MeshMatColor = data.color;

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
