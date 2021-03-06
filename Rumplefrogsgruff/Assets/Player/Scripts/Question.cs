﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Question
{

    public enum Item { KNIFE, AXE, PEN, DESK, LOGS, CANDLE, RSS, CHAIR, BOOK, NONE };
    private string text, id;
    private Dictionary<Item, Response> subject_response;
    //Can't keep a list of Question objects easily, so just look up on Question ids instead
    private List<int> blocks;
    private List<Item> items_seen = new List<Item>();

    public static Question.Item NameToEnum(string name)
    {
        return (Item)Enum.Parse(typeof(Item), name);
    }

    public class Response
    {
        public string text;
        public List<string> opens;
        public Response(string text, List<string> opens)
        {
            this.text = text;
            this.opens = opens;
        }
    }

    public Question(string id, string text, Dictionary<Item, Response> subject_response, List<int> blocks)
    {
        this.subject_response = subject_response;
        this.id = id;
        this.text = text;
        this.blocks = blocks;
    }

    public List<string> GetOpens(Item item)
    {
        return subject_response[item].opens;
    }

    public string getResponse(Item item)
    {
        string response = "";
        if (!HasSeenItem(item))
        {
            Response response_object = ResponseContainsItem(item);
            if (response_object != null)
            {
                response = response_object.text;
            }
            items_seen.Add(item);
        }
        return response;
    }

    public bool HasSeenItem(Item item){
        return items_seen.Contains(item);
    }

    public Response ResponseContainsItem(Item item){
        Response response_object = null;
        subject_response.TryGetValue(item, out response_object);
        return response_object;
    }

    public Response ResponseContainsItem(GameObject g){
        Item i = QuestionController.GameObjectToItem(g);
        if(i == Item.NONE) {
            return null;
        } else {
            return ResponseContainsItem(i);
        }
    }

    public void addBlock(int toBlockId)
    {
        blocks.Add(toBlockId);
    }

    public bool doesBlock(int qid)
    {
        return blocks.Contains(qid);
    }

    public string getId()
    {
        return id;
    }

    public string getText()
    {
        return text;
    }

}
