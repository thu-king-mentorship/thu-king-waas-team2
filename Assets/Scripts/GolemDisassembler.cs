using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // Quita esta línea si no usas DOTween

public class GolemDisassembler : MonoBehaviour
{
    public List<Transform> bodyParts = new List<Transform>();
    private Dictionary<Transform, Vector3> originalLocalPositions = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Quaternion> originalLocalRotations = new Dictionary<Transform, Quaternion>();

    public float explosionForce = 300f;
    public float explosionRadius = 3f;
    public float rebuildDelay = 3f;
    public float rebuildDuration = 1.5f;

    private void Start()
    {
        foreach (var part in bodyParts)
        {
            originalLocalPositions[part] = part.localPosition;
            originalLocalRotations[part] = part.localRotation;
        }
    }

    public void OnHit()
    {
        StartCoroutine(ExplodeAndRebuild());
    }

    private IEnumerator ExplodeAndRebuild()
    {
        // Desmontar
        foreach (var part in bodyParts)
        {
            part.parent = null;

            Rigidbody rb = part.GetComponent<Rigidbody>();
            if (rb == null) rb = part.gameObject.AddComponent<Rigidbody>();

            Collider col = part.GetComponent<Collider>();
            if (col == null) col = part.gameObject.AddComponent<BoxCollider>();

            rb.isKinematic = false;
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }

        yield return new WaitForSeconds(rebuildDelay);

        // Reconstruir
        foreach (var part in bodyParts)
        {
            Rigidbody rb = part.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            if (DOTween.IsTweening(part)) DOTween.Kill(part);

            // Usamos DOTween para que vuelva a su sitio
            part.DOMove(transform.TransformPoint(originalLocalPositions[part]), rebuildDuration).SetEase(Ease.InOutQuad);
            part.DORotateQuaternion(originalLocalRotations[part], rebuildDuration).SetEase(Ease.InOutQuad);
        }

        yield return new WaitForSeconds(rebuildDuration);

        // Volver a hacer hijos y limpiar
        foreach (var part in bodyParts)
        {
            part.SetParent(transform);
            part.localPosition = originalLocalPositions[part];
            part.localRotation = originalLocalRotations[part];

            // Opcional: eliminar Rigidbody y Collider si ya no los necesitas
            Destroy(part.GetComponent<Rigidbody>());
            Destroy(part.GetComponent<Collider>());
        }
    }
}
