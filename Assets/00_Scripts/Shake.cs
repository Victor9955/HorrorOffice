using System.Collections.Generic;
using TMPEffects.AutoParameters.Attributes;
using TMPEffects.Databases;
using TMPEffects.TMPCommands;
using UnityEngine;

[AutoParameters]
[CreateAssetMenu(fileName = "SpriteChange")]
public partial class Shake : TMPCommand
{
    public override TagType TagType => TagType.Index;
    public override bool ExecuteInstantly => false;
    public override bool ExecuteOnSkip => true;
    public override bool ExecuteRepeatable => true;

#if UNITY_EDITOR
    public override bool ExecuteInPreview => true;
#endif


    private partial void ExecuteCommand(AutoParametersData data, ICommandContext context)
    {
        if (context.Writer.gameObject.TryGetComponent(out ActionReceiver actionReceiver))
        {
            actionReceiver.ShakeSprite();
        }
    }
}
