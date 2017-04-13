using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Question
{

    public enum Items { KNIFE, AXE, PEN, DESK, LOGS, CANDLE, NONE };
    private string text;
    private Dictionary<Items, Response> subject_response;
    //Can't keep a list of Question objects easily, so just look up on Question ids instead
    private List<int> blocks;
    private int id;

    public static Question.Items NameToEnum(string name)
    {
        return (Items)Enum.Parse(typeof(Items), name);
    }

    public class Response
    {
        public string text;
        public List<int> opens;
        public Response(string text, List<int> opens)
        {
            this.text = text;
            this.opens = opens;
        }
    }

    public Question(int id, string text, Dictionary<Items, Response> subject_response, List<int> blocks)
    {
        this.subject_response = subject_response;
        this.id = id;
        this.text = text;
        this.blocks = blocks;
    }

    public List<int> GetOpens(Items item)
    {
        return subject_response[item].opens;
    }

    public string getResponse(Items item)
    {
        string response = "";
        Response response_object = null;
        subject_response.TryGetValue(item, out response_object);
        if (response_object != null)
        {
            response = response_object.text;
        }
        return response;
    }

    public void addBlock(int toBlockId)
    {
        blocks.Add(toBlockId);
    }

    public bool doesBlock(int qid)
    {
        return blocks.Contains(qid);
    }

    public int getId()
    {
        return id;
    }

    public string getText()
    {
        return text;
    }
}
