%option noyywrap c++ yylineno

%{
#include "Parserbase.h"
%}

DIGIT   [0-9]
WS  [ \t\n]

%%

{DIGIT}+    return Parser::NUMBER;

"+"         return Parser::PLUS;
"-"         return Parser::MINUS;
"="         return Parser::EQUAL;
"("         return Parser::OPEN;
")"         return Parser::CLOSE;

{WS}+       // feher szokozok: semmi teendo

.           {
                std::cerr << lineno() << ": Lexikalis hiba." << std::endl;
                exit(1);
            }

%%
