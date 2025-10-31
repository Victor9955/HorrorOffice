using System;
using Unity.Properties;
using UnityEngine;


public class FileBinder : MonoBehaviour, IDropContainer
{

    public Binder BinderType => _binderType;
    public bool isOpen;
    private MeshRenderer _meshRend;
    private Binder _binderType;
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

    public void Init( Binder bindertype)
    {
        _binderType = bindertype;
    }
    public bool Drop<T>(T dropped) where T : Draggable
    {
        EmployeeFile file = dropped as EmployeeFile;
        if (file == null) throw new Exception("Bruh that aint no File");
        return true;
    }
    public bool IsOpen()
    {
        return isOpen;
    }


    #region Debug


    private void OnMouseDown()
    {
        Debug.Log($"{name}'s type is {BinderType}");
    }
    #endregion
}
