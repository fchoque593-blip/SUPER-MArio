
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.IO;

public class GenerarEscenasMario : EditorWindow
{
    private const string CarpetaEscenas = "Assets/Scenes/";
    private const string CarpetaPrefabs = "Assets/Prefabs/";

    [MenuItem("Mario/Generador completo de niveles")]
    public static void Abrir()
    {
        GetWindow<GenerarEscenasMario>("Niveles Mario");
    }

    private void OnGUI()
    {
        GUILayout.Label("GENERADOR DE NIVELES MARIO",
            EditorStyles.boldLabel);

        EditorGUILayout.HelpBox(
            "Genera las escenas 1-2, 1-3, 1-4 y GameOver. " +
            "No modifica 1-1.",
            MessageType.Info);

        if (GUILayout.Button("GENERAR SUBTERRÁNEO 1-2", GUILayout.Height(35)))
            Confirmar("1-2", CrearSubterraneo);

        if (GUILayout.Button("GENERAR ACUÁTICO 1-3", GUILayout.Height(35)))
            Confirmar("1-3", CrearAcuatico);

        if (GUILayout.Button("GENERAR CASTILLO 1-4", GUILayout.Height(35)))
            Confirmar("1-4", CrearCastillo);

        if (GUILayout.Button("GENERAR GAME OVER", GUILayout.Height(35)))
            Confirmar("GameOver", CrearGameOver);
    }

    private static void Confirmar(string nombre, System.Action accion)
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        string ruta = CarpetaEscenas + nombre + ".unity";

        if (File.Exists(ruta) &&
            !EditorUtility.DisplayDialog(
                "Regenerar escena",
                "Se reemplazará el diseño actual de " + nombre +
                ". La escena 1-1 no se modifica.",
                "Continuar", "Cancelar"))
            return;

        accion();
    }

    private static void NuevaEscena()
    {
        EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);
    }

    private static GameObject ObtenerPrefab(string nombre)
    {
        string ruta = CarpetaPrefabs + nombre + ".prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(ruta);

        if (prefab == null)
            Debug.LogWarning("No se encontró el prefab: " + ruta);

        return prefab;
    }

    private static GameObject Poner(string nombre, float x, float y)
    {
        GameObject prefab = ObtenerPrefab(nombre);

        if (prefab == null)
            return null;

        GameObject instancia =
            PrefabUtility.InstantiatePrefab(prefab) as GameObject;

        if (instancia != null)
            instancia.transform.position = new Vector3(x, y, 0f);

        return instancia;
    }

    private static void Fila(string prefab, int inicio, int fin, float y)
    {
        for (int x = inicio; x <= fin; x++)
            Poner(prefab, x, y);
    }

    private static void Monedas(int inicio, int fin, float y)
    {
        for (int x = inicio; x <= fin; x += 2)
            Poner("Coin", x, y);
    }

    private static void Enemigo(string prefab, params int[] posiciones)
    {
        for (int i = 0; i + 1 < posiciones.Length; i += 2)
            Poner(prefab, posiciones[i], posiciones[i + 1]);
    }

    private static void PrepararCamara(Color fondo, float zoom = 5f)
    {
        GameObject cam = new GameObject("Main Camera");
        cam.tag = "MainCamera";
        cam.transform.position = new Vector3(0f, 2f, -10f);

        Camera componente = cam.AddComponent<Camera>();
        componente.orthographic = true;
        componente.orthographicSize = zoom;
        componente.backgroundColor = fondo;

        cam.AddComponent<AudioListener>();

        // Solo se agrega el seguimiento si el script está disponible.
        CameraSeguirMario seguimiento =
            cam.AddComponent<CameraSeguirMario>();

        seguimiento.desplazamientoY = 0f;
    }

    private static void MarioInicial()
    {
        Poner("Mario", 2f, 0f);
    }

    private static void Guardar(string nombre)
    {
        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            AssetDatabase.CreateFolder("Assets", "Scenes");

        string ruta = CarpetaEscenas + nombre + ".unity";

        EditorSceneManager.SaveScene(
            SceneManager.GetActiveScene(), ruta);

        List<EditorBuildSettingsScene> escenas =
            new List<EditorBuildSettingsScene>(
                EditorBuildSettings.scenes);

        bool existe = false;

        for (int i = 0; i < escenas.Count; i++)
        {
            if (escenas[i].path == ruta)
            {
                escenas[i] = new EditorBuildSettingsScene(ruta, true);
                existe = true;
                break;
            }
        }

        if (!existe)
            escenas.Add(new EditorBuildSettingsScene(ruta, true));

        EditorBuildSettings.scenes = escenas.ToArray();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Escena guardada: " + ruta);
        EditorUtility.DisplayDialog(
            "Generación finalizada",
            "Se guardó la escena " + nombre + ".",
            "Aceptar");
    }

    // ---------------- SUBTERRÁNEO ----------------

    private static void CrearSubterraneo()
    {
        NuevaEscena();
        PrepararCamara(new Color(0.025f, 0.025f, 0.09f));

        Fila("UndergroundBlock", -3, 100, -1.5f);

        Fila("UndergroundBlock", 0, 14, 7f);
        Fila("UndergroundBlock", 18, 32, 7f);
        Fila("UndergroundBlock", 36, 50, 7f);
        Fila("UndergroundBlock", 54, 68, 7f);
        Fila("UndergroundBlock", 72, 86, 7f);
        Fila("UndergroundBlock", 90, 100, 7f);

        Fila("UndergroundBrick", 6, 10, 2f);
        Fila("UndergroundBrick", 15, 19, 3.5f);
        Fila("UndergroundBrick", 24, 29, 2f);
        Fila("UndergroundBrick", 34, 39, 3.5f);
        Fila("UndergroundBrick", 44, 49, 2f);
        Fila("UndergroundBrick", 58, 63, 3.5f);
        Fila("UndergroundBrick", 70, 75, 2f);
        Fila("UndergroundBrick", 82, 87, 3.5f);
        Fila("UndergroundBrick", 93, 97, 2f);

        Monedas(5, 11, 3f);
        Monedas(23, 29, 3f);
        Monedas(43, 49, 3f);
        Monedas(69, 75, 3f);
        Monedas(92, 98, 3f);

        Poner("MysteryBlock", 12, 3f);
        Poner("MysteryBlock", 32, 4f);
        Poner("MysteryBlock", 52, 3f);
        Poner("MysteryBlock", 78, 4f);

        Poner("Pipe", 20, -0.5f);
        Poner("Pipe", 47, -0.5f);
        Poner("Pipe", 76, -0.5f);

        Enemigo("Goomba", 13, 0, 27, 0, 40, 0, 61, 0, 84, 0);
        Enemigo("Koopa", 31, 0, 55, 0, 90, 0);

        MarioInicial();
        Guardar("1-2");
    }

    // ---------------- ACUÁTICO ----------------

    private static void CrearAcuatico()
    {
        NuevaEscena();
        PrepararCamara(new Color(0.02f, 0.25f, 0.68f), 5.5f);

        Fila("Ground", -3, 100, -2f);

        Fila("Brick", 5, 9, 0f);
        Fila("Brick", 13, 17, 1.5f);
        Fila("Brick", 21, 25, 3f);
        Fila("Brick", 29, 33, 1.5f);
        Fila("Brick", 37, 42, 0f);
        Fila("Brick", 46, 51, 2f);
        Fila("Brick", 55, 60, 3.5f);
        Fila("Brick", 64, 69, 1.5f);
        Fila("Brick", 73, 78, 0f);
        Fila("Brick", 82, 87, 2.5f);
        Fila("Brick", 91, 96, 1f);

        Monedas(4, 10, 1.5f);
        Monedas(20, 26, 4f);
        Monedas(36, 42, 1f);
        Monedas(54, 60, 4.5f);
        Monedas(72, 78, 1f);
        Monedas(90, 96, 3f);

        Poner("MysteryBlock", 16, 3f);
        Poner("MysteryBlock", 39, 2f);
        Poner("MysteryBlock", 66, 3f);
        Poner("MysteryBlock", 85, 4f);

        Poner("Pipe", 30, -1f);
        Poner("Pipe", 68, -1f);
        Poner("Pipe", 98, -1f);

        Enemigo("Koopa", 18, -1, 33, -1, 49, -1, 66, -1, 84, -1);
        Enemigo("Goomba", 11, -1, 27, -1, 44, -1, 61, -1, 78, -1, 94, -1);

        MarioInicial();
        Guardar("1-3");
    }

    // ---------------- CASTILLO ----------------

    private static void CrearCastillo()
    {
        NuevaEscena();
        PrepararCamara(new Color(0.12f, 0.025f, 0.035f));

        Fila("Ground", -3, 105, -1.5f);

        Fila("HardBlock", 7, 12, 1.5f);
        Fila("HardBlock", 17, 22, 3f);
        Fila("HardBlock", 27, 32, 1.5f);
        Fila("HardBlock", 37, 42, 3.5f);
        Fila("HardBlock", 47, 52, 1.5f);
        Fila("HardBlock", 57, 62, 3f);
        Fila("HardBlock", 67, 72, 1.5f);
        Fila("HardBlock", 77, 82, 3.5f);
        Fila("HardBlock", 87, 92, 1.5f);
        Fila("HardBlock", 96, 101, 3f);

        Fila("HardBlock", 14, 15, 0.5f);
        Fila("HardBlock", 14, 15, 1.5f);
        Fila("HardBlock", 34, 35, 0.5f);
        Fila("HardBlock", 34, 35, 1.5f);
        Fila("HardBlock", 54, 55, 0.5f);
        Fila("HardBlock", 54, 55, 1.5f);
        Fila("HardBlock", 74, 75, 0.5f);
        Fila("HardBlock", 74, 75, 1.5f);
        Fila("HardBlock", 94, 95, 0.5f);
        Fila("HardBlock", 94, 95, 1.5f);

        for (int x = 4; x <= 100; x += 8)
            Poner("Brick", x, 4.5f);

        Monedas(4, 10, 3.5f);
        Monedas(24, 30, 3.5f);
        Monedas(44, 50, 3.5f);
        Monedas(64, 70, 3.5f);
        Monedas(84, 90, 3.5f);

        Poner("MysteryBlock", 10, 2.5f);
        Poner("MysteryBlock", 30, 2.5f);
        Poner("MysteryBlock", 50, 2.5f);
        Poner("MysteryBlock", 70, 2.5f);
        Poner("MysteryBlock", 90, 2.5f);

        Enemigo("Goomba", 10, 0, 23, 0, 39, 0, 49, 0, 63, 0, 79, 0, 90, 0);
        Enemigo("Koopa", 19, 0, 35, 0, 58, 0, 75, 0, 98, 0);

        Poner("Pipe", 42, -0.5f);
        Poner("Pipe", 82, -0.5f);
        Poner("Castle", 102, 1f);

        MarioInicial();
        Guardar("1-4");
    }

    // ---------------- GAME OVER ----------------

    private static void CrearGameOver()
    {
        NuevaEscena();
        PrepararCamara(Color.black);

        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();

        CrearTexto("GAME OVER", canvasObj.transform,
            new Vector2(0.5f, 0.68f), new Vector2(800, 150), 72);

        CrearTexto("TE HAS QUEDADO SIN VIDAS", canvasObj.transform,
            new Vector2(0.5f, 0.53f), new Vector2(1000, 100), 36);

        GameObject botonObj = new GameObject("BotonVolverAJugar");
        botonObj.transform.SetParent(canvasObj.transform, false);

        RectTransform rect = botonObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.35f);
        rect.anchorMax = new Vector2(0.5f, 0.35f);
        rect.sizeDelta = new Vector2(360, 85);

        Image imagen = botonObj.AddComponent<Image>();
        imagen.color = new Color(0.8f, 0.08f, 0.08f);

        Button boton = botonObj.AddComponent<Button>();

        CrearTexto("VOLVER A JUGAR", botonObj.transform,
            new Vector2(0.5f, 0.5f), new Vector2(350, 80), 28);

        GameObject controladorObj = new GameObject("ControladorGameOver");
        GameOverUI controlador = controladorObj.AddComponent<GameOverUI>();

        // No usa UnityEditor.Events.UnityEventTools.
        // La acción se conecta en tiempo de ejecución.
        ControladorBotonGameOver conector =
            botonObj.AddComponent<ControladorBotonGameOver>();
        conector.controlador = controlador;
        conector.boton = boton;

        GameObject eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<StandaloneInputModule>();

        Guardar("GameOver");
    }

    private static void CrearTexto(
        string contenido, Transform padre,
        Vector2 ancla, Vector2 tamano, int fuente)
    {
        GameObject obj = new GameObject(contenido);
        obj.transform.SetParent(padre, false);

        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = ancla;
        rect.anchorMax = ancla;
        rect.sizeDelta = tamano;
        rect.anchoredPosition = Vector2.zero;

        Text texto = obj.AddComponent<Text>();
        texto.text = contenido;
        texto.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        texto.fontSize = fuente;
        texto.alignment = TextAnchor.MiddleCenter;
        texto.color = Color.white;
        texto.horizontalOverflow = HorizontalWrapMode.Wrap;
        texto.verticalOverflow = VerticalWrapMode.Overflow;
    }
}
