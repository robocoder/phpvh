PhpVH.LexicalAnalysis.Php

HTML

<(\?(php)?)|%   OpenTag             PHP
<(\?|%)=        OpenTagWithEcho     PHP

<               %%
    state = 0;

    do
    {
        if (state == 0 && currentChar == '<')
            state = 1;
        else if (state == 1 && currentChar == '?')
        {
            charIndex -= 2;
            return PhpTokenType.Html;
        }
        else
            state = 0;
    } 
    while (NextChar());

    return PhpTokenType.Html;
%%

%%
    state = 0;

    do
    {
        if (state == 0 && currentChar == '<')
            state = 1;
        else if (state == 1 && currentChar == '?')
        {
            charIndex -= 2;
            return PhpTokenType.Html;
        }
        else
            state = 0;
    } 
    while (NextChar());

    return PhpTokenType.Html;
%%



PHP

(\?|%)>         CloseTag            HTML

&               Ampersand
,               Comma
:               ColonOperator
::              ScopeResolutionOperator
->              ObjectOperator
=>              KeyValueOperator
\\              Namespace
@               ErrorSuppressor

\(              LeftParenthesis 
\)              RightParenthesis 
\[              LeftBracket 
\]              RightBracket 
{               LeftBrace 
}               RightBrace 

-               MinusOperator
=               AssignmentOperator 
\+=             PlusEqualOperator 
-=              MinusEqualOperator 
\*=             MultiplicationEqualOperator 
/=              DivisionEqualOperator 
%=              ModulusEqualOperator 
.=              ConcatEqualOperator 
\|=             OrEqualOperator 
^=              XorEqualOperator 

\+              AdditionOperator
\*              MultiplicationOperator 
/               DivisionOperator 
%               ModulusOperator 
\+\+            IncrementOperator 
--              DecrementOperator             
\|              OrOperator 
^               XorOperator
~               ComplementOperator
<<              ShiftLeft 
>>              ShiftRight 
.               StringConcatOperator 

!               NotOperator 
&&              AndOperator 
\|\|            OrOperator 

<<<             %%
    state = 0;

    string id = null;
    int idStart = 0;
    var isString = false;
    while (NextChar())
    {
        if (state == 0 && (currentChar == ' ' || currentChar == '\t'))
            continue;
        else if (state == 0 && !isString && currentChar == '\'')
            isString = true;
        else if (state == 0 &&
            ((currentChar >= 'a' && currentChar <= 'z') ||
            (currentChar >= 'A' && currentChar <= 'Z') ||
            currentChar == '_' ||
            (currentChar >= '\u007f' && currentChar <= '\uffff')))
        {
            idStart = charIndex;
            state = 1;
        }
        else if (state == 1 &&
            ((currentChar >= 'a' && currentChar <= 'z') ||
            (currentChar >= 'A' && currentChar <= 'Z') ||
            (currentChar >= '0' && currentChar <= '9') ||
            currentChar == '_' ||
            (currentChar >= '\u007f' && currentChar <= '\uffff')))
            state = 1;
        else if (state == 1 || state == 2)
        {
            id = text.Substring(idStart, charIndex - idStart);

            if (isString)
            {
                if (currentChar == '\'')
                    NextChar();
                else
                    return PhpTokenType.Unknown;
            }

            state = 3;

            break;
        }
        else
            return PhpTokenType.Unknown;
    }


    var exitState = id.Length + 1;
    state = 0;
    do
    {
        if ((state == 0 || state == 1) && currentChar == '\n')
            state = 1;
        else if (state != 0 && id[state - 1] == currentChar)
        {
            if (++state == exitState)
                return PhpTokenType.HereDocString;
        }
        else
            state = 0;
    }
    while (NextChar());

    return PhpTokenType.Unknown;
%%

==              EqualityOperator
===             IdenticalOperator
!=              NotEqualOperator
<>              NotEqualOperator
!==             NotIdenticalOperator
<               LessThanOperator
>               GreaterThanOperator
<=              LessThanOrEqualOperator
>=              GreaterThanOrEqualOperator
\?              TernaryOperator

;               EndOfStatement                

\r              WhiteSpace
\n              WhiteSpace
\t              WhiteSpace
\v              WhiteSpace 
\s              WhiteSpace

