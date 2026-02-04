using Steamworks;
using Steamworks.Data;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SteamManager : MonoBehaviour
{
    [SerializeField] uint app_id;
    [SerializeField] RawImage avatar;
    [SerializeField] TextMeshProUGUI username;

    private IEnumerator Start()
    {
        try
        {
            SteamClient.Init(app_id);
        }
        catch 
        {
            username.text = "Not Connected\nto Steam";
            avatar.enabled = false;
        }

        yield return new WaitUntil(() => (SteamClient.IsValid && SteamClient.IsLoggedOn));

        if(SteamClient.IsValid && SteamClient.IsLoggedOn)
        {
            username.text = SteamClient.Name;
            GetAvatar();
            var ach = new Achievement("OPEN_GAME");
            if(!ach.State)
            {
                ach.Trigger();
            }

            foreach (var a in SteamUserStats.Achievements)
            {
                Debug.Log($"{a.Name} ({a.State})");
            }
        }
        else
        {
            username.text = "Not Connected\nto Steam";
            avatar.enabled = false;
        }
    }

    private async void GetAvatar()
    {
        try
        {
            Steamworks.Data.Image? result = await SteamFriends.GetLargeAvatarAsync(SteamClient.SteamId);
            if (result.HasValue)
            {
                avatar.texture = Convert(result.Value);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public Texture2D Convert(Steamworks.Data.Image image)
    {
        var avatar = new Texture2D((int)image.Width, (int)image.Height, TextureFormat.ARGB32, false);

        avatar.filterMode = FilterMode.Trilinear;

        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                var p = image.GetPixel(x, y);
                avatar.SetPixel(x, (int)image.Height - y, new UnityEngine.Color(p.r / 255.0f, p.g / 255.0f, p.b / 255.0f, p.a / 255.0f));
            }
        }

        avatar.Apply();
        return avatar;
    }
}
