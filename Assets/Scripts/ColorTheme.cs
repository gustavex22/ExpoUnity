using UnityEngine;

/// <summary>
/// Sistema de colores por código para diferenciar objetos del laberinto
/// sin necesidad de importar assets externos. Compatible con URP y Built-in.
/// </summary>
public static class ColorTheme
{
    public static readonly Color PlayerColor   = new Color(0.2f, 0.6f, 1f, 1f);   // Azul cian
    public static readonly Color CoinColor     = new Color(1f, 0.85f, 0.2f, 1f);  // Oro
    public static readonly Color WallColor     = new Color(0.4f, 0.4f, 0.45f, 1f);// Gris oscuro
    public static readonly Color FloorColor    = new Color(0.3f, 0.5f, 0.3f, 1f); // Verde hierba
    public static readonly Color WallAltColor  = new Color(0.6f, 0.2f, 0.2f, 1f); // Rojo ladrillo

    /// <summary>
    /// Crea o reutiliza un material con el color especificado y lo asigna al MeshRenderer.
    /// Intenta usar un shader compatible con URP primero, con fallback a Standard.
    /// </summary>
    public static void ApplyColor(MeshRenderer renderer, Color color)
    {
        if (renderer == null) return;

        Material mat = null;

        // Si el renderer ya tiene un material, clonarlo para conservar el shader correcto (URP/Lit, etc.)
        if (renderer.sharedMaterial != null)
        {
            mat = new Material(renderer.sharedMaterial);
        }
        else
        {
            // Buscar shader URP primero, si no, fallback a Standard
            Shader urpShader = Shader.Find("Universal Render Pipeline/Lit");
            Shader fallbackShader = Shader.Find("Standard");
            Shader shaderToUse = urpShader != null ? urpShader : fallbackShader;

            if (shaderToUse != null)
            {
                mat = new Material(shaderToUse);
            }
        }

        if (mat != null)
        {
            mat.color = color;
            mat.name = $"Mat_{color.r:F2}_{color.g:F2}_{color.b:F2}";
            renderer.material = mat;
        }
    }
}
