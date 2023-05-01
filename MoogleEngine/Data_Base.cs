using System.Diagnostics;
using System.Security.Cryptography;
using System.Collections.Generic;
namespace MoogleEngine
{
    ///<summary>
    /// En esta clase se hace toda la gestion con los documentos que estan en la Carpeta Content
    ///</summary>
    public static class Data_Base
    {
        static string [] files = Directory.GetFiles(@"../Content/"); //Obtiene los documentos(ubicacion) de tipo txt en un array.

        static Documents Documents = new Documents();

        public static List <Documents> documents = List_Documents(files); // Lista de documentos
        public static Dictionary<string,int> Global_Vocabulary {get; set;} = Global_VocabularyM(documents);
        public static List <Documents> Document_list {get; set;} =TF_IDF(documents,Global_Vocabulary); 
        public static List<int> Validates{get; set;} = Validates_Text(); 
        

        //* - Se modifican los documentos (cada texto) llevando las palabras a minuscula,quitando espacios,tildes y caracteres extraños
        //* - Se obtiene la cantidad de veces que se repite una palabra en un determinado documento con el objetivo de calcular el tf
        //* - Se obtiene la cantidad de documentos en donde aparece una palabra determinada con el objetivo de calcular el idf
        //* - Se obtiene el tf y el idf con el objetivo de calcular el tf-idf
        //* - Se llena una lista de tipo (Documents) con todas las propiedades del docuemento para que sea mas facil operar con ellas.

        #region Methods // Implementacion


        ///<summary>
        /// Este metodo Llena la lista de Documentos (documents)
        ///</summary>
        ///<param name="files">
        /// Lista de las direcciones de los documentos
        ///</param>
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

        ///<summary>
        /// Este metodo verifica cuales son los textos validos para ser devueltos 
        ///</summary>
        public static List<int> Validates_Text()
        {
            List<int> Valid = new List<int>();
            foreach(Documents doc in Document_list)
            {
                Valid.Add(doc.id);
            }
            return Valid;
        }


        ///<summary>
        /// Este metodo obtiene cada una de las propiedades de los Documentos y se las asigna a los mismos
        ///</summary>
        ///<param name="rutaArchivo">
        ///Direccion donde se encuentra el archivo .txt
        ///</param>
        ///<param name="id">
        ///Id correspondiente al documento
        ///</param>
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
        
        ///<summary>
        /// Este metodo lee los textos de la ruta de archivo proporcionada y devuelve el texto en forma de string
        ///</summary>
        ///<param name="rutaArchivo">
        ///Direccion donde se encuentra el archivo .txt
        ///</param>
        static string Text (string rutaArchivo)
        {
            StreamReader sr = new StreamReader(rutaArchivo);//Lee el archivo
            string contenido = sr.ReadToEnd();//Lee el contenido del archivo
            return contenido;
        }
        

        ///<summary>
        /// Este metodo recibe un texto y devuelve el contenido del texto normalizado (en minuscula, sin tildes, sin caracteres raros) en foram de array
        ///</summary>
        ///<param name="Text">
        ///Texto a procesar
        ///</param>
        public static string[]Text_Words (string Text)
        {
            string contenido_minusculas=Text.ToLower();//convierte el contenido a minusculas
            //contenido_minusculas=contenido= new string(contenido_minusculas.Where(c => !char.IsPunctuation(c)).ToArray());//
            string[]arr_text_words=contenido_minusculas.Split(' ','´',',','\t','\n','|','/','\r','.',':','\\',';','-','_','(',')','[',']','{','}','=','+','*','%','&','^','!','@','#','$','<','>','|','?');//separa contenido en palabras y quita los signos de puntuacion que aparecen en el parentesis
            string[]newarr_text_words=arr_text_words.Where(x=>x!="").ToArray();//elimina los espacios en blanco
            Quitar_Tildes( newarr_text_words);
            return newarr_text_words;//devulve un array con las palabras del contenido del archivo
        }

        ///<summary>
        /// Este metodo quita las tildes de un array de palabras
        ///</summary>
        ///<param name="words">
        ///Palabras a procesar.
        ///</param>
       public static void Quitar_Tildes( string[]words)
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
        }


        ///<summary>
        /// Este metodo guarda solo las palabras unicas (diferentes) en un array Vocabulary
        ///</summary>
        ///<param name="Words">
        ///Palabras a procesar.
        ///</param>
        public static string[] Text_Vocabulary(string[]Words)
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

        ///<summary>
        /// Metodo que calcula la cantidad de veces que se repite una palabra en un texto
        ///</summary>
        ///<param name="Words">
        ///Palabras del texto a procesar.
        ///</param>
        ///<param name="Word">
        ///Palabra a buscar.
        ///</param>
        private static double term_Frequency(string[]Words,string Word)
        {
            double count=0;
            for(int i=0;i<Words.Length;i++){
                if(Words[i]==Word){
                    count++;
                }
            }
            return count;
        }
    
        ///<summary>
        /// Metodo que calcula el tf de una palabra en el documento 
        ///( tf = cantidad de veces que se repite la palabra en el texto/total de palabras del texto )
        ///</summary>
        ///<param name="words">
        ///Palabras del texto a procesar.
        ///</param>
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

        ///<summary>
        /// Metodo que cuenta la cantidad de textos donde aparece una palabra determinada
        ///</summary>
        ///<param name="word">
        ///Palabra a buscar.
        ///</param>
        ///<param name="documents">
        ///Lista de documentos a realizar la busqueda.
        ///</param>
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
        

        ///<summary>
        /// Metodo que llena un diccionario (diccionario vocabulario global y en la cantidad de textos q se encuentra cada palabra)
        ///</summary>
        ///<param name="documents">
        ///Lista de documentos a realizar la busqueda.
        ///</param>
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


        ///<summary>
        /// Este metodo calcula el TF_IDF
        ///</summary>
        ///<param name="tf">
        ///Cantidad de veces que se repite la palabra en el documento actual.
        ///</param>
        ///<param name="df">
        ///Cantidad de documentos en donde aparece la palabra.
        ///</param>
        ///<param name="corpus">
        ///Cantidad de documentos en total.
        ///</param>
        public static double TF_IDF (double tf,double df, int corpus)
            {
                double idf = (double)Math.Log(corpus / df+1); //calcula el idf
                return (tf*idf);
            } 

        
        ///<summary>
        /// Este metodo agrega al diccionario term_frecuency de cada doc el tf_idf de cada palabra de dicho texto
        ///</summary>
        ///<param name="documents">
        ///Lista de documentos.
        ///</param>
        ///<param name="Vocabulary">
        ///Lista de palabras que aparecen en todos los documentos, sin repetir.
        ///</param>
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
        
        #endregion   
    
    }

     
}