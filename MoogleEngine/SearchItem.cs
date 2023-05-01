namespace MoogleEngine;

///<summary>
///Objeto que representa cada documento que forma parte del resultado de la busqueda.
///</summary>
public class SearchItem
{
    ///<summary>
    /// Constructor que crea un objeto valido para el resultado de la busqueda.
    ///</summary>
    ///<param name="title">
    ///Titulo del documento.
    ///</param>
    ///<param name="snippet">
    ///Pequenio texto descriptivo del documento.
    ///</param>
    ///<param name="score">
    ///Puntuacion por la cual se ordenan los resultados.
    ///</param>
    public SearchItem(string title, string snippet, float score)
    {
        this.Title = title;
        this.Snippet = snippet;
        this.Score = score;
    }

    public string Title { get; private set; }

    public string Snippet { get; private set; }

    public float Score { get; private set; }
}
