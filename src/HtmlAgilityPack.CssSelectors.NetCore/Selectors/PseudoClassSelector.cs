﻿using System.Collections.Generic;
namespace HtmlAgilityPack.CssSelectors.NetCore.Selectors;
internal class PseudoClassSelector : CssSelector
{
    public override string Token => ":";
    protected internal override IEnumerable<HtmlNode> FilterCore(IEnumerable<HtmlNode> currentNodes)
    {
        var values = Selector.TrimEnd(')').Split(['('], 2);
        var pseudoClass = PseudoClass.GetPseudoClass(values[0]);
        var value = values.Length > 1 ? values[1] : null;
        return pseudoClass.Filter(currentNodes, value);
    }
}
