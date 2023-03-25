using System.Reflection.Metadata;
using System;
namespace MoogleEngine
{
    class Algorithms
    {
        Documents Documents = new Documents();
        public static double Similarity(Documents document,Documents query)
        {
            double suma_numerador=0;
            double suma_denom1=0;
            double suma_denom2=0;
            
            foreach (var item in query.term_frequency.Keys)
            {
                if(document.term_frequency.ContainsKey(item))
                {
                    suma_numerador+=document.term_frequency[item]*query.term_frequency[item];
                    suma_denom1 += Math.Pow(document.term_frequency[item],2);
                    suma_denom2 += Math.Pow(query.term_frequency[item],2);
                }
                
            }
            if((suma_denom1*suma_denom2 == 0))// quitar la division por 0
            {
                return -1;
            }

            double score = (suma_numerador)/(Math.Sqrt(suma_denom1*suma_denom2));
            return score;
        }
            
        public static List<Documents> Get_Score(List<Documents>documents,Documents query)
        {
            foreach(var doc in documents)
            {
                if(Similarity(doc,query) != -1)
                {
                    doc.score = Similarity(doc,query);
                }
            }
            return documents;
        }

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

        public static string Sugestion(string query,Dictionary<string,int>Global_Vocabulary)
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


        static string Substring(string s,int startIndex,int length)
        {
            if(startIndex+length > s.Length)
            {
                length = s.Length - startIndex;
                // throw new ArgumentOutOfRangeException("Longitud fuera del rango");
            }
            char[] result = new char[length];
            for(int i=0; i<length; i++)
            {
                result[i]=s[startIndex+i];
            }
            return new string(result);
        }

        public static string Snippet(Documents query,Documents Text)
        {
            int index=0;
            string snippet = "-1";
            for(int i=0; i<query.text_words.Length;i++)
            {
                if(Text.term_frequency.ContainsKey(query.text_words[i]))
                {
                    index=Text.text.IndexOf(query.text_words[i]);
                    snippet = Substring(Text.text,index,500);
                    return snippet;
                }
            }
            return snippet;
        }

  
    }

}