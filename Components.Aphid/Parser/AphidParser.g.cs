namespace Components.Aphid.Parser {
    
    
    public partial class AphidParser {
        
        private Components.Aphid.Parser.Expression ParseAssignmentExpression() {
            Components.Aphid.Parser.Expression operand = this.ParsePipelineExpression();
            for (
            ; (this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.AssignmentOperator); 
            ) {
                Components.Aphid.Lexer.AphidTokenType op = this._currentToken.TokenType;
                this.NextToken();
                operand = new Components.Aphid.Parser.BinaryOperatorExpression(operand, op, this.ParsePipelineExpression());
            }
            return operand;
        }
        
        private Components.Aphid.Parser.Expression ParsePipelineExpression() {
            Components.Aphid.Parser.Expression operand = this.ParseLogicalExpression();
            for (
            ; (this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.PipelineOperator); 
            ) {
                Components.Aphid.Lexer.AphidTokenType op = this._currentToken.TokenType;
                this.NextToken();
                operand = new Components.Aphid.Parser.BinaryOperatorExpression(operand, op, this.ParseLogicalExpression());
            }
            return operand;
        }
        
        private Components.Aphid.Parser.Expression ParseLogicalExpression() {
            Components.Aphid.Parser.Expression operand = this.ParseComparisonExpression();
            for (
            ; ((this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.AndOperator) 
                        || (this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.OrOperator)); 
            ) {
                Components.Aphid.Lexer.AphidTokenType op = this._currentToken.TokenType;
                this.NextToken();
                operand = new Components.Aphid.Parser.BinaryOperatorExpression(operand, op, this.ParseComparisonExpression());
            }
            return operand;
        }
        
        private Components.Aphid.Parser.Expression ParseComparisonExpression() {
            Components.Aphid.Parser.Expression operand = this.ParsePostfixUnaryOperationExpression();
            for (
            ; ((((((this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.EqualityOperator) 
                        || (this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.NotEqualOperator)) 
                        || (this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.LessThanOperator)) 
                        || (this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.LessThanOrEqualOperator)) 
                        || (this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.GreaterThanOperator)) 
                        || (this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.GreaterThanOrEqualOperator)); 
            ) {
                Components.Aphid.Lexer.AphidTokenType op = this._currentToken.TokenType;
                this.NextToken();
                operand = new Components.Aphid.Parser.BinaryOperatorExpression(operand, op, this.ParsePostfixUnaryOperationExpression());
            }
            return operand;
        }
        
        private Components.Aphid.Parser.Expression ParseBinaryOrExpression() {
            Components.Aphid.Parser.Expression operand = this.ParseXorExpression();
            for (
            ; (this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.BinaryOrOperator); 
            ) {
                Components.Aphid.Lexer.AphidTokenType op = this._currentToken.TokenType;
                this.NextToken();
                operand = new Components.Aphid.Parser.BinaryOperatorExpression(operand, op, this.ParseXorExpression());
            }
            return operand;
        }
        
        private Components.Aphid.Parser.Expression ParseXorExpression() {
            Components.Aphid.Parser.Expression operand = this.ParseBinaryAndExpression();
            for (
            ; (this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.XorOperator); 
            ) {
                Components.Aphid.Lexer.AphidTokenType op = this._currentToken.TokenType;
                this.NextToken();
                operand = new Components.Aphid.Parser.BinaryOperatorExpression(operand, op, this.ParseBinaryAndExpression());
            }
            return operand;
        }
        
        private Components.Aphid.Parser.Expression ParseBinaryAndExpression() {
            Components.Aphid.Parser.Expression operand = this.ParseShiftExpression();
            for (
            ; (this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.BinaryAndOperator); 
            ) {
                Components.Aphid.Lexer.AphidTokenType op = this._currentToken.TokenType;
                this.NextToken();
                operand = new Components.Aphid.Parser.BinaryOperatorExpression(operand, op, this.ParseShiftExpression());
            }
            return operand;
        }
        
        private Components.Aphid.Parser.Expression ParseShiftExpression() {
            Components.Aphid.Parser.Expression operand = this.ParseAdditionExpression();
            for (
            ; ((this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.ShiftLeft) 
                        || (this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.ShiftRight)); 
            ) {
                Components.Aphid.Lexer.AphidTokenType op = this._currentToken.TokenType;
                this.NextToken();
                operand = new Components.Aphid.Parser.BinaryOperatorExpression(operand, op, this.ParseAdditionExpression());
            }
            return operand;
        }
        
        private Components.Aphid.Parser.Expression ParseAdditionExpression() {
            Components.Aphid.Parser.Expression operand = this.ParseTerm();
            for (
            ; ((this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.AdditionOperator) 
                        || (this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.MinusOperator)); 
            ) {
                Components.Aphid.Lexer.AphidTokenType op = this._currentToken.TokenType;
                this.NextToken();
                operand = new Components.Aphid.Parser.BinaryOperatorExpression(operand, op, this.ParseTerm());
            }
            return operand;
        }
        
        private Components.Aphid.Parser.Expression ParseTerm() {
            Components.Aphid.Parser.Expression operand = this.ParsePrefixUnaryOperatorExpression();
            for (
            ; (((this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.MultiplicationOperator) 
                        || (this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.DivisionOperator)) 
                        || (this._currentToken.TokenType == Components.Aphid.Lexer.AphidTokenType.ModulusOperator)); 
            ) {
                Components.Aphid.Lexer.AphidTokenType op = this._currentToken.TokenType;
                this.NextToken();
                operand = new Components.Aphid.Parser.BinaryOperatorExpression(operand, op, this.ParsePrefixUnaryOperatorExpression());
            }
            return operand;
        }
    }
}
