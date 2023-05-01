namespace MoogleEngine
{
    ///<summary>
    /// Objeto tipo documento.
    ///</summary>
    public class Documents
    {
        public int id; //Identificador del documento (0....hasta la cantidad de docs-1); 
        public string title; // Titulo  del docuemento
        public string text; // Texto sin normalizar del documento
        public string [] text_words; // Array con las palabras del texto sin repetir y normalizadaas
        public Dictionary<string,double> term_frequency; // Diccionario <palabra,tf-idf> 
        public double score; // Puntuacion (relevancia) del texto
        public double score_operator; // Variable auxiliar para gestionar el impacto de los operadores sobre la relevancia del documento


    }
}