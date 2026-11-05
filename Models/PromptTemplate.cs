namespace AsistenteIA.Models;

public record PromptTemplate(string Name, string Category, string Text);

public static class PromptTemplates
{
    public static readonly List<PromptTemplate> All =
    [
        new("Explicar un concepto", "Estudio", "Explica con palabras sencillas y un ejemplo: "),
        new("Resumir un tema", "Estudio", "Resume en cinco ideas clave el tema: "),
        new("Generar preguntas de práctica", "Evaluación", "Crea cinco preguntas de práctica con su respuesta sobre: "),
        new("Corregir redacción", "Escritura", "Corrige ortografía y claridad de este párrafo: "),
        new("Comparar conceptos", "Estudio", "Compara en una tabla las diferencias entre: "),
    ];
}
