namespace MoogleEngine;

///<summary>
/// Objeto que guarda el resultado de la busqueda.
///</summary>
public class SearchResult
{
    private SearchItem[] items;

    ///<summary>
    /// Constructor que crea el resultado a la busqueda actual.
    ///</summary>
    ///<param name="items">
    ///Objetos de tipo SearchItem que pertenecen al resultado de la busqueda.
    ///</param>
    ///<param name="suggestion">
    ///Sugerencia en caso de cometer algun error de escritura para realizar la busqueda.
    ///</param>
    public SearchResult(SearchItem[] items, string suggestion="")
    {
        if (items == null) {
            throw new ArgumentNullException("items");
        }

        this.items = items;
        this.Suggestion = suggestion;
    }

    public SearchResult() : this(new SearchItem[0]) {

    }

    public string Suggestion { get; private set; }

    public IEnumerable<SearchItem> Items() {
        return this.items;
    }

    public int Count { get { return this.items.Length; } }
}
