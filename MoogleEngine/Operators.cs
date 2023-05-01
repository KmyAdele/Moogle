using System.Text.RegularExpressions;
using System;
namespace MoogleEngine
{
    ///<summary>
    /// Biblioteca de metodos encargados de procesar todos los operadores de busqueda.
    ///</summary>
    public class Operators
    {
        
        Documents documents = new Documents();
        
            //* - (!) La palabra que lo precede no debe aparecer en ninguno de los documentos devueltos
            //* - (^) La palabra que lo precede debe aparecer obligatoriamente en los documentos devueltos
            //* - (*) La palabra que lo precede tendra mas relevancia en la busqueda
            //* - (~) Los documentos devuletos tendran mas relevancia si las palabras que une este operador estan mas cerca en el mismo (operador de cercania) 
        ///<summary>
        /// Este metodo identifica cual(es) operador(es) se uso(aron) y hace lo correspondiente a cada caso
        ///</summary>
        ///<param name="query">
        ///Palabras de la query asociada a la busqueda actual.
        ///</param>
        ///<param name="query">
        ///Palabras que aparecen en todos los documentos sin repetir.
        ///</param>
        public static void Operators_Identify(string[]query, Dictionary<string, double> Vocabulary)
        {
            Data_Base.Validates = Data_Base.Validates_Text();
            for(int i = 0; i<query.Length; i++)
            {
                if(query[i].Contains('!'))
                {
                    Data_Base.Validates = Doc_Can_Appear(query[i],1);
            
                }
                if(query[i].Contains('^'))
                {
                    Data_Base.Validates = Doc_Can_Appear(query[i],2);
                }
                if(query[i].Contains('*'))
                {
                    int increment = Increment(query[i]);
                    string word = query[i].Substring(increment,query[i].Length-increment);
                    Vocabulary[word] *= increment;

                } 
                if(query[i]=="~")
                {
                    for(int s=0; s<query.Length; s++)
                    {
                        int index = Array.IndexOf(query,"~",s);
                        while(index !=-1)
                        {
                            Near_Operator(query,index);
                            s = index+1;
                            index = Array.IndexOf(query,"~",s);
                        }
                        
                    }
                }
                
            }
            
        }


        //? Este metodo responde a 2 operadores:
        //? - (!) Si el metodo es llamado en la aparicion del (!) entonces el metodo devuelve los documentos donde no aparece la palabra
        //? - (!) Si el metodo es llamado en la aparicion del (^) entonces el metodo devuelve solo los documentos donde aparece la palabra
        ///<summary>
        /// Procesa los operadores asociados a la aparicion o no de palabras en los documentos.
        ///</summary>
        ///<param name="word">
        ///Palabra a identificar si se encuentra o no en los documentos.
        ///</param>
        ///<param name="option">
        ///Tipo de operador que se quiere ejecutar (1: Aparicion de la palabra, 2: No aparicion de la palabra).
        ///</param>
        private static List<int> Doc_Can_Appear(string word,int option)
        {
            word = word.Substring(1,word.Length-1);
            List<int> Docs = new List<int>();
            if(option == 1)
            {
                /*foreach (Documents doc in Data_Base.Document_list)
                {
                    if(!Appear(word,doc.term_frequency))
                    {
                        Docs.Add(doc.id);
                    }
                }*/

                foreach(int id in Data_Base.Validates)
                {
                    Documents doc = Data_Base.Document_list[id];
                    if(!Appear(word,doc.term_frequency))
                    {
                        Docs.Add(doc.id);
                    }
                }
            }
            if(option == 2)
            {
               /* foreach (Documents doc in Data_Base.Document_list)
                {
                    if(Appear(word,doc.term_frequency))
                    {
                        Docs.Add(doc.id);
                    }
                }*/

                foreach(int id in Data_Base.Validates)
                {
                    Documents doc = Data_Base.Document_list[id];
                    if(Appear(word,doc.term_frequency))
                    {
                        Docs.Add(doc.id);
                    }
                }
            }
            return Docs;
        }