%%
    state = 0;

    do
    {    
        if (state == 0 && 
            ((currentChar >= 'a' && currentChar <= 'z') || 
            (currentChar >= 'A' && currentChar <= 'Z') ||
            currentChar == '_' ||
            (currentChar >= '\u007f' && currentChar <= '\uffff')))
            state = 1;
        else if (state == 1 && 
            ((currentChar >= 'a' && currentChar <= 'z') || 
            (currentChar >= 'A' && currentChar <= 'Z') ||
            (currentChar >= '0' && currentChar <= '9') ||
            currentChar == '_' ||
            (currentChar >= '\u007f' && currentChar <= '\uffff')))
            state = 1;
        else if (state == 1 || state == 2)
        {
            charIndex--;

            return PhpTokenType.Identifier;
        }
        else
        {  
            break;
        }
    }
    while (NextChar());
%%

$           %%
    state = 0;

    while (NextChar())
    {    
        if (state == 0 && 
            ((currentChar >= 'a' && currentChar <= 'z') || 
            (currentChar >= 'A' && currentChar <= 'Z') ||
            currentChar == '_' ||
            (currentChar >= '\u007f' && currentChar <= '\uffff')))
            state = 1;
        else if (state == 1 && 
            ((currentChar >= 'a' && currentChar <= 'z') || 
            (currentChar >= 'A' && currentChar <= 'Z') ||
            (currentChar >= '0' && currentChar <= '9') ||
            currentChar == '_' ||
            (currentChar >= '\u007f' && currentChar <= '\uffff')))
            state = 1;
        else if (state == 1 || state == 2)
        {
            charIndex--;

            return PhpTokenType.Variable;
        }
        else if (state == 0)
        {
            charIndex--;

            return PhpTokenType.VariableVariable;
        }
        else
        {  
            break;
        }
    }
%%

0           %%
    state = 0;

    do
    {    
        if ((state == 0 || state == 1) && currentChar > 47 && currentChar < 58)
            state = 1;
        else if (state == 1 && currentChar == '.')
            state = 2;
        else if (state == 2 || state == 3 && currentChar > 47 && currentChar < 58)
            state = 3;
        else if ((state == 1 || state == 3) && (currentChar == 'E' || currentChar == 'e'))
        {
            state = 4;
        }
        else if (state == 4 && (currentChar == '-' || currentChar == '+'))
        {
            state = 6;
        }
        else if (state == 4 && currentChar > 47 && currentChar < 58)
        {
            state = 5;
        }
        else if (state == 5 && currentChar > 47 && currentChar < 58)
        {
            continue;
        }
        else if (state == 6)
        {
            if (currentChar > 47 && currentChar < 58)
            {
                state = 5;
                continue;
            }
            else
            {
                return PhpTokenType.Unknown;
            }
        }
        else if (state == 1 || state == 3 || state == 5)
        {
            charIndex--;

            return PhpTokenType.Number;
        }
        else
        {  
            break;
        }
    }
    while (NextChar());

    return PhpTokenType.Number;
%%

0x            %%
    currentChar = text[++charIndex];
    state = 0;

    do
    {    
        if ((state == 0 || state == 1) && 
            ((currentChar > 47 && currentChar < 58) ||
            (64 < currentChar && currentChar < 71) ||
            (96 < currentChar && currentChar < 103)))
            state = 1;        
        else if (state == 1)
        {
            charIndex--;

            return PhpTokenType.HexNumber;
        }
        else
        {  
            charIndex--;

            return PhpTokenType.Unknown;
        }
    }
    while (NextChar());
%%

%%
    state = 0;

    do
    {    
        if ((state == 0 || state == 1) && currentChar > 47 && currentChar < 58)
            state = 1;
        else if (state == 1 && currentChar == '.')
            state = 2;
        else if (state == 2 || state == 3 && currentChar > 47 && currentChar < 58)
            state = 3;
        else if ((state == 1 || state == 3) && (currentChar == 'E' || currentChar == 'e'))
        {
            state = 4;
        }
        else if (state == 4 && (currentChar == '-' || currentChar == '+'))
        {
            state = 6;
        }
        else if (state == 4 && currentChar > 47 && currentChar < 58)
        {
            state = 5;
        }
        else if (state == 5 && currentChar > 47 && currentChar < 58)
        {
            continue;
        }
        else if (state == 6)
        {
            if (currentChar > 47 && currentChar < 58)
            {
                state = 5;
                continue;
            }
            else
            {
                return PhpTokenType.Unknown;
            }
        }
        else if (state == 1 || state == 3 || state == 5)
        {
            charIndex--;

            return PhpTokenType.Number;
        }
        else
        {  
            break;
        }
    }
    while (NextChar());
