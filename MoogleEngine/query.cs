namespace MoogleEngine
{
    ///<summary>
    /// En esta clase se hace el trabajo con la query (busqueda del ususario)
    ///</summary>
    public class query_Manage 
    {
        Documents Documents = new Documents();

        ///<summary>
        /// Este metodo normaliza la query (todas las palabras en minuscula,quitar espacios en blanco) y devuelve un array con las palabras
        ///</summary>
        ///<param name="query">
        ///Query asociada a la busqueda actual.
        ///</param>
        public static string[]Text_Query(string query)
        {
            string contenido=query.ToLower();//convierte el contenido a minusculas
            //contenido_minusculas=contenido= new string(contenido_minusculas.Where(c => !char.IsPunctuation(c)).ToArray());//
            string[]newarr=contenido.Split(' ');//elimina los espacios en blanco
            Data_Base.Quitar_Tildes(newarr);
            return newarr;//devulve un array con las palabras del contenido del archivo
        }

        ///<summary>
        /// Este metodo llena las propiedades de la query (considerandolo como un documento mas)
        ///</summary>
        ///<param name="query">
        ///Query asociada a la busqueda actual.
        ///</param>
        public static Documents Query_Data(string query)
        {
            Documents query1 = new Documents();
            query1.id = 0;
            query1.text = query;
            query1.text_words = Data_Base.Text_Words(query1.text);
            query1.term_frequency = Data_Base.T_Frequency(query1.text_words);

            Dictionary<string,int>Vocabulary = Data_Base.Global_Vocabulary;
            int documents_Count = Data_Base.documents.Count;
            foreach(var item in query1.term_frequency.Keys) //aqui se calcula el tf-idf de las palabras de la query
                {
                    if(Vocabulary.ContainsKey(item))
                    {
                        query1.term_frequency[item] = Data_Base.TF_IDF(query1.term_frequency[item],Vocabulary[item],documents_Count);
                    }
                    else
                    {
                        query1.term_frequency[item]=0;
                    }
                }
            return query1;
        }

    }
}