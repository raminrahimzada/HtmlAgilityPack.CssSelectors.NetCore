﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HtmlAgilityPack.CssSelectors.NetCore.Selectors;
namespace HtmlAgilityPack.CssSelectors.NetCore;
public abstract class CssSelector
{
    protected CssSelector()
    {
        SubSelectors = [];
    }

    private static readonly CssSelector[] Selectors = FindSelectors();
    public abstract string Token { get; }
    protected virtual bool IsSubSelector => false;
    public virtual bool AllowTraverse => true;
    public IList<CssSelector> SubSelectors { get; set; }
    public string Selector { get; set; }

    protected internal abstract IEnumerable<HtmlNode> FilterCore(IEnumerable<HtmlNode> currentNodes);

    public IEnumerable<HtmlNode> Filter(IEnumerable<HtmlNode> currentNodes)
    {
        var nodes = currentNodes;
        var rt = FilterCore(nodes).Distinct();
        if (SubSelectors.Count == 0)
            return rt;
        foreach (var selector in SubSelectors)
            rt = selector.FilterCore(rt);
        return rt;
    }
    public virtual string GetSelectorParameter(string selector)
    {
        return selector[Token.Length..];
    }
    public static IList<CssSelector> Parse(string cssSelector)
    {
        var tokens = Tokenizer.GetTokens(cssSelector);
        return tokens.Select(ParseSelector).ToList();
    }
    private static CssSelector ParseSelector(Token token)
    {
        var selector = (char.IsLetter(token.Filter[0])
            ? Selectors.First(i => i is TagNameSelector)
            : Selectors.Where(s => s.Token.Length > 0).FirstOrDefault(s => token.Filter.StartsWith(s.Token)))
            ?? throw new InvalidOperationException($"Invalid token : {token.Filter}.");

        var selectorType = selector.GetType();
        var rt = (CssSelector)Activator.CreateInstance(selectorType);
        var filter = token.Filter[selector.Token.Length..];
        rt.SubSelectors = token.SubTokens.Select(ParseSelector).ToList();
        rt.Selector = filter;
        return rt;
    }

    private static CssSelector[] FindSelectors()
    {
        var defaultAsm = typeof(CssSelector).GetTypeInfo().Assembly;
        static bool TypeQuery(Type type) => type.GetTypeInfo().IsSubclassOf(typeof(CssSelector)) && !type.GetTypeInfo().IsAbstract;
        var defaultTypes = defaultAsm.GetTypes().Where(TypeQuery);
        var types = defaultAsm.GetTypes().Where(TypeQuery);
        types = defaultTypes.Concat(types);
        var rt = types.Select(Activator.CreateInstance).Cast<CssSelector>().ToArray();
        return rt;
    }
}