#include <iostream>
#include <fstream>
#include <FlexLexer.h>

int main(int argc, char** argv) {
    if(argc == 2) {
        std::ifstream file(argv[1]);
        yyFlexLexer lexer(&file, &std::cout);
        lexer.yylex();
        return 0;
    }
    return 1;
}