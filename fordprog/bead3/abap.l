%option noyywrap c++ yylineno

%{
#include "Parserbase.h"
%}

SZAMJEGY    [0-9]
WS          [ \t\n]
BETU        [a-zA-Z]

%%

PROGRAM     return Parser::PROGRAM;
DATA        return Parser::DATA;
TYPE        return Parser::TIPUS;
I           return Parser::EGESZ; 
B           return Parser::LOGIKAI;
IF          return Parser::HA;
ELSE        return Parser::KULONBEN;
ELSEIF      return Parser::KULONBENHA;
ENDIF       return Parser::HA_VEGE;
WHILE       return Parser::AMIG;
ENDWHILE    return Parser::CIKLUS_VEGE;
READ        return Parser::OLVAS;
WRITE       return Parser::IR;
"."         return Parser::PONT;
":"         return Parser::KETTOSPONT;
","         return Parser::VESSZO;
"="         return Parser::EGYENLO;
"<"         return Parser::KISEBB;
">"         return Parser::NAGYOBB;
MOVE        return Parser::ERTEKADAS;
ADD         return Parser::PLUSZ;
SUBTRACT    return Parser::MINUSZ;
MULTIPLY    return Parser::SZORZAS;
DIVIDE      return Parser::OSZTAS;
TO          return Parser::TO;
FROM        return Parser::FROM;
BY          return Parser::BY;
AND         return Parser::ES;
OR          return Parser::VAGY;
NOT         return Parser::NEM;
"("         return Parser::BALZAROJEL;
")"         return Parser::JOBBZAROJEL;
{SZAMJEGY}+ return Parser::SZAM;
TRUE        return Parser::IGAZ;
FALSE       return Parser::HAMIS;

^"*".*\n    // nem kell semmit csinalni

{BETU}({BETU}|{SZAMJEGY})*    return Parser::AZONOSITO;

{WS}+   // feher szokozok: semmi teendo

. {
    std::cerr << lineno() << ". sor: Lexikalis hiba!" << std::endl;
    exit(1);
}

%%
