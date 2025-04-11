using UnityEngine;
using System.Collections.Generic;

public class GreenCheckObjects : MonoBehaviour
{
    private Light spotLight;
    private bool isObjectPlaced = false;

    private static HashSet<string> requiredObjects = new HashSet<string> { "violette", "ballon", "feuille" };
    private static HashSet<string> placedObjects = new HashSet<string>();

    public GreenCameraLookAt cameraScript; // 🎥 Référence au script de la caméra

    void Start()
    {
        spotLight = transform.Find("SpotLight")?.GetComponent<Light>();

        if (spotLight == null)
        {
            Debug.LogWarning("⚠️ Aucune lumière 'SpotLight' trouvée !");
        }
        else
        {
            spotLight.intensity = 0;
        }

        if (cameraScript == null)
        {
            Debug.LogError("❌ CameraLookAt n'est pas assigné dans CheckObjects !");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (spotLight == null) return;

        string objName = other.gameObject.name;

        // Si l'objet appartient au joueur, on ne le traite pas
        if (other.gameObject.CompareTag("Player")) return;

        // Si l'objet fait partie des objets requis
        if (requiredObjects.Contains(objName))
        {
            // Si l'objet n'est pas encore dans la liste des objets placés
            if (!placedObjects.Contains(objName))
            {
                spotLight.intensity = 100;
                spotLight.color = Color.green;  // Lumière verte
                placedObjects.Add(objName);
                Debug.Log($"✅ {objName} ajouté !");
            }
        }
        else
        {
            // Si l'objet n'est pas requis et est posé, on affiche la couleur rouge
            spotLight.intensity = 100;
            spotLight.color = Color.red;  // Lumière rouge
            isObjectPlaced = true;
        }

        // Vérifie la condition de victoire
        CheckWinCondition();
    }

    void OnTriggerExit(Collider other)
    {
        if (spotLight == null) return;
        string objName = other.gameObject.name;

        if (isObjectPlaced)
        {
            isObjectPlaced = false;
            spotLight.intensity = 0;
            Debug.Log("💡 Objet retiré, la lumière s'éteint !");
        }

        placedObjects.Remove(objName);
        CheckWinCondition();
    }

    void CheckWinCondition()
    {
        // Si tous les objets nécessaires sont placés correctement
        if (placedObjects.SetEquals(requiredObjects))
        {
            Debug.Log("🎉 VICTOIRE ! Tous les objets corrects sont placés !");

            // ⏳ Attendre 3 secondes avant d'exécuter Victory()
            Invoke("Victory", 3f);
        }
    }

    void Victory()
    {
        Debug.Log("🏆 Félicitations ! La caméra va tourner !");

        if (cameraScript != null)
        {
            cameraScript.StartCameraRotation();
        }
        else
        {
            Debug.LogError("❌ La caméra n'est pas assignée dans l'Inspector !");
        }
    }
}
