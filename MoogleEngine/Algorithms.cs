using System.Reflection.Metadata;
using System;
namespace MoogleEngine
{
    ///<summary>
    ///Biblioteca de metodos algoritmicos
    ///</summary>
    class Algorithms
    {
        Documents Documents = new Documents();
        
        //* - Se usa la similitud coseno con el peso del query(tf-idf de las palabras del query) y el del documento en cuestion (tf-idf de las palabras del documento)

        //? Hciendo uso de la Lista Doc_List 
                //*             |doc1|doc2|doc3|.....|docN|
                //*       word1| A11 | A12 |A13 |.....|A1N|
                //*       word2| A21 | A22 |A23 |.....|A2N|
                //*       word3| A31 | A32 |A33 |.....|A3N|
                //*        ... |... | ...  |... |.....|...|
                //*       wordM| AM1 | AM2 |AM3 |.....|AMN|
        //? Siendo doc1,doc2,...,docN (los elementos de la lista (documentos))
        //? Siendo word1,word2,...wordN (las palabras del diccionario (term_frecuency) de cada elemnto de la lista)
        //? Siendo ANM (los valores de tf-idf de cada palabra de cada elemento (docuemento)de la lista)

        ///<summary>
        /// Este metodo saca el score (relevancia) de un documento con respecto a una busqueda determinada
        ///</summary>
        ///<param name="document">
        ///Documento a calcular su similitud con la query.
        ///</param>
        ///<param name="query">
        ///Query asociada a la busqueda actual.
        ///</param>
        public static double Similarity(Documents document,Documents query)
        {
            double suma_numerador=0;
            double suma_denom1=0;
            double suma_denom2=0;
            
            foreach (var item in query.term_frequency.Keys)
            {
                if(document.term_frequency.ContainsKey(item))
                {
                    double wordtfidf = document.term_frequency[item];
                    suma_numerador += document.term_frequency[item] * query.term_frequency[item];
                    suma_denom1 += document.term_frequency[item] * document.term_frequency[item];
                    suma_denom2 += query.term_frequency[item] * query.term_frequency[item];
                }
                
            }

            //double score = (suma_numerador)/(double)(Math.Sqrt(suma_denom1 * suma_denom2)+1);
            double a1 = (double)(Math.Sqrt(suma_denom1));
            double a2 = (double)(Math.Sqrt(suma_denom2));
            double score = (double)(suma_numerador)/(a1 * a2 +1);
            
            return score;
        }
            
        
        ///<summary>
        /// Este metodo asigna a cada documento (en la propiedad score) su relevancia (score) sobre la busqueda hecha (query)
        ///</summary>
        ///<param name="documents">
        ///Documento a calcular su similitud con la query.
        ///</param>
        ///<param name="query">
        ///Query asociada a la busqueda actual.
        ///</param>
        public static List<Documents> Get_Score(List<Documents>documents,Documents query)
        {
            //List<Documents> NewDoc = new List<Documents>();

           
           foreach(var doc in documents)
            {
                if(Similarity(doc,query) != -1)
                {
                    doc.score = Similarity(doc,query);
                }
            }
            return documents;

            /*foreach(var id in Data_Base.Validates)
            {
                if(Similarity(documents[id],query) != -1)
                {
                    documents[id].score = Similarity(documents[id],query)+documents[id].score_operator;
                    NewDoc.Add(documents[id]);
                }
            }
            return NewDoc;*/
        }



