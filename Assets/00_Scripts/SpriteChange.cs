using System.Collections.Generic;
using TMPEffects.AutoParameters.Attributes;
using TMPEffects.Databases;
using TMPEffects.TMPCommands;
using UnityEngine;

[AutoParameters]
[CreateAssetMenu(fileName = "SpriteChange")]
public partial class SpriteChange : TMPCommand
{
    public override TagType TagType => TagType.Index;
    public override bool ExecuteInstantly => false;
    public override bool ExecuteOnSkip => false;
    public override bool ExecuteRepeatable => true;

#if UNITY_EDITOR
    public override bool ExecuteInPreview => true;
#endif

    [AutoParameter(true,"")] private string spriteKey;

    private partial void ExecuteCommand(AutoParametersData data, ICommandContext context)
    {
        if(context.Writer.gameObject.TryGetComponent(out SpriteChanger dialoguePlayer))
        {
            dialoguePlayer.ChangeSprite(data.spriteKey);
        }
        else
        {
            Debug.Log("Couldnt Get Component in Parent");
        }
    }
}
