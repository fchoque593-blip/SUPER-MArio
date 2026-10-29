
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Text;
using System.Linq;

public class DiagnosticarRecursosMario
{
    [MenuItem("Mario/1 - Revisar todos los recursos")]
    public static void RevisarRecursos()
    {
        StringBuilder informe = new StringBuilder();

        informe.AppendLine("INVENTARIO DEL PROYECTO SUPER MARIO");
        informe.AppendLine("====================================");

        Agregar(informe, "PREFABS", "t:Prefab");
        Agregar(informe, "SPRITES", "t:Sprite");
        Agregar(informe, "TEXTURAS", "t:Texture2D");
        Agregar(informe, "TILES", "t:Tile");
        Agregar(informe, "ESCENAS", "t:Scene");

        string ruta = "Assets/ReporteRecursosMario.txt";

        File.WriteAllText(ruta, informe.ToString(), Encoding.UTF8);

        AssetDatabase.Refresh();

        Debug.Log("INVENTARIO CREADO: " + ruta);

        EditorUtility.RevealInFinder(
            Path.GetFullPath(ruta)
        );
    }

    private static void Agregar(
        StringBuilder informe,
        string titulo,
        string filtro)
    {
        informe.AppendLine();
        informe.AppendLine("========== " + titulo + " ==========");

        var recursos = AssetDatabase.FindAssets(filtro)
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Where(path => path.StartsWith("Assets/"))
            .OrderBy(path => path);

        foreach (string recurso in recursos)
        {
            informe.AppendLine(recurso);
        }
    }
}
