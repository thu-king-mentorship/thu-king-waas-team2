using UnityEngine;

public class EyeCube : MonoBehaviour
{
    // Referencias a los dos materiales
    public Material material1;  // Material original
    public Material material2;  // Material para reemplazar

    private Renderer cubeRenderer;  // Componente Renderer del cubo
    private Light cubeLight;        // Componente Light del cubo

    private bool isInLightZone = false;  // Bandera que indica si el cubo est� dentro del rango de luz
    private bool isMaterial1 = true;     // Bandera que indica cu�l material est� asignado actualmente

    void Start()
    {
        // Obtener el componente Renderer y Light del objeto al iniciar
        cubeRenderer = GetComponent<Renderer>();
        cubeLight = GetComponentInChildren<Light>();  // Suponiendo que la luz es un hijo del cubo

        // Aseg�rate de que el material1 se est� utilizando al inicio
        if (cubeRenderer != null && material1 != null)
        {
            cubeRenderer.material = material1;  // Asignamos el primer material al cubo
        }

        // Aseg�rate de que la luz est� apagada inicialmente
        if (cubeLight != null)
        {
            cubeLight.enabled = false;  // Apagar la luz inicialmente
        }
    }

    void Update()
    {
        // Si el cubo est� dentro de la zona de luz, enciende la luz
        if (isInLightZone && cubeLight != null)
        {
            cubeLight.enabled = true;  // Encender la luz
        }
        else if (!isInLightZone && cubeLight != null)
        {
            cubeLight.enabled = false;  // Apagar la luz
        }
    }

    // M�todo para intercambiar el material
    public void ChangeMaterial()
    {
        if (cubeRenderer != null)
        {
            if (isMaterial1 && material2 != null)
            {
                // Si el material actual es material1, cambiamos a material2
                cubeRenderer.material = material2;
                isMaterial1 = false;  // Cambiamos la bandera
            }
            else if (!isMaterial1 && material1 != null)
            {
                // Si el material actual es material2, cambiamos a material1
                cubeRenderer.material = material1;
                isMaterial1 = true;  // Cambiamos la bandera
            }
        }
    }

}