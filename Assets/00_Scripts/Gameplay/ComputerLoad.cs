using UnityEngine;

public class ComputerLoad : MonoBehaviour
{
    [SerializeField] string username;
    [SerializeField] int passwordLength;

    [SerializeField] float loadTime;

    public void Load()
    {
        Debug.Log("Test");
    }
}
