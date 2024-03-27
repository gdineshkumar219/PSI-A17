using PSI;
using static PSI.NType;

namespace Parser;
class ExprTyper : Visitor<NType> {
   public override NType Visit (NLiteral literal) {
      string exprTxt = literal.Value.Text;
      if (exprTxt.Contains ('.') || exprTxt.Contains ('e') || exprTxt.Contains ('E')) literal.Type = Real;
      else if (exprTxt is "true" or "false") literal.Type = Bool;
      else literal.Type = Int;
      return literal.Type;
   }

   public override NType Visit (NIdentifier identifier) {
      return identifier.Type = Int;
   }

   public override NType Visit (NUnary unary) {
      NType exprType = unary.Expr.Accept (this);
      return unary.Op.Text switch {
         "-" => exprType == Int || exprType == Real ? exprType : Error,
         _ => Error,
      };
   }

   public override NType Visit (NBinary binary) {
      NType a = binary.Left.Accept (this);
      NType b = binary.Right.Accept (this);
      if (binary.Op.Text is "<" or ">" or ">=" or "<=" or "<>" or "=" && (a == b || a is Bool || b is Bool)) binary.Type = Bool;
      else if (a is Int && b is Int) binary.Type = Int;
      else if (a is Int or Real && b is Real or Int) binary.Type = Real;
      else binary.Type = Error;
      return binary.Type;
   }

   private bool IsComparisonOperator (Token op) {
      return op.Kind switch {
         Token.E.EQ => true,
         Token.E.NEQ => true,
         Token.E.GT => true,
         Token.E.LT => true,
         Token.E.GEQ => true,
         Token.E.LEQ => true,
         _ => false
      };
   }
}