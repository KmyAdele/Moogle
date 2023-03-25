namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query) {
        // Modifique este método para responder a la búsqueda

        Documents documents = new Documents();
        List<Documents> Doc_List = Data_Base.Document_list;
        Dictionary<string,int> Vocabulary= Data_Base.Global_Vocabulary;
        Documents query1 = query_Manage.Query_Data(query);
        List<Documents> New_Doc = Algorithms.Get_Score(Doc_List,query1);
        List<SearchItem>ItemsList=new List<SearchItem>();
        int count=0;
        
        //(title,snippet,score)
        foreach(var item in New_Doc )
        {
            if(count>=10)
            {
                break;
            }
            string snippet = Algorithms.Snippet(query1,item);
            if(snippet != "-1")
            {
                ItemsList.Add(new SearchItem(item.title,snippet,(float)item.score));
                count++;
            }
           
        }

        SearchItem[] items = ItemsList.OrderByDescending(x => x.Score).ToArray();
        string Sugestion = Algorithms.Sugestion(query,Vocabulary);
        return new SearchResult(items, Sugestion);
    }
}
