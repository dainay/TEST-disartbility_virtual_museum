using UnityEngine;

public class GreenRaycastClickHandler : MonoBehaviour
{
    public float raycastRange = 10f; // Distance du Raycast

    private Light sceneLight; // Lumière principale
    private float originalIntensity; // Intensité originale

    private Light[] objectLights1; // Groupe Fruits
    private Light[] objectLights2; // Groupe Animaux
    private Light[] objectLights3; // Groupe Objets

    private bool isGroup1On = false;
    private bool isGroup2On = false;
    private bool isGroup3On = false;

    void Start()
    {
        // Trouver la lumière principale
        GameObject lightObject = GameObject.Find("lightscene");
        if (lightObject != null)
        {
            sceneLight = lightObject.GetComponent<Light>();
            originalIntensity = sceneLight.intensity;
        }
        else
        {
            Debug.LogWarning("Aucune lumière nommée 'lightscene' trouvée !");
        }

        // Définition des groupes d'objets
        string[] objectNames1 = { "bougie", "banane", "feuille" };
        string[] objectNames2 = { "violette", "cuillere", "orange" };
        string[] objectNames3 = { "book", "ballon", "potion" };

        // Initialisation des lumières avec des couleurs différentes
        objectLights1 = InitializeLights(objectNames1, Color.yellow);  // Fruits -> Jaune
        objectLights2 = InitializeLights(objectNames2, Color.blue);    // Animaux -> Bleu
        objectLights3 = InitializeLights(objectNames3, Color.green);   // Objets -> Vert
    }

    Light[] InitializeLights(string[] objectNames, Color color)
    {
        Light[] lights = new Light[objectNames.Length];

        for (int i = 0; i < objectNames.Length; i++)
        {
            GameObject obj = GameObject.Find(objectNames[i]);
            if (obj != null)
            {
                Light objLight = obj.GetComponentInChildren<Light>(); // Recherche la lumière
                if (objLight != null)
                {
                    lights[i] = objLight;
                    objLight.intensity = 0; // Éteint par défaut
                    objLight.color = color; // Applique la couleur
                }
                else
                {
                    Debug.LogWarning($"Aucune lumière trouvée dans '{objectNames[i]}' !");
                }
            }
            else
            {
                Debug.LogWarning($"Aucun objet nommé '{objectNames[i]}' trouvé !");
            }
        }
        return lights;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, raycastRange))
            {
                string objectName = hit.collider.gameObject.name;

                if (objectName == "Interrupteur1")
                {
                    ToggleGroup(ref isGroup1On, objectLights1);
                }
                else if (objectName == "Interrupteur2")
                {
                    ToggleGroup(ref isGroup2On, objectLights2);
                }
                else if (objectName == "Interrupteur3")
                {
                    ToggleGroup(ref isGroup3On, objectLights3);
                }
            }
        }
    }

    void ToggleGroup(ref bool groupState, Light[] objectLights)
    {
        groupState = !groupState;
        ToggleLights(objectLights, groupState);

        sceneLight.intensity = (isGroup1On || isGroup2On || isGroup3On) ? 0 : originalIntensity;

        Debug.Log($"Lumière principale {(sceneLight.intensity == 0 ? "éteinte" : "rallumée")}");
    }

    void ToggleLights(Light[] lights, bool state)
    {
        foreach (Light objLight in lights)
        {
            if (objLight != null)
            {
                objLight.intensity = state ? 10 : 0;
            }
        }

        Debug.Log(state ? "Lumières allumées" : "Lumières éteintes");
    }
}
