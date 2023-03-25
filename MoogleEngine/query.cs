namespace MoogleEngine
{
    public class query_Manage
    {
        Documents Documents = new Documents();

        public static Documents Query_Data(string query)
        {
            Documents query1 = new Documents();
            query1.id = 0;
            query1.text = query;
            query1.text_words = Data_Base.Text_Words(query1.text);
            query1.term_frequency = Data_Base.T_Frequency(query1.text_words);

            Dictionary<string,int>Vocabulary = Data_Base.Global_Vocabulary;
            int documents_Count = Data_Base.documents.Count;
            foreach(var item in query1.term_frequency.Keys)
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