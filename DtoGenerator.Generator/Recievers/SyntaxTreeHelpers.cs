using Microsoft.CodeAnalysis;

namespace Orders.CodeGen.Recievers;

public static class SyntaxTreeHelpers
{
    public static void RecieveAllNode(this ISyntaxReceiver receiver, SyntaxTree tree)
    {
        if (!tree.TryGetRoot(out var root))
        {
            return;
        }

        receiver.OnVisitSyntaxNode(root);
        foreach (var node in root.ChildNodes())
        {
            RecieveAllNode(receiver,  node);
        }
    }

    public static void RecieveAllNode(this ISyntaxReceiver receiver, SyntaxNode node)
    {
        receiver.OnVisitSyntaxNode(node);
        foreach (var childNode in node.ChildNodes())
        {
            receiver.RecieveAllNode(childNode);
        }
    }
}