%%

"            %%
    bool escaped = false;

    while (NextChar())
    {
        if (!escaped && currentChar == '"')
            return PhpTokenType.String;

        escaped = !escaped && currentChar == '\\';
    }

    return PhpTokenType.String;
%%

'            %%
    escaped = false;

    while (NextChar())
    {
        if (!escaped && currentChar == '\'')
            return PhpTokenType.String;

        escaped = !escaped && currentChar == '\\';
    }

    return PhpTokenType.String;
%%

`            %%
    escaped = false;

    while (NextChar())
    {
        if (!escaped && currentChar == '`')
            return PhpTokenType.BacktickString;

        escaped = !escaped && currentChar == '\\';
    }

    return PhpTokenType.BacktickString;
%%

//            %%
    state = 0;
    while (NextChar())
    {
        if (currentChar == '\r' || currentChar == '\n')
        {
            PreviousChar();

            return PhpTokenType.Comment;
        }
        else if (currentChar == '?')
        {
            state = 1;
        }
        else if (state == 1 && currentChar == '>')
        {
            charIndex -= 2;

            return PhpTokenType.Comment;
        }
        else
        {
            state = 0;
        }
    }

    return PhpTokenType.Comment;
%%

#            %%
    state = 0;
    while (NextChar())
    {
        if (currentChar == '\r' || currentChar == '\n')
        {
            PreviousChar();

            return PhpTokenType.Comment;
        }
        else if (currentChar == '?')
        {
            state = 1;
        }
        else if (state == 1 && currentChar == '>')
        {
            charIndex -= 2;

            return PhpTokenType.Comment;
        }
        else
        {
            state = 0;
        }
    }

    return PhpTokenType.Comment;
%%

/\*           %%
    state = 0;

    while (NextChar())
    {
        if ((state == 0 || state == 1) && currentChar == '*')
            state = 1;
        else if (state == 1 && currentChar == '/')
            return PhpTokenType.Comment;
        else
            state = 0;
    }

    return PhpTokenType.Comment;
%%



@@
    abstract 
    and 
    array 
    as 
    break 
    case 
    catch 
    cfunction 
    class 
    clone 
    const 
    continue 
    declare 
    default 
    do 
    else 
    elseif 
    enddeclare 
    endfor 
    endforeach 
    endif 
    endswitch 
    endwhile 
    extends 
    final 
    for 
    foreach 
    function 
    global 
    goto 
    if 
    implements 
    interface 
    instanceof 
    namespace 
    new 
    old_function 
    or 
    private 
    protected 
    public 
    static 
    switch 
    throw 
    try 
    use 
    var 
    while 
    xor

%%
    NextChar();    
    state = 0;

    do
    {
        if (((currentChar >= 'a' && currentChar <= 'z') ||
            (currentChar >= 'A' && currentChar <= 'Z') ||
            (currentChar >= '0' && currentChar <= '9') ||
            currentChar == '_' ||
            (currentChar >= '\u007f' && currentChar <= '\uffff')))
        {
            state = 1;
        }
        else if (state == 1)
        {
            charIndex--;

            return PhpTokenType.Identifier;
        }
        else
        {  
            charIndex--;

            return PhpTokenType.Identifier;
        }
    }
    while (NextChar());
%%

%%
    NextChar();    
    state = 0;

    do
    {
        if (((currentChar >= 'a' && currentChar <= 'z') ||
            (currentChar >= 'A' && currentChar <= 'Z') ||
            (currentChar >= '0' && currentChar <= '9') ||
            currentChar == '_' ||
            (currentChar >= '\u007f' && currentChar <= '\uffff')))
        {
            state = 1;
        }
        else if (state == 1)
        {
            charIndex--;

            return PhpTokenType.Identifier;
        }
        else
        {  
            charIndex--;

            return PhpTokenType.{Keyword};
        }
    }
    while (NextChar());
%%
@@