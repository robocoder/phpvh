Components.Aphid.Lexer.Aphid

Aphid

#               LoadScriptOperator
##              LoadLibraryOperator

,               Comma
:               ColonOperator
@               funKeyword
\?              ExistsOperator


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
\|              BinaryOrOperator 
&               BinaryAndOperator
^               XorOperator
~               ComplementOperator
<<              ShiftLeft 
>>              ShiftRight 
.               MemberOperator 

!               NotOperator 
&&              AndOperator 
\|\|            OrOperator 

==              EqualityOperator
!=              NotEqualOperator
<>              NotEqualOperator
<               LessThanOperator
>               GreaterThanOperator
<=              LessThanOrEqualOperator
>=              GreaterThanOrEqualOperator

$               AsyncOperator
$>              JoinOperator

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

            return AphidTokenType.Identifier;
        }
        else
        {  
            break;
        }
    }
    while (NextChar());
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
                return AphidTokenType.Unknown;
            }
        }
        else if (state == 1 || state == 3 || state == 5)
        {
            charIndex--;

            return AphidTokenType.Number;
        }
        else
        {  
            break;
        }
    }
    while (NextChar());

    return AphidTokenType.Number;
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

            return AphidTokenType.HexNumber;
        }
        else
        {  
            charIndex--;

            return AphidTokenType.Unknown;
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
                return AphidTokenType.Unknown;
            }
        }
        else if (state == 1 || state == 3 || state == 5)
        {
            charIndex--;

            return AphidTokenType.Number;
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
            return AphidTokenType.String;

        escaped = !escaped && currentChar == '\\';
    }

    return AphidTokenType.String;
%%

'            %%
    escaped = false;

    while (NextChar())
    {
        if (!escaped && currentChar == '\'')
            return AphidTokenType.String;

        escaped = !escaped && currentChar == '\\';
    }

    return AphidTokenType.String;
%%

//            %%
    state = 0;
    while (NextChar())
    {
        if (currentChar == '\r' || currentChar == '\n')
        {
            PreviousChar();

            return AphidTokenType.Comment;
        }
        else if (currentChar == '?')
        {
            state = 1;
        }
        else if (state == 1 && currentChar == '>')
        {
            charIndex -= 2;

            return AphidTokenType.Comment;
        }
        else
        {
            state = 0;
        }
    }

    return AphidTokenType.Comment;
%%

/\*           %%
    state = 0;

    while (NextChar())
    {
        if ((state == 0 || state == 1) && currentChar == '*')
            state = 1;
        else if (state == 1 && currentChar == '/')
            return AphidTokenType.Comment;
        else
            state = 0;
    }

    return AphidTokenType.Comment;
%%



@@
    true
    false
    null

    if
    else
    
    in
    
    ret

    for
    break

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

                return AphidTokenType.Identifier;
            }
            else
            {  
                charIndex--;

                return AphidTokenType.Identifier;
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

                return AphidTokenType.Identifier;
            }
            else
            {  
                charIndex--;

                return AphidTokenType.{Keyword};
            }
        }
        while (NextChar());
    %%
@@