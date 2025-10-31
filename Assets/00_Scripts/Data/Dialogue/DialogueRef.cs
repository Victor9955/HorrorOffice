using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "DialogueRef", menuName = "Scriptable Objects/DialogueRef")]
public class DialogueRef : ScriptableObject
{
    [SerializeField] string link;
    public async Task SyncDataAsync()
    {
        UnityWebRequest _webRequest = UnityWebRequest.Get(link);
        Debug.Log("<color=yellow> Starting CSV synchronisation... </color>");
        await _webRequest.SendWebRequest();
        while (!_webRequest.isDone)
        {
            await Task.Delay(500);
            if (_webRequest.result == UnityWebRequest.Result.ConnectionError) Debug.LogError("Web request failed : connection error");
        }
        ParseData(_webRequest.downloadHandler.text);
    }

    void ParseData(string data)
    {

    }
}
