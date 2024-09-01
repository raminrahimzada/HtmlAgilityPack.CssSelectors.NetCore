namespace HtmlAgilityPack.CssSelectors.NetCore.PseudoClassSelectors;
[PseudoClassName("nth-of-type")]
internal class NthOfTypePseudoClass : PseudoClass
{
    protected override bool CheckNode(HtmlNode node, string parameter)
    {
        var ofType = node.Name;
        return node.GetIndexOnParent(ofType) == int.Parse(parameter) - 1;
    }
}