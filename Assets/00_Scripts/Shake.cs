using System.Collections.Generic;
using TMPEffects.AutoParameters.Attributes;
using TMPEffects.Databases;
using TMPEffects.TMPCommands;
using UnityEngine;

[AutoParameters]
[CreateAssetMenu(fileName = "Shake")]
public partial class Shake : TMPCommand
{
    public override TagType TagType => TagType.Index;
    public override bool ExecuteInstantly => false;
    public override bool ExecuteOnSkip => true;
    public override bool ExecuteRepeatable => true;

#if UNITY_EDITOR
    public override bool ExecuteInPreview => true;
#endif
    [AutoParameter("direction", "dir")]
    Vector2 dir = Vector2.one;

    [AutoParameter("strength", "s")]
    float s = 1f;

    private partial void ExecuteCommand(AutoParametersData data, ICommandContext context)
    {
        if (context.Writer.gameObject.TryGetComponent(out ActionReceiver actionReceiver))
        {
            actionReceiver.ShakeSprite(data.dir, data.s);
        }
    }
}
