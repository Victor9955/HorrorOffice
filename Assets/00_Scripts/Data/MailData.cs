using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "MailData", menuName = "Scriptable Objects/MailData")]
public class MailData : ScriptableObject
{
    public string title;
    public CharacterData character;
    public bool hasDelay;
    [ShowIf("hasDelay")]
    public bool hasRandomDelay;
    [ShowIf("hasDelay")]
    public float delay;
    [ShowIf("hasRandomDelay")]
    public float maxDelay;
    [TextArea(maxLines: 100,minLines: 1)]public string mailText;
}
