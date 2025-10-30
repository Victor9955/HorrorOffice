using UnityEngine;

[CreateAssetMenu(fileName = "MailData", menuName = "Scriptable Objects/MailData")]
public class MailData : ScriptableObject
{
    public string title;
    public CharacterData character;
    public string mailText;
}
