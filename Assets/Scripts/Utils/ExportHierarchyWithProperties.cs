using System.IO;
using UnityEngine;

public class ExportHierarchyWithComponents : MonoBehaviour
{
    private void Start()
    {
        string path = Application.dataPath + "/HierarchyExport.txt";
        using (StreamWriter writer = new StreamWriter(path))
        {
            foreach (GameObject obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            {
                WriteHierarchy(obj.transform, writer, 0);
            }
        }

        Debug.Log("Hiérarchie exportée vers : " + path);
    }

    private void WriteHierarchy(Transform obj, StreamWriter writer, int level)
    {
        // Écrire le nom du GameObject avec indentation
        writer.WriteLine(new string('-', level * 2) + obj.name);

        // Récupérer et écrire les composants attachés
        Component[] components = obj.GetComponents<Component>();
        foreach (Component comp in components)
        {
            if (comp != null)
                writer.WriteLine(new string(' ', (level + 1) * 2) + "- " + comp.GetType().Name);
        }

        // Explorer les enfants
        foreach (Transform child in obj)
        {
            WriteHierarchy(child, writer, level + 1);
        }
    }
}
