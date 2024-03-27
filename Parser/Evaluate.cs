namespace PSI;
using static Token.E;

// An expression evaluator, implementing using the visitor pattern
class ExprEvaluator : Visitor<int> {
   public ExprEvaluator (Dictionary<string, int> dict) => mDict = dict;

   Dictionary<string, int> mDict;

   public override int Visit (NLiteral literal)
      => int.Parse (literal.Value.Text);

   public override int Visit (NIdentifier identifier)
      => mDict[identifier.Name.Text];

   public override int Visit (NUnary unary) {
      int d = unary.Expr.Accept (this);
      if (unary.Op.Kind == SUB) d = -d;
      return d;
   }

   public override int Visit (NBinary binary) {
      int a = binary.Left.Accept (this), b = binary.Right.Accept (this);
      bool res = false;
      return binary.Op.Kind switch {
         ADD => a + b,
         SUB => a - b,
         MUL => a * b,
         DIV => a / b,
         EQ => ComparisonOperator (EQ),
         NEQ => ComparisonOperator (NEQ),
         LT => ComparisonOperator (LT),
         GT => ComparisonOperator (GT),
         LEQ => ComparisonOperator (LEQ),
         GEQ => ComparisonOperator (GEQ),
         _ => throw new NotImplementedException ()
      };
      int ComparisonOperator (Token.E token) {
         switch (token) {
            case EQ: res = a == b; break;
            case NEQ: res = a != b; break;
            case LT: res = a < b; break;
            case GT: res = a > b; break;
            case LEQ: res = a <= b; break;
            case GEQ: res = a >= b; break;
         }
         return res ? 1 : 0;
      }
   }
}