        ///<summary>
        /// Este metodo calcula la similitud entre dos palabras devolviendo la cantidad de cambios que hay que hacer 
        ///en una de ellas para convertirla en la otra.
        ///</summary>
        ///<param name="a">
        ///Primera palabra.
        ///</param>
        ///<param name="a">
        ///Segunda palabra.
        ///</param>
        static int  Levenshte_in_Distance(string a, string b)
        {
        if (String.IsNullOrEmpty(a) && String.IsNullOrEmpty(b))
        {
            return 0;
        }
        if (String.IsNullOrEmpty(a)) 
        {
            return b.Length;
        }
        if (String.IsNullOrEmpty(b)) 
        {
            return a.Length;
        }
        int  lengthA   = a.Length;
        int  lengthB   = b.Length;
        var  distances = new int[lengthA + 1, lengthB + 1];
        for (int i = 0;  i <= lengthA;  distances[i, 0] = i++);
        for (int j = 0;  j <= lengthB;  distances[0, j] = j++);

        for (int i = 1;  i <= lengthA;  i++)
        {

            for (int j = 1;  j <= lengthB;  j++)
                {
                int  cost = b[j - 1] == a[i - 1] ? 0 : 1;
                distances[i, j] = Math.Min
                    (
                    Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                    distances[i - 1, j - 1] + cost
                    );
                }
        }
        return distances[lengthA, lengthB];
        }


        ///<summary>
        /// Este metodo devuelve una sugerencia para una palabra erroneamente escrita
        ///</summary>
        ///<remarks>
        ///Se calcula la palabra/texto mas similar a la busqueda erronea, usando como referencia las 
        ///palabras del vocabulario de los textos de la Base de Datos
        ///</remarks>
        ///<param name="query">
        ///Query asociada a la busqueda actual.
        ///</param>
        ///<param name="Global_Vocabulary">
        ///Vocabulario que contiene todas las palabras de todos los documentos.
        ///</param>
        public static string Sugestion(string query, Dictionary<string,int>Global_Vocabulary)
        {
            string[]queryarr=query.Split(' ');
            string word = "";

            for(int i=0;i<queryarr.Length;i++)
            {   
                double similarity=double.MaxValue;
                if(!Global_Vocabulary.ContainsKey(queryarr[i]))
                {
                    string aux = "";
                    foreach(var a in Global_Vocabulary)
                    {
                        double similarity2=Levenshte_in_Distance(queryarr[i],a.Key);
                        if(similarity>similarity2)
                        {
                            similarity=similarity2;
                            aux = a.Key;
                        }
                    }
                    queryarr[i] = aux;
                }
                
            }
            word = string.Join(' ',queryarr);
            return word;
        }


        ///<summary>
        ///Este metodo saca un snippet (porcion de texto) por documento 
        ///</summary>
        ///<param name="query">
        ///Query asociada a la busqueda actual.
        ///</param>
        ///<param name="Text">
        ///Documento a procesar.
        ///</param>
        public static string Snippet(Documents query,Documents Text)
        {
            int index=0, start=0;
            string snippet = "-1";
            for(int i=0; i<query.text_words.Length;i++)
            {
                if(Text.term_frequency.ContainsKey(query.text_words[i]))
                {
                    string sentence = "";

                    start = 0;
                    Text.text += '.';
                    index = Text.text.IndexOf('.');

                    while (index != -1)
                    {
                        sentence = Text.text.Substring(start, index - start + 1);
                        string[] words = Data_Base.Text_Words(sentence);

                        if (Array.IndexOf(words, query.text_words[i]) != -1)
                        {
                            snippet = Text.text.Substring(start, IndexValid(Text.text.Length, start));
                            
                            //snippet = "<mark>" + snippet.Substring(0, 10) + "</mark>" + snippet.Substring(10);

                            return snippet;
                        }

                        start = index + 1;
                        index = Text.text.IndexOf('.', start);
                    }
                }
            }

            return snippet;
        }

        ///<summary>
        ///Este metodo verifica si el indice es valido
        ///</summary>
        ///<remarks>
        ///Un indice es valido si se encuentra en un rango valido dentro del tamanio del texto.
        ///</remarks>
        ///<param name="text_size">
        ///Tamanio del texto.
        ///</param>
        ///<param name="index">
        ///Indice a comprobar.
        ///</param>
        private static int IndexValid(int text_size,int index)
        {
            if((index+300)>=text_size)
            {
                return text_size-index-1;
            }
            return 300;
        }

  
    }

}