        ///<summary>
        /// Metodo auxiliar para identificar si una palabra aparece o no en  el documento
        ///</summary>
        ///<param name="word">
        ///Palabra a buscar.
        ///</param>
        ///<param name="Vocabulary">
        ///Lista de palabras que aparecen en los documentos sin repetir.
        ///</param>
        private static bool Appear (string word, Dictionary<string, double>Vocabulary)
        {
            if(Vocabulary.ContainsKey(word))
            {
                return true;
            }
            return false;
        }

        ///<summary>
        /// Metodo auxiliar que cuenta los (*)
        ///</summary>
        ///<param name="word">
        ///Palabra a procesar.
        ///</param>
        private static int Increment(string word)
        {
            int count = 0;
            for(int i =0; i<word.Length; i++)
            {
                if(word[i] == '*')
                {
                    count++;
                }
            }
            return count;
        }

        ///<summary>
        /// Metodo auxiliar que verifica si un indice es valido en un array
        ///</summary>
        ///<param name="query">
        ///Palabras de la query asociada a la busqueda actual.
        ///</param>
        ///<param name="index">
        ///Indice de la palabra.
        ///</param>
        private bool Index_isValid(string[]query,int index)
        {
            if(index < 0)
            {
                return false;
            }
            else if(index >= query.Length)
            {
                return false;
            }
            return true;
        }

        ///<summary>
        /// Metodo auxiliar que devuelve una Tupla (palabra1,palabra2) que relaciona el operador de cercania
        ///</summary>
        ///<param name="query">
        ///Query asociada a la busqueda actual.
        ///</param>
        ///<param name="index_of_operator">
        ///Indice donde se encuentra ubicado el operador de cercania.
        ///</param>
        private static  (string,string) Detect_NearWord(string[]query,int index_of_operator)
        {
            return (query[index_of_operator-1],query[index_of_operator+1]);
        }

        ///<summary>
        /// Metodo que devuelve una lista con los indices de aparicion de determinada palabra
        ///</summary>
        ///<param name="arr_text">
        ///Textos a realizar la busqueda.
        ///</param>
        ///<param name="word">
        ///Palabra a buscar.
        ///</param>
        private static List<int>Index_of_words(string[]arr_text,string word)
        {
            List<int> Index_word = new List<int>();
            int start = 0;
            int index = Array.IndexOf(arr_text,word,start);
            while(index != -1)
            {
                Index_word.Add(index);
                index = Array.IndexOf(arr_text,word,index+1);      
            }
            return Index_word;
        }

        ///<summary>
        /// Metodo que saca la distancia minima entre dos palabras usando la diferencia de sus indices
        ///</summary>
        ///<param name="query">
        ///Palabras de la query asociada a la busqueda actual.
        ///</param>
        ///<param name="arr_text">
        ///Textos a realizar la busqueda.
        ///</param>
        ///<param name="word1">
        ///Primera palabra a buscar.
        ///</param>
        ///<param name="word2">
        ///Segunda palabra a buscar.
        ///</param>
        private static int Min_Distance_BWords(string[]query,string[]text_arr,string word1, string word2)// Distancia minima entre dos palabras en un texto
        {
            List<int>index_word1 = Index_of_words(text_arr,word1);
            List<int>index_word2 = Index_of_words(text_arr,word2);

            int min_distance = int.MaxValue;
            for(int i=0; i<index_word1.Count; i++)
            {
                for(int j=0; j<index_word2.Count; j++)
                {
                    int distance = Math.Abs(index_word1[i]-index_word2[j]);
                    min_distance = Math.Min(min_distance,distance);
                }
            }
            return min_distance;
        } 

        ///<summary>
        /// Metodo que establece relevancia en dependencia de la cercania de dos palabras unidas por (~)
        ///</summary>
        ///<param name="query">
        ///Query asociada a la busqueda actual.
        ///</param>
        ///<param name="index">
        ///Indice donde se encuentra el operador de ceercania.
        ///</param>
        private static void Near_Operator(string[]query,int index)
        {
            (string,string)Words = Detect_NearWord(query,index);
            string word1 = Words.Item1;
            string word2 = Words.Item2;

            foreach(Documents doc in  Data_Base.Document_list)
            {
                if(doc.term_frequency.Keys.Contains(word1) && doc.term_frequency.Keys.Contains(word2))
                {
                    double min = Min_Distance_BWords(query,doc.text_words, word1, word2);
                    doc.score_operator = (double)(1/min);
                }
            }

        }


    }
}