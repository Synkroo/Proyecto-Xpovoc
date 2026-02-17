using System.Collections;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    public static Coroutine RunCoroutine(IEnumerator coroutine)
    {
        GameObject runner = new GameObject("CoroutineRunner");
        CoroutineHelper helper = runner.AddComponent<CoroutineHelper>();
        return helper.StartCoroutine(helper.Execute(coroutine, runner));
    }

    private IEnumerator Execute(IEnumerator coroutine, GameObject runner)
    {
        yield return StartCoroutine(coroutine);
        Destroy(runner);
    }
}
