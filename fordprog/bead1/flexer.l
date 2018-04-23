%option noyywrap c++ yylineno

%{
#include <iostream>
#include <cstdlib>
%}

DIGIT   [0-9]
IDENTIFIER_START [a-zA-Z]
IDENT {IDENTIFIER_START}({IDENTIFIER_START}|{DIGIT})*
WS  [ \t\n]

%%

I           std::cout << "keyword: " << YYText() << std::endl;
B           std::cout << "keyword: " << YYText() << std::endl;
FALSE       std::cout << "literal: " << YYText() << std::endl;
TRUE        std::cout << "literal: " << YYText() << std::endl;
TYPE        std::cout << "keyword: " << YYText() << std::endl;
PROGRAM     std::cout << "keyword: " << YYText() << std::endl;
"."         std::cout << "keyword: " << YYText() << std::endl;
DATA        std::cout << "keyword: " << YYText() << std::endl;
":"         std::cout << "keyword: " << YYText() << std::endl;
"="         std::cout << "operator: " << YYText() << std::endl;
"<"         std::cout << "operator: " << YYText() << std::endl;
">"         std::cout << "operator: " << YYText() << std::endl;
")"         std::cout << "operator: " << YYText() << std::endl;
"("         std::cout << "operator: " << YYText() << std::endl;
","         std::cout << "operator: " << YYText() << std::endl;
AND         std::cout << "operator: " << YYText() << std::endl;
OR          std::cout << "operator: " << YYText() << std::endl;
NOT         std::cout << "operator: " << YYText() << std::endl;
MOVE        std::cout << "keyword: " << YYText() << std::endl;
TO          std::cout << "keyword: " << YYText() << std::endl;
READ        std::cout << "keyword: " << YYText() << std::endl;
WRITE       std::cout << "keyword: " << YYText() << std::endl;
ADD         std::cout << "keyword: " << YYText() << std::endl;
SUBTRACT    std::cout << "keyword: " << YYText() << std::endl;
FROM        std::cout << "keyword: " << YYText() << std::endl;
MULTIPLY    std::cout << "keyword: " << YYText() << std::endl;
BY          std::cout << "keyword: " << YYText() << std::endl;
DIVIDE      std::cout << "keyword: " << YYText() << std::endl;
WHILE       std::cout << "keyword: " << YYText() << std::endl;
ENDWHILE    std::cout << "keyword: " << YYText() << std::endl;
IF          std::cout << "keyword: " << YYText() << std::endl;
ENDIF       std::cout << "keyword: " << YYText() << std::endl;
ELSE        std::cout << "keyword: " << YYText() << std::endl;
ELSEIF      std::cout << "keyword: " << YYText() << std::endl;


{DIGIT}+    std::cout << "literal: " << YYText() << std::endl;
{IDENT}     std::cout << "identifier: " << YYText() << std::endl; 

^\*.*
{WS}+

. {
    std::cerr << "Line " << lineno() << ": Parse error: Unexpected character '" << YYText() << "'" << std::endl;
    exit(1);
  }

%%