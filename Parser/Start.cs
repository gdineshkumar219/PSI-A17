using Parser;
namespace PSI;

static class Start {
   static void Main () {
      var parser = new Parser (new Tokenizer (Expr0));
      var node = parser.Parse ();
      var dict = new Dictionary<string, int> () { ["five"] = 5, ["two"] = 2 };
      var sb = node.Accept (new ExprILGen ());
      var et = new ExprTyper ();
      var expr = new ExprEvaluator(dict);
      if (node.Accept (et) != NType.Error) {
         if (node.Accept (et) is NType.Bool) {
            bool result = node.Accept (expr) == 1;
            Console.WriteLine (result);
         } else {
            int value = node.Accept (expr);
            Console.WriteLine ($"Value = {value}");
         }
         ExprGrapher newGraph = new ();
         _ = node.Accept (newGraph);
         string filePath = "../Parser/Data/output.html";
         newGraph.WriteToHtmlFile (Expr0,filePath);
         Console.WriteLine ("\nGenerated code: ");
         Console.WriteLine (sb);
      } else Console.WriteLine ("Input a proper expression");
   }
   static string Expr0
      = "(3 + 2) * 4 - 17 * -five * (two + 1 + 4 + 5)";
}