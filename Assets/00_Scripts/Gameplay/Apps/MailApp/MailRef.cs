using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MailRef", menuName = "Scriptable Objects/MailRef")]
public class MailRef : ScriptableObject
{
    public List<MailView> mailsPrefab;
}
