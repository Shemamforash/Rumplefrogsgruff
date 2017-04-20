using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public static class FileReader
{
    public static List<Question> Read()
    {
        List<Question> imported_questions = new List<Question>();
        TextAsset questions = (TextAsset)Resources.Load("questions");
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(questions.text);
        XmlNodeList dataNodes = xmldoc.SelectNodes("/Questions/Question");
        foreach (XmlNode node in dataNodes)
        {
            int id = int.Parse(node.SelectSingleNode("ID").InnerText);
            string text = node.SelectSingleNode("Text").InnerText;
            XmlNodeList responses = node.SelectNodes("Responses/Response");
            Dictionary<Question.Item, Question.Response> response_dictionary = new Dictionary<Question.Item, Question.Response>();
            foreach (XmlNode response_node in responses)
            {
                string subject = response_node.SelectSingleNode("Subject").InnerText;
                Question.Item subject_enum = Question.NameToEnum(subject);
                string response_text = response_node.SelectSingleNode("Text").InnerText;
                string[] open_questions = response_node.SelectSingleNode("OpenQuestion").InnerText.Split(',');
                List<int> open_list = new List<int>();
                foreach (string open_question in open_questions)
                {
                    try
                    {
                        open_list.Add(int.Parse(open_question));
                    }
                    catch { }
                }
                Question.Response response_object = new Question.Response(response_text, open_list);
                response_dictionary[subject_enum] = response_object;
            }
            XmlNodeList blocked_questions = node.SelectNodes("BlockedQuestions");
            List<int> blocked_list = new List<int>();
            foreach (XmlNode blocked_question in blocked_questions)
            {
                try
                {
                    int blocked_question_id = int.Parse(blocked_question.InnerText);
                    blocked_list.Add(blocked_question_id);
                }
                catch { } //not relevant if the try block fails- just means there were no blocked questions
            }

            Question new_question = new Question(id, text, response_dictionary, blocked_list);
            imported_questions.Add(new_question);
        }
        return imported_questions;
    }
}
