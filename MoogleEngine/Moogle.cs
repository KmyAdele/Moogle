namespace MoogleEngine;


///<summary>
///Clase principal del programa. Se encarga de manejar la busqueda.
///</summary>
public static class Moogle
{
    static Documents documents = new Documents(); //Clase donde se crea el objeto Documentos y sus propiedades
    static List<Documents> Doc_List; //Lista de los Docuemntos de la Base de Datos (carpeta Content)
    static Dictionary<string,int> Vocabulary; //Diccionario con las palabras unicas de todos los textos asociado a la cantidad de textos donde aparecen
   
    ///<summary>
    /// Metodo que se inicia antes de cargar la pagina, donde se cargan los documentos y sus propiedades
    ///</summary>
    public static void Initialize() 
    {
        Doc_List = Data_Base.Document_list;
        Vocabulary= Data_Base.Global_Vocabulary;
    }    
    
    ///<summary>
    ///Metodo que se encarga de realizar la busqueda.
    ///</summary>
    ///<param name="query">
    ///Query asociada a la busqueda actual.
    ///</param>
    public static SearchResult Query(string query) {
        /*Documents documents = new Documents();
        List<Documents> Doc_List = Data_Base.Document_list;
        Dictionary<string,int> Vocabulary= Data_Base.Global_Vocabulary;*/

        Documents query1 = query_Manage.Query_Data(query); //  query seminormalizada (se mantienen los caracteres que podrian ser operadores)
        Operators.Operators_Identify(query_Manage.Text_Query(query), query1.term_frequency); // Identificacion y trabajo con operadores => (ir a la clase operadores)
        List<Documents> New_Doc = Algorithms.Get_Score(Doc_List,query1); // Lista de documentos con los score
        List<SearchItem>ItemsList=new List<SearchItem>(); 
        int count=0;
        
        //(title,snippet,score)
        //? Agregando los elementos del resultado de la busqueda a ser devueltos en la Lista ItemList
        foreach(var item in New_Doc )
        {
            string snippet = Algorithms.Snippet(query1,item);
            if(snippet != "-1" && Data_Base.Validates.Contains(item.id))
            {
                ItemsList.Add(new SearchItem(item.title,snippet,(float)item.score));
                count++;
            }
           
        }

        SearchItem[] All_Items = ItemsList.OrderByDescending(x => x.Score).ToArray();

        ///<summary>
        ///Reduciendo la cantidad de resultados de busqueda 
        ///</summary>
        ///<remarks>
        ///Para cambiar la cantidad de elemntos mostrados debe cambiar el (n)
        ///</remarks>
        int n = 10;
        SearchItem[] items = new SearchItem[n];
        if(All_Items.Length>10)
        {
            for(int i=0; i<items.Length; i++)
            {
                items[i] = All_Items[i];
            }
        }
        else
        {
            items = All_Items;
        }
        
        string Sugestion = Algorithms.Sugestion(query,Vocabulary);
        return new SearchResult(items, Sugestion);
    }
}
