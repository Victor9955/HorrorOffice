using UnityEngine;

[CreateAssetMenu(fileName = "MailData", menuName = "Scriptable Objects/MailData")]
public class MailData : ScriptableObject
{
    public string title;
    public CharacterData character;
    [TextArea(maxLines: 100,minLines: 1)]public string mailText;
}
