using System.Diagnostics;
using System.Security.Cryptography;
using System.Collections.Generic;
namespace MoogleEngine
{
    class Data_Base
    {
        
        static string [] files = Directory.GetFiles(@"E:\PROYECTO\prueba\Content");//Obtiene los documentos(ubicacion) de tipo txt en un array.

        Documents Documents = new Documents();

        public static List <Documents> documents = List_Documents(files);
        public static Dictionary<string,int> Global_Vocabulary {get; set;} = Global_VocabularyM(documents);

        public static List <Documents> Document_list {get; set;} =TF_IDF(documents,Global_Vocabulary); 

        #region Methods

        static List<Documents> TF_IDF(List<Documents> documents,Dictionary<string,int> Vocabulary)
        {
            foreach(Documents doc in documents)
            {
                foreach(var item in doc.term_frequency.Keys)
                {
                    doc.term_frequency[item] = TF_IDF(doc.term_frequency[item],Global_Vocabulary[item],documents.Count);
                }
            }
            return documents;
        }
        static List<Documents> List_Documents(string[]files) //LLenando la Lista de Documents
        {
            List <Documents> documents = new List<Documents>();
            int id = 0;
            foreach (string file in files)
            {
                documents.Add(Get_Doc_Info(file,id));
                id++;
            }
            return documents;
        }

        static Documents Get_Doc_Info(string rutaArchivo,int id) //Asigna la informacion del documento
        {
            Documents document = new Documents();
            document.id = id;
            document.title = Path.GetFileNameWithoutExtension(rutaArchivo);
            document.text = Text(rutaArchivo);
            document.text_words = Text_Words(document.text);
            document.term_frequency = T_Frequency(document.text_words);
            return document;
        } 
        
        static string Text (string rutaArchivo)
        {
            StreamReader sr = new StreamReader(rutaArchivo);//Lee el archivo
            string contenido = sr.ReadToEnd();//Lee el contenido del archivo
            return contenido;
        }
        public static string[]Text_Words (string Text)//metodo que recibe la ruta del archivo y devuelve el contenido del archivo normalizado
        {
            string contenido_minusculas=Text.ToLower();//convierte el contenido a minusculas
            //contenido_minusculas=contenido= new string(contenido_minusculas.Where(c => !char.IsPunctuation(c)).ToArray());//
            string[]arr_text_words=contenido_minusculas.Split(' ','´',',','\t','\n','|','/','\r','.',':','\\',';','-','_','(',')','[',']','{','}','=','+','*','%','&','^','!','@','#','$','<','>','|','?');//separa contenido en palabras y quita los signos de puntuacion que aparecen en el parentesis
            string[]newarr_text_words=arr_text_words.Where(x=>x!="").ToArray();//elimina los espacios en blanco
            newarr_text_words=Quitar_Tildes(newarr_text_words);
            return newarr_text_words;//devulve un array con las palabras del contenido del archivo
        }

        public static string[]Quitar_Tildes(string[]words)//Metodo que quita las tildes de un array de palabras
        {
            string[]tildes={"á","é","í","ó","ú","Á","É","Í","Ó","Ú"};
            String[]letras={"a","e","i","o","u","A","E","I","O","U"};
            for(int j=0;j<words.Length;j++)
            {
            for(int i=0;i<tildes.Length;i++)
            {
                words[j]=words[j].Replace(tildes[i],letras[i]);
            }
            }
            return words;
        }

        public static string[] Text_Vocabulary(string[]Words)//Metodo que guarda las palabras diferentes de un texto en un array Vocabulario
        {
            string[]vocabulary=Words.ToArray();
            for(int i=0;i<vocabulary.Length;i++)
            {
                for(int j=i+1;j<Words.Length;j++)
                {
                    if(Words[i]==Words[j])
                    {
                        vocabulary[j]=" ";
                    }
                }
            }
            vocabulary=vocabulary.Where(x=>x!=" ").ToArray();//elimina los espacios en blanco
            return vocabulary;
        }

        private static double term_Frequency(string[]Words,string Word)//Metodo que calcula la frecuencia de una palabra en un texto
        {
            double count=0;
            for(int i=0;i<Words.Length;i++){
                if(Words[i]==Word){
                    count++;
                }
            }
            return count;
        }
    
        public static Dictionary <string,double> T_Frequency(string[]words)
        {
            Dictionary<string,double> term_frequency = new Dictionary<string,double>();
            string[]vocabulary = Text_Vocabulary(words);

            for(int i=0; i<vocabulary.Length; i++)
            {
                double tf = (term_Frequency(words,vocabulary[i]))/(words.Length);
                term_frequency.Add(vocabulary[i],tf);
            }
            return term_frequency;
        }

        
        public static int Word_inDoc(string word, List<Documents> documents)
        {
            int count = 0;
            for(int i=0; i<documents.Count; i++)
            {
                if(documents[i].term_frequency.ContainsKey(word))
                {
                    count++;
                }
                
            }
            return count;
        }
        
        public static Dictionary<string,int> Global_VocabularyM(List<Documents> documents)
        {
            Dictionary<string,int> global = new Dictionary<string, int>();
            foreach(Documents doc in documents)
            {
                foreach(var d in doc.term_frequency.Keys)
                {
                    if(!(global.ContainsKey(d)))
                    {
                        global.Add(d,Word_inDoc(d,documents));
                    }
                }
            }
            return global;
        }


        public static double TF_IDF (double tf,double df, int corpus)
            {
                double idf = Math.Log10(corpus/df);
                return tf*idf;
            } 
        
        #endregion   
    
    
    
    }

     
}