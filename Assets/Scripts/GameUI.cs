using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlador de interfaz de usuario.
/// Crea botones de Reiniciar y Menú por código dentro del panel de victoria
/// si no se asignaron referencias desde el Inspector.
/// </summary>
[RequireComponent(typeof(Canvas))]
public class GameUI : MonoBehaviour
{
    [Header("Referencias opcionales")]
    public GameObject victoryPanel;

    void Start()
    {
        if (victoryPanel == null) return;

        // Asegurar que el panel inicie desactivado
        victoryPanel.SetActive(false);

        // Si no tiene botones hijos, crearlos por código
        if (victoryPanel.transform.childCount == 0)
        {
            CreateVictoryButtons();
        }
    }

    private void CreateVictoryButtons()
    {
        // Contenedor vertical para botones
        GameObject container = new GameObject("ButtonContainer");
        container.transform.SetParent(victoryPanel.transform, false);

        RectTransform containerRect = container.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.5f, 0.5f);
        containerRect.anchorMax = new Vector2(0.5f, 0.5f);
        containerRect.pivot = new Vector2(0.5f, 0.5f);
        containerRect.anchoredPosition = new Vector2(0, -40);
        containerRect.sizeDelta = new Vector2(200, 80);

        VerticalLayoutGroup layout = container.AddComponent<VerticalLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.spacing = 10;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        // Crear botón Reiniciar
        GameObject btnRestart = CreateButton("Reiniciar Nivel", container.transform);
        // Crear botón Menú Principal
        GameObject btnMenu = CreateButton("Menú Principal", container.transform);

        // Asignar acciones a los botones
        Button btnRestartComp = btnRestart.GetComponent<Button>();
        Button btnMenuComp = btnMenu.GetComponent<Button>();

        if (GameManager.Instance != null)
        {
            btnRestartComp.onClick.AddListener(GameManager.Instance.RestartLevel);
            btnMenuComp.onClick.AddListener(GameManager.Instance.GoToMainMenu);
        }
    }

    private GameObject CreateButton(string text, Transform parent)
    {
        GameObject btnObj = new GameObject(text);
        btnObj.transform.SetParent(parent, false);

        RectTransform rect = btnObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(180, 40);

        Image img = btnObj.AddComponent<Image>();
        img.color = new Color(0.2f, 0.6f, 1f, 1f);

        Button btn = btnObj.AddComponent<Button>();

        GameObject textObj = new GameObject("Label");
        textObj.transform.SetParent(btnObj.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        Text txt = textObj.AddComponent<Text>();
        txt.text = text;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        txt.fontSize = 18;
        txt.color = Color.white;

        return btnObj;
    }
}
