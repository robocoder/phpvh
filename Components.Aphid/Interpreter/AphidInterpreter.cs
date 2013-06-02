using Components.Aphid.Lexer;
using Components.Aphid.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Components.Aphid.Interpreter
{
    public class AphidInterpreter
    {
        private const string _return = "$r";

        private bool _isReturning = false;

        private bool _isBreaking = false;

        private AphidLoader _loader;

        public AphidLoader Loader
        {
            get { return _loader; }
        }

        private AphidScope _currentScope;

        public AphidScope CurrentScope
        {
            get { return _currentScope; }
        }

        private OperatorHelper _opHelper;

        public AphidInterpreter()
        {
            Init();

            _currentScope = new AphidScope(new AphidObject());
            _currentScope.Variables.Add(
                "__initList",
                ValueHelper.Wrap(new AphidFunction()
                {
                    Args = new[] { "x" },
                    Body = new List<Expression>()
                }));

            _currentScope.Variables.Add(
                "__initString",
                ValueHelper.Wrap(new AphidFunction()
                {
                    Args = new[] { "x" },
                    Body = new List<Expression>()
                }));
        }

        public AphidInterpreter(AphidScope currentScope)
        {
            Init();
            _currentScope = currentScope;
        }

        private void Init()
        {
            _opHelper = new OperatorHelper(CallInitFunction);
            _loader = new AphidLoader(this);
        }

        private AphidObject CompareDecimals(BinaryOperatorExpression expression, Func<decimal, decimal, bool> equal)
        {
            return new AphidObject(
                equal(
                    (decimal)ValueHelper.Unwrap(InterpretExpression(expression.LeftOperand)),
                    (decimal)ValueHelper.Unwrap(InterpretExpression(expression.RightOperand))));
        }

        private AphidObject InterpretAndExpression(BinaryOperatorExpression expression)
        {
            var left = (bool)ValueHelper.Unwrap(InterpretExpression(expression.LeftOperand));
            var right = (bool)ValueHelper.Unwrap(InterpretExpression(expression.RightOperand));
            return new AphidObject(left && right);
        }

        private AphidObject InterpretOrExpression(BinaryOperatorExpression expression)
        {
            var left = (bool)ValueHelper.Unwrap(InterpretExpression(expression.LeftOperand));
            var right = (bool)ValueHelper.Unwrap(InterpretExpression(expression.RightOperand));
            return new AphidObject(left || right);
        }

        private AphidObject InterpretEqualityExpression(BinaryOperatorExpression expression)
        {
            var left = InterpretExpression(expression.LeftOperand) as AphidObject;
            var right = InterpretExpression(expression.RightOperand) as AphidObject;

            bool val = left.Value != null ?
                left.Value.Equals(right.Value) :
                (null == right.Value && left.Count == 0 && right.Count == 0);

            if (expression.Operator == AphidTokenType.NotEqualOperator)
            {
                val = !val;
            }

            return new AphidObject(val);
        }

        private object InterpretMemberExpression(BinaryOperatorExpression expression, bool returnRef = false)
        {
            var obj = InterpretExpression(expression.LeftOperand) as AphidObject;

            string key;

            if (expression.RightOperand is IdentifierExpression)
            {
                key = (expression.RightOperand as IdentifierExpression).Identifier;
            }
            else if (expression.RightOperand is StringExpression)
            {
                key = (string)ValueHelper.Unwrap(InterpretStringExpression(expression.RightOperand as StringExpression));
            }
            else if (expression.RightOperand is DynamicMemberExpression)
            {
                var memberExp = expression.RightOperand as DynamicMemberExpression;
                key = ValueHelper.Unwrap(InterpretExpression(memberExp.MemberExpression)).ToString();
            }
            else
            {
                throw new AphidRuntimeException("Unexpected expression {0}", expression.RightOperand);
            }

            if (returnRef)
            {
                return new AphidRef() { Name = key, Object = obj };
            }
            else
            {
                AphidObject val;

                if (!obj.TryGetValue(key, out val))
                {
                    throw new AphidRuntimeException("Undefined member {0} in expression {1}", key, expression);
                }

                return val;
            }
        }

        private object InterpetAssignmentExpression(BinaryOperatorExpression expression, bool returnRef = false)
        {
            var value = InterpretExpression(expression.RightOperand);
            if (expression.LeftOperand is IdentifierExpression)
            {
                var id = (expression.LeftOperand as IdentifierExpression).Identifier;
                var destObj = InterpretIdentifierExpression(expression.LeftOperand as IdentifierExpression);

                if (destObj == null)
                {
                    _currentScope.Variables.Add(id, ValueHelper.Wrap(value));
                }
                else
                {
                    destObj.Value = ValueHelper.Unwrap(value);
                }
            }
            else
            {
                var objRef = InterpretBinaryOperatorExpression(expression.LeftOperand as BinaryOperatorExpression, true) as AphidRef;

                if (objRef.Object == null)
                {
                    throw new AphidRuntimeException("Undefined variable {0}", expression.LeftOperand);
                }
                else if (objRef.Object.ContainsKey(objRef.Name))
                {
                    objRef.Object[objRef.Name].Value = ValueHelper.Unwrap(value);
                }
                else
                {
                    objRef.Object.Add(objRef.Name, ValueHelper.Wrap(value));
                }
            }

            return value;
        }

        private object InterpretBinaryOperatorExpression(BinaryOperatorExpression expression, bool returnRef = false)
        {
            switch (expression.Operator)
            {
                case AphidTokenType.AdditionOperator:
                    return _opHelper.Add(
                        InterpretExpression(expression.LeftOperand) as AphidObject,
                        InterpretExpression(expression.RightOperand) as AphidObject);

                case AphidTokenType.MinusOperator:
                    return _opHelper.Subtract(
                        InterpretExpression(expression.LeftOperand) as AphidObject,
                        InterpretExpression(expression.RightOperand) as AphidObject);

                case AphidTokenType.MultiplicationOperator:
                    return _opHelper.Multiply(
                        InterpretExpression(expression.LeftOperand) as AphidObject,
                        InterpretExpression(expression.RightOperand) as AphidObject);

                case AphidTokenType.DivisionOperator:
                    return _opHelper.Divide(
                        InterpretExpression(expression.LeftOperand) as AphidObject,
                        InterpretExpression(expression.RightOperand) as AphidObject);

                case AphidTokenType.MemberOperator:
                    return InterpretMemberExpression(expression, returnRef);                    

                case AphidTokenType.AssignmentOperator:
                    return InterpetAssignmentExpression(expression, returnRef);

                case AphidTokenType.NotEqualOperator:
                case AphidTokenType.EqualityOperator:
                    return InterpretEqualityExpression(expression);

                case AphidTokenType.LessThanOperator:
                    return CompareDecimals(expression, (x, y) => x < y);

                case AphidTokenType.LessThanOrEqualOperator:
                    return CompareDecimals(expression, (x, y) => x <= y);

                case AphidTokenType.GreaterThanOperator:
                    return CompareDecimals(expression, (x, y) => x > y);

                case AphidTokenType.GreaterThanOrEqualOperator:
                    return CompareDecimals(expression, (x, y) => x >= y);

                case AphidTokenType.AndOperator:
                    return InterpretAndExpression(expression);

                case AphidTokenType.OrOperator:
                    return InterpretOrExpression(expression);

                case AphidTokenType.ModulusOperator:
                    return _opHelper.Mod(
                        InterpretExpression(expression.LeftOperand) as AphidObject,
                        InterpretExpression(expression.RightOperand) as AphidObject);

                case AphidTokenType.ShiftLeft:
                    return _opHelper.BinaryShiftLeft(
                        InterpretExpression(expression.LeftOperand) as AphidObject,
                        InterpretExpression(expression.RightOperand) as AphidObject);

                case AphidTokenType.ShiftRight:
                    return _opHelper.BinaryShiftRight(
                        InterpretExpression(expression.LeftOperand) as AphidObject,
                        InterpretExpression(expression.RightOperand) as AphidObject);

                case AphidTokenType.BinaryAndOperator:
                    return _opHelper.BinaryAnd(
                        InterpretExpression(expression.LeftOperand) as AphidObject,
                        InterpretExpression(expression.RightOperand) as AphidObject);

                case AphidTokenType.BinaryOrOperator:
                    return _opHelper.BinaryOr(
                        InterpretExpression(expression.LeftOperand) as AphidObject,
                        InterpretExpression(expression.RightOperand) as AphidObject);

                case AphidTokenType.XorOperator:
                    return _opHelper.Xor(
                        InterpretExpression(expression.LeftOperand) as AphidObject,
                        InterpretExpression(expression.RightOperand) as AphidObject);

                case AphidTokenType.PipelineOperator:
                    return InterpretCallExpression(new CallExpression(expression.RightOperand, expression.LeftOperand));

                default:
                    throw new AphidRuntimeException("Unknown operator {0} in expression {1}", expression.Operator, expression);
            }
        }

        private AphidObject InterpretObjectExpression(ObjectExpression expression)
        {
            var obj = new AphidObject();

            foreach (var kvp in expression.Pairs)
            {
                var id = (kvp.LeftOperand as IdentifierExpression).Identifier;
                var value = ValueHelper.Wrap(InterpretExpression(kvp.RightOperand));
                obj.Add(id, value);
            }

            return obj;
        }

        private AphidObject InterpretIdentifierExpression(IdentifierExpression expression, AphidScope scope = null)
        {
            if (scope == null)
            {
                scope = _currentScope;
            }

            AphidObject obj;
            if (scope.Variables.TryGetValue(expression.Identifier, out obj))
            {
                return obj;
            }
            else if (scope.Parent != null)
            {
                return InterpretIdentifierExpression(expression, scope.Parent);
            }
            else
            {
                return null;
            }
        }

        private AphidObject InterpretStringExpression(StringExpression expression)
        {
            return CallInitFunction(new AphidObject(StringParser.Parse(expression.Value)));            
        }

        public AphidObject CallFunction(string name, params object[] parms)
        {
            var val = InterpretIdentifierExpression(new IdentifierExpression(name));
            var func = ValueHelper.Unwrap(val) as AphidFunction;
            return CallFunction(func, parms);
        }

        public AphidObject CallFunction(AphidFunction function, params object[] parms)
        {
            return CallFunctionCore(function, parms.Select(ValueHelper.Wrap));
        }

        private AphidObject CallFunctionCore(AphidFunction function, IEnumerable<AphidObject> parms)
        {
            var functionScope = new AphidScope(new AphidObject(), function.ParentScope);

            var i = 0;
            foreach (var arg in parms)
            {
                if (function.Args.Length == i)
                {
                    break;
                }

                functionScope.Variables.Add(function.Args[i++], arg);
            }

            var lastScope = _currentScope;
            _currentScope = functionScope;

            Interpret(function.Body);

            var retVal = GetReturnValue();

            _currentScope = lastScope;

            return retVal;
        }

        public AphidObject GetReturnValue()
        {
            AphidObject retVal = null;

            _currentScope.Variables.TryGetValue(_return, out retVal);

            return retVal;
        }

        private void SetReturnValue(AphidObject obj)
        {
            _currentScope.Variables.Add(_return, obj);
        }

        private object InterpretCallExpression(CallExpression expression)
        {
            var value = InterpretExpression(expression.FunctionExpression);

            object funcExp = ValueHelper.Unwrap(value);

            var func = funcExp as AphidInteropFunction;

            if (func == null)
            {
                var func2 = funcExp as AphidFunction;

                if (func2 == null)
                {
                    throw new AphidRuntimeException("Could not find function {0}", expression.FunctionExpression);
                }

                return CallFunctionCore(func2, expression.Args.Select(x => ValueHelper.Wrap(InterpretExpression(x))));
            }
            else
            {
                Func<Expression, object> selector;
                
                if (func.UnwrapParameters)
                {
                    selector = x => ValueHelper.Unwrap(InterpretExpression(x));
                }
                else
                {
                    selector = x => InterpretExpression(x);
                }

                var args = expression.Args.Select(selector).ToArray();

                var retVal = ValueHelper.Wrap(func.Invoke(this, args));

                if (retVal.Value is List<AphidObject> ||
                    retVal.Value is string)
                {
                    CallInitFunction(retVal);
                    //InitList(retVal);
                }

                return retVal;
            }
        }

        private AphidFunction InterpretFunctionExpression(FunctionExpression expression)
        {
            return new AphidFunction()
            {
                Args = expression.Args.Select(x => x.Identifier).ToArray(),
                Body = expression.Body,
                ParentScope = _currentScope,
            };
        }

        private AphidFunction GetInitFunction(string name)
        {
            return InterpretIdentifierExpression(new IdentifierExpression("__init" + name)).Value as AphidFunction;
        }

        private AphidObject CallInitFunction(AphidObject obj)
        {
            string name = "List";

            if (obj.Value is List<AphidObject>)
            {
                name = "List";
            }
            else if (obj.Value is string)
            {
                name = "String";
            }
            else
            {
                return obj;
            }

            CallFunctionCore(GetInitFunction(name), new[] { obj });

            return obj;
        }

        private AphidObject InterpretArrayExpression(ArrayExpression expression)
        {
            var list = new AphidObject(expression.Elements.Select(InterpretExpression).OfType<AphidObject>().ToList());
            CallInitFunction(list);
            //InitList(list);
            //CallFunction(GetInitList(), new[] { list });
            return list;
        }

        private AphidRuntimeException CreateUnaryOperatorException(UnaryOperatorExpression expression)
        {
            throw new AphidRuntimeException("Unknown operator {0} in expression {1}", expression.Operator, expression);
        }

        private object InterpretUnaryOperatorExpression(UnaryOperatorExpression expression)
        {
            if (!expression.IsPostfix)
            {
                switch (expression.Operator)
                {
                    case AphidTokenType.retKeyword:
                        SetReturnValue(ValueHelper.Wrap(InterpretExpression(expression.Operand)));
                        _isReturning = true;
                        return null;

                    case AphidTokenType.NotOperator:
                        return new AphidObject(!(bool)ValueHelper.Unwrap(InterpretExpression(expression.Operand) as AphidObject));

                    default:
                        throw CreateUnaryOperatorException(expression);
                }
            }
            else
            {
                switch (expression.Operator)
                {
                    case AphidTokenType.IncrementOperator:
                        var obj = InterpretExpression(expression.Operand) as AphidObject;
                        obj.Value = ((decimal)obj.Value) + 1;
                        return obj;

                    case AphidTokenType.ExistsOperator:
                        if (expression.Operand is IdentifierExpression)
                        {
                            return ValueHelper.Wrap(InterpretIdentifierExpression(expression.Operand as IdentifierExpression) != null);
                        }
                        else if (expression.Operand is BinaryOperatorExpression)
                        {
                            var objRef = InterpretBinaryOperatorExpression(expression.Operand as BinaryOperatorExpression, true) as AphidRef;
                            return new AphidObject(objRef.Object.ContainsKey(objRef.Name));
                        }
                        else
                        {
                            throw new AphidRuntimeException("Unknown ? operand");
                        }
                    //var obj = InterpretExpression(

                    default:
                        throw CreateUnaryOperatorException(expression);
                }
            }
        }

        private AphidObject InterpretBooleanExpression(BooleanExpression expression)
        {
            return new AphidObject(expression.Value);
        }

        private void InterpretChild(List<Expression> block)
        {
            EnterChildScope();
            Interpret(block, false);
            LeaveChildScope(true);
        }

        private AphidObject InterpretIfExpression(IfExpression expression)
        {
            if ((bool)ValueHelper.Unwrap(InterpretExpression(expression.Condition)))
            {
                InterpretChild(expression.Body);
            }
            else if (expression.ElseBody != null)
            {
                InterpretChild(expression.ElseBody);
            }
            return null;
        }

        public AphidObject InterpretNumberExpression(NumberExpression expression)
        {
            return new AphidObject(expression.Value);
        }

        private AphidObject InterpretArrayAccessExpression(ArrayAccessExpression expression)
        {
            var val = ValueHelper.Unwrap(InterpretExpression(expression.ArrayExpression));
            var index = Convert.ToInt32(ValueHelper.Unwrap(InterpretExpression(expression.KeyExpression)));
            var array = val as List<AphidObject>;
            string str;

            if (array != null)
            {
                return array[index];
            }
            else if ((str = val as string) != null)
            {
                return CallInitFunction(new AphidObject(str[index].ToString()));
            }
            else
            {
                throw new AphidRuntimeException("Array access not supported by {0}.", val);
            }
        }

        public void EnterChildScope()
        {
            _currentScope = new AphidScope(new AphidObject(), _currentScope);
        }

        public bool LeaveChildScope(bool bubbleReturnValue = false)
        {
            if (bubbleReturnValue)
            {
                var ret = GetReturnValue();
                _currentScope = _currentScope.Parent;

                if (ret != null)
                {
                    SetReturnValue(ret);

                    return true;
                }
            }
            else
            {
                _currentScope = _currentScope.Parent;
            }

            return false;
        }

        private AphidObject InterpretForExpression(ForExpression expression)
        {
            EnterChildScope();
            var init = InterpretExpression(expression.Initialization);
            
            while ((InterpretExpression(expression.Condition) as AphidObject).GetBool())
            {
                EnterChildScope();
                Interpret(expression.Body, false);
                InterpretExpression(expression.Afterthought);
                if (LeaveChildScope(true) || _isBreaking)
                {
                    _isBreaking = false;
                    break;
                }
            }

            LeaveChildScope(true);
            return null;
        }

        private AphidObject InterpretForEachExpression(ForEachExpression expression)
        {
            var collection = InterpretExpression(expression.Collection) as AphidObject;
            var elements = collection.Value as List<AphidObject>;
            var elementId = (expression.Element as IdentifierExpression).Identifier;

            foreach (var element in elements)
            {
                EnterChildScope();
                _currentScope.Variables.Add(elementId, element);
                Interpret(expression.Body, false);

                if (LeaveChildScope(true) || _isBreaking)
                {
                    _isBreaking = false;
                    break;
                }                
            }

            return null;
        }

        private AphidObject InterpretLoadScriptExpression(LoadScriptExpression expression)
        {
            var file = ValueHelper.Unwrap(InterpretExpression(expression.FileExpression)) as string;

            if (file == null)
            {
                throw new AphidRuntimeException("Cannot load script {0}", expression.FileExpression);
            }

            _loader.LoadScript(file);

            return null;
        }

        private AphidObject InterpretLoadLibraryExpression(LoadLibraryExpression expression)
        {
            var library = ValueHelper.Unwrap(InterpretExpression(expression.LibraryExpression)) as string;

            if (library == null)
            {
                throw new AphidRuntimeException("Cannot load script {0}", expression.LibraryExpression);
            }

            _loader.LoadLibrary(library, _currentScope.Variables);

            return null;
        }

        private AphidObject InterpretBreakExpression()
        {
            _isBreaking = true;
            return null;
        }

        public AphidObject InterpretPartialFunctionExpression(PartialFunctionExpression expression)
        {
            var obj = (AphidObject)InterpretExpression(expression.Call.FunctionExpression);
            var func = (AphidFunction)obj.Value;
            var partialArgCount = func.Args.Length - expression.Call.Args.Count();
            var partialArgs = func.Args.Skip(partialArgCount).ToArray();
            var partialFunc = new AphidFunction()
            {
                Args = partialArgs,
                Body = new List<Expression> 
                {
                    new UnaryOperatorExpression(AphidTokenType.retKeyword,
                        new CallExpression(
                            expression.Call.FunctionExpression, 
                            expression.Call.Args.Concat(partialArgs.Select(x => new IdentifierExpression(x))).ToArray())),
                },
                ParentScope = _currentScope,
            };

            return new AphidObject(partialFunc);
        }

        public AphidObject InterpretThisExpression()
        {
            return _currentScope.Variables;
        }

        private object InterpretExpression(Expression expression)
        {
            if (expression is BinaryOperatorExpression)
            {
                return InterpretBinaryOperatorExpression(expression as BinaryOperatorExpression);
            }
            else if (expression is ObjectExpression)
            {
                return InterpretObjectExpression(expression as ObjectExpression);
            }
            else if (expression is StringExpression)
            {
                return InterpretStringExpression(expression as StringExpression);
            }
            else if (expression is NumberExpression)
            {
                return InterpretNumberExpression(expression as NumberExpression);
            }
            else if (expression is CallExpression)
            {
                var callExp = expression as CallExpression;
                return InterpretCallExpression(callExp);
            }
            else if (expression is IdentifierExpression)
            {
                return InterpretIdentifierExpression(expression as IdentifierExpression);
            }
            else if (expression is FunctionExpression)
            {
                return InterpretFunctionExpression(expression as FunctionExpression);
            }
            else if (expression is ArrayExpression)
            {
                return InterpretArrayExpression(expression as ArrayExpression);
            }
            else if (expression is UnaryOperatorExpression)
            {
                return InterpretUnaryOperatorExpression(expression as UnaryOperatorExpression);
            }
            else if (expression is BooleanExpression)
            {
                return InterpretBooleanExpression(expression as BooleanExpression);
            }
            else if (expression is IfExpression)
            {
                return InterpretIfExpression(expression as IfExpression);
            }
            else if (expression is ArrayAccessExpression)
            {
                return InterpretArrayAccessExpression(expression as ArrayAccessExpression);
            }
            else if (expression is ForEachExpression)
            {
                return InterpretForEachExpression(expression as ForEachExpression);
            }
            else if (expression is ForExpression)
            {
                return InterpretForExpression(expression as ForExpression);
            }
            else if (expression is LoadScriptExpression)
            {
                return InterpretLoadScriptExpression(expression as LoadScriptExpression);
            }
            else if (expression is LoadLibraryExpression)
            {
                return InterpretLoadLibraryExpression(expression as LoadLibraryExpression);
            }
            else if (expression is NullExpression)
            {
                return new AphidObject(null);
            }
            else if (expression is BreakExpression)
            {
                return InterpretBreakExpression();
            }
            else if (expression is PartialFunctionExpression)
            {
                return InterpretPartialFunctionExpression(expression as PartialFunctionExpression);
            }
            else if (expression is ThisExpression)
            {
                return InterpretThisExpression();
            }
            else
            {
                throw new AphidRuntimeException("Unexpected expression {0}", expression);
            }
        }

        public void Interpret(List<Expression> expressions, bool resetIsReturning = true)
        {
            foreach (var expression in expressions)
            {
                if (expression is IdentifierExpression)
                {
                    _currentScope.Variables.Add((expression as IdentifierExpression).Identifier, new AphidObject());
                }
                else
                {
                    InterpretExpression(expression);
                }

                if (_isBreaking)
                {
                    break;
                }
                else if (_isReturning)
                {
                    if (resetIsReturning)
                    {
                        _isReturning = false;
                    }

                    break;
                }
            }
        }

        public void Interpret(string code)
        {
            var lexer = new AphidLexer(code);
            var parser = new AphidParser(lexer.GetTokens());
            var ast = parser.Parse();
            Interpret(ast);
        }

        public void InterpretFile(string filename)
        {
            //_loader.LoadScript(filename);
            Interpret(File.ReadAllText(filename));
        }
    }
}
