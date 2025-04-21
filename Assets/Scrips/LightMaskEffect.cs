using UnityEngine;

[RequireComponent(typeof(Camera))]
public class LightMaskEffect : MonoBehaviour
{
    public Material effectMaterial;
    public Transform mascaraObjeto;
    public float radius = 0.2f;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (effectMaterial == null || mascaraObjeto == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        // Obtener posición en pantalla normalizada [0..1]
        Vector3 screenPos = Camera.main.WorldToViewportPoint(mascaraObjeto.position);
        effectMaterial.SetVector("_LightPos", new Vector2(screenPos.x, screenPos.y));
        effectMaterial.SetFloat("_Radius", radius);

        Graphics.Blit(source, destination, effectMaterial);
    }
}

