using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Burst.Intrinsics.X86.Avx;

public static class TextHelper 
{
    
    public static void ShowUpwardText(string text, Vector3 position, Color color, TMPro.FontStyles fontStyle = TMPro.FontStyles.Normal)
    {
        GameObject textObject = new GameObject("textObject");
        textObject.transform.position = position;
        TextMeshPro textMesh = textObject.AddComponent<TextMeshPro>();
        textMesh.text = text;
        textMesh.color = color;
        textMesh.fontStyle = fontStyle;
        textMesh.fontSize = 6;
        textMesh.alignment = TextAlignmentOptions.Center;

        TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>("Fonts/medieval-pixel SDF");
        textMesh.font = fontAsset;

        textMesh.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
        textMesh.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);

        textMesh.fontMaterial.EnableKeyword("UNDERLAY_ON");
        textMesh.fontMaterial.SetColor(ShaderUtilities.ID_UnderlayColor, Color.black);
        textMesh.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayOffsetX, 2f);
        textMesh.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayOffsetY, -2f);
        textMesh.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlaySoftness, 0.5f);
        MeshRenderer renderer = textObject.GetComponent<MeshRenderer>();
        renderer.sortingLayerName = "UI";
        CoroutineHelper.RunCoroutine(UpwardText(position, textObject));
    }

    public static IEnumerator UpwardText(Vector3 startPosition, GameObject textObject)
    {

        float elapsed = 0;
        float duration = 3f;

        while (elapsed < duration)
        {
            textObject.transform.position = Vector3.Lerp(startPosition, startPosition + 1 * Vector3.up, elapsed / duration);
            TextMeshPro textMesh = textObject.GetComponent<TextMeshPro> ();
            Color color = textMesh.color;
            color.a = Mathf.Lerp(1, 0, elapsed / duration);
            textMesh .color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }

        Object.Destroy(textObject, 1f);
    }
}
