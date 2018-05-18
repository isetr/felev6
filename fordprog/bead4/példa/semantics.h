#ifndef SEMANTICS_H
#define SEMANTICS_H

#include <string>
#include <iostream>
#include <cstdlib>

enum tipus { Egesz, Logikai };

struct kifejezes_leiro
{
    int sor;
    tipus ktip;
    std::string kod;
    kifejezes_leiro( int s, tipus t, std::string k )
        : sor(s), ktip(t), kod(k) {}
};

struct utasitas_leiro
{
    int sor;
    std::string kod;
    utasitas_leiro( int s, std::string k )
        : sor(s), kod(k) {} 
};

#endif //SEMANTICS_H

