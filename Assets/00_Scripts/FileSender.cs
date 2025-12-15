using System;
using System.Collections.Generic;
using TMPEffects.AutoParameters.Attributes;
using TMPEffects.Databases;
using TMPEffects.TMPCommands;
using UnityEngine;

[AutoParameters]
[CreateAssetMenu(fileName = "FileSender")]
public partial class FileSender : TMPCommand
{
    public override TagType TagType => TagType.Index;
    public override bool ExecuteInstantly => false;
    public override bool ExecuteOnSkip => true;
    public override bool ExecuteRepeatable => true;

#if UNITY_EDITOR
    public override bool ExecuteInPreview => true;
#endif

    public event Action OnGiveFile;

    private partial void ExecuteCommand(AutoParametersData data, ICommandContext context)
    {
        OnGiveFile?.Invoke();
    }
}
