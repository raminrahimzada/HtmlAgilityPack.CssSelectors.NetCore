﻿using System;
using System.Collections.Generic;
using System.Linq;
namespace HtmlAgilityPack.CssSelectors.NetCore;
public static partial class HapCssExtensionMethods
{
    public static IEnumerable<HtmlNode> GetChildElements(this HtmlNode node)
    {
        return node.ChildNodes.Where(i => i.NodeType == HtmlNodeType.Element);
    }
    public static IList<string> GetClassList(this HtmlNode node)
    {
        var attr = node.Attributes["class"];
        if(attr == null)
            return [];
        return attr.Value.Split([' ', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
    }
    public static int GetIndexOnParent(this HtmlNode node)
    {
        int idx = 0;
        foreach(var n in node.ParentNode.GetChildElements())
        {
            if(n == node)
                return idx;
            idx++;
        }
        throw new InvalidOperationException("Node not found in its parent!");
    }
    public static int GetIndexOnParent(this HtmlNode node, string tagName)
    {
        int idx = 0;
        foreach(var n in node.ParentNode.GetChildElements())
        {
            if(!string.Equals(tagName, n.Name, StringComparison.OrdinalIgnoreCase))
                continue;
            if(n == node)
                return idx;
            idx++;
        }
        throw new InvalidOperationException("Node not found in its parent!");
    }
    public static HtmlNode NextSiblingElement(this HtmlNode node)
    {
        var rt = node.NextSibling;
        while(rt != null && rt.NodeType != HtmlNodeType.Element)
            rt = rt.NextSibling;
        return rt;
    }
    public static HtmlNode PreviousSiblingElement(this HtmlNode node)
    {
        var rt = node.PreviousSibling;
        while(rt != null && rt.NodeType != HtmlNodeType.Element)
            rt = rt.PreviousSibling;
        return rt;
    }
}