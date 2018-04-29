%baseclass-preinclude "semantics.h"

%lsp-needed

%token PROGRAM
%token DATA
%token TIPUS
%token EGESZ 
%token LOGIKAI
%token HA
%token KULONBEN
%token KULONBENHA
%token HA_VEGE
%token AMIG
%token CIKLUS_VEGE
%token OLVAS
%token IR
%token ERTEKADAS
%token PLUSZ
%token MINUSZ
%token SZORZAS
%token OSZTAS
%token TO
%token FROM
%token BY
%token PONT
%token KETTOSPONT
%token VESSZO
%token BALZAROJEL
%token JOBBZAROJEL
%token SZAM
%token IGAZ
%token HAMIS
%token <szoveg> AZONOSITO
%type <t> atom
%type <t> kifejezes

%left VAGY
%left ES
%left NEM
%left EGYENLO
%left KISEBB NAGYOBB

%union {
    std::string *szoveg;
    type *t;
}

%%

start:
    PROGRAM AZONOSITO PONT deklaraciok utasitasok
    {
        std::cout   << "start -> PROGRAM AZONOSITO PONT deklaraciok utasitasok" << std::endl;
    }
;

deklaraciok:
    // ures
    {
        std::cout << "deklaraciok -> epsilon" << std::endl;
    }
|
    DATA KETTOSPONT deklaracio deklaraciolista PONT
    {
        std::cout << "deklaraciok -> DATA KETTOSPONT deklaracio deklaraciolista PONT" << std::endl;
    }
;

deklaraciolista:
    // ures
    {
        std::cout << "deklaraciolista -> epsilon" << std::endl;
    }
|
    VESSZO deklaracio deklaraciolista
    {
        std::cout << "deklaraciolista -> VESSZO deklaracio deklaraciolista" << std::endl;
    }
;

deklaracio:
    AZONOSITO TIPUS EGESZ
    {
        std::cout << "deklaracio -> AZONOSITO TIPUS EGESZ" << std::endl;
        if( szimbolumtabla.count(*$1) > 0 )
        {
            std::stringstream ss;
            ss << "Ujradeklaralt valtozo: " << *$1 << ".\n"
            << "Korabbi deklaracio sora: " << szimbolumtabla[*$1].decl_row << std::endl;
            error( ss.str().c_str() );
        }
        szimbolumtabla[*$1] = var_data(d_loc__.first_line, natural);
        delete $1;
    }
|
    AZONOSITO TIPUS LOGIKAI
    {
        std::cout << "deklaracio -> AZONOSITO TIPUS LOGIKAI" << std::endl;
        if( szimbolumtabla.count(*$1) > 0 )
        {
            std::stringstream ss;
            ss << "Ujradeklaralt valtozo: " << *$1 << ".\n"
            << "Korabbi deklaracio sora: " << szimbolumtabla[*$1].decl_row << std::endl;
            error( ss.str().c_str() );
        }
        szimbolumtabla[*$1] = var_data(d_loc__.first_line, boolean);
        delete $1;
    }
;

utasitasok:
    // ures
    {
        std::cout << "utasitasok -> epsilon" << std::endl;
    }
|
    utasitas utasitasok
    {
        std::cout << "utasitasok -> utasitas utasitasok" << std::endl;
    }
;

utasitas:
    ertekadas
    {
        std::cout << "utasitas -> ertekadas" << std::endl;
    }
|
    be
    {
        std::cout << "utasitas -> be" << std::endl;
    }
|
    ki
    {
        std::cout << "utasitas -> ki" << std::endl;
    }
|
    osszeadas
    {
        std::cout << "utasitas -> osszeadas" << std::endl;
    }
|
    kivonas
    {
        std::cout << "utasitas -> kivonas" << std::endl;
    }
|
    szorzas
    {
        std::cout << "utasitas -> szorzas" << std::endl;
    }
|
    osztas
    {
        std::cout << "utasitas -> osztas" << std::endl;
    }
|
    elagazas
    {
        std::cout << "utasitas -> elagazas" << std::endl;
    }
|
    ciklus
    {
        std::cout << "utasitas -> ciklus" << std::endl;
    }
;

ertekadas:
    ERTEKADAS atom TO AZONOSITO PONT
    {
        std::cout << "ertekadas -> ERTEKADAS atom TO AZONOSITO PONT" << std::endl;
        if( szimbolumtabla.count(*$4) == 0 )
        {
            std::stringstream ss;
            ss << "Nem deklaralt valtozo: " << *$4 << std::endl;
            error( ss.str().c_str() );
        }
        if( *$2 != szimbolumtabla[*$4].var_type )
        {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << *$2 << std::endl;
            error( ss.str().c_str() );
        }
        delete $2;
        delete $4;
    }
;

be:
    OLVAS TO AZONOSITO PONT
    {
        std::cout << "be -> OLVAS TO AZONOSITO PONT" << std::endl;
        if( szimbolumtabla.count(*$3) == 0 )
        {
            std::stringstream ss;
            ss << "Nem deklaralt valtozo: " << *$3 << std::endl;
            error( ss.str().c_str() );
        }
        delete $3;
    }
;

ki:
    IR atom PONT
    {
        std::cout << "ki -> IR atom PONT" << std::endl;
    }
;

elagazas:
    ha kulonbenha kulonben
    {
        std::cout << "elagazas -> ha kulonbenha kulonben" << std::endl;
    }
;

ha:
    HA kifejezes PONT utasitas utasitasok
    {
        std::cout << "ha -> HA kifejezes PONT utasitas utasitasok" << std::endl;
    }
;

kulonbenha:
    // ures
    {
        std::cout << "kulonbenha -> epsilon" << std::endl;
    }
|
    KULONBENHA kifejezes PONT utasitas utasitasok kulonbenha
    {
        std::cout << "kulonbenha -> KULONBENHA kifejezes PONT utasitas utasitasok kulonbenha" << std::endl;
    }
;

kulonben:
    HA_VEGE PONT
    {
        std::cout << "kulonben -> HA_VEGE PONT" << std::endl;
    }
|
    KULONBEN PONT utasitas utasitasok HA_VEGE PONT
    {
        std::cout << "elagazas -> KULONBEN PONT utasitas utasitasok HA_VEGE PONT" << std::endl;
    }
;

ciklus:
    AMIG kifejezes PONT utasitas utasitasok CIKLUS_VEGE PONT
    {
        std::cout << "ciklus -> AMIG kifejezes PONT utasitas utasitasok CIKLUS_VEGE PONT" << std::endl;
    }
;

atom:
    SZAM
    {
        std::cout << "atom -> SZAM" << std::endl;
        $$ = new type(natural);
    }
|
    IGAZ
    {
        std::cout << "atom -> IGAZ" << std::endl;
        $$ = new type(boolean);
    }
|
    HAMIS
    {
        std::cout << "atom -> HAMIS" << std::endl;
        $$ = new type(boolean);
    }
|
    AZONOSITO
    {
        std::cout << "atom -> AZONOSITO" << std::endl;
        if( szimbolumtabla.count(*$1) == 0 )
        {
            std::stringstream ss;
            ss << "Nem deklaralt valtozo: " << *$1 << std::endl;
            error( ss.str().c_str() );
        }
        $$ = new type(szimbolumtabla[*$1].var_type);
    }
;

osszeadas:
    PLUSZ atom TO AZONOSITO PONT
    {
        std::cout << "osszeadas -> PLUSZ atom TO AZONOSITO PONT" << std::endl;
        if( szimbolumtabla.count(*$4) == 0 )
        {
            std::stringstream ss;
            ss << "Nem deklaralt valtozo: " << *$4 << std::endl;
            error( ss.str().c_str() );
        }
        if( *$2 != szimbolumtabla[*$4].var_type )
        {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << *$2 << std::endl;
            error( ss.str().c_str() );
        }
        delete $2;
        delete $4;
    }
;

kivonas:
    MINUSZ atom FROM AZONOSITO PONT
    {
        std::cout << "kivonas -> MINUSZ atom FROM AZONOSITO PONT" << std::endl;
        if( szimbolumtabla.count(*$4) == 0 )
        {
            std::stringstream ss;
            ss << "Nem deklaralt valtozo: " << *$4 << std::endl;
            error( ss.str().c_str() );
        }
        if( *$2 != szimbolumtabla[*$4].var_type )
        {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << *$2 << std::endl;
            error( ss.str().c_str() );
        }
        delete $2;
        delete $4;
    }
;

szorzas:
    SZORZAS AZONOSITO BY atom PONT
    {
        std::cout << "szorzas -> SZORZAS AZONOSITO BY atom PONT" << std::endl;
        if( szimbolumtabla.count(*$2) == 0 )
        {
            std::stringstream ss;
            ss << "Nem deklaralt valtozo: " << *$2 << std::endl;
            error( ss.str().c_str() );
        }
        if( szimbolumtabla[*$2].var_type != *$4 )
        {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << *$4 << std::endl;
            error( ss.str().c_str() );
        }
        delete $2;
        delete $4;
    }
;

osztas:
    OSZTAS AZONOSITO BY atom PONT
    {
        std::cout << "osztas -> OSZTAS AZONOSITO BY atom PONT" << std::endl;
        if( szimbolumtabla.count(*$2) == 0 )
        {
            std::stringstream ss;
            ss << "Nem deklaralt valtozo: " << *$2 << std::endl;
            error( ss.str().c_str() );
        }
        if( szimbolumtabla[*$2].var_type != *$4 )
        {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << *$4 << std::endl;
            error( ss.str().c_str() );
        }
        delete $2;
        delete $4;
    }
;

kifejezes:
    atom
    {
        std::cout << "kifejezes -> atom" << std::endl;
        if( *$1 != boolean ) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << *$1 << std::endl;
            error( ss.str().c_str() );
        }
        $$ = new type(boolean);
    }
|
    atom KISEBB atom
    {
        std::cout << "kifejezes -> atom KISEBB atom" << std::endl;
        if( *$1 != natural ) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << *$1 << std::endl;
            error( ss.str().c_str() );
        }
        if( *$3 != natural ) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << *$3 << std::endl;
            error( ss.str().c_str() );
        }
        $$ = new type(boolean);
        delete $1;
        delete $3;
    }
|
    atom NAGYOBB atom
    {
        std::cout << "kifejezes -> atom NAGYOBB atom" << std::endl;
        if( *$1 != natural ) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << *$1 << std::endl;
            error( ss.str().c_str() );
        }
        if( *$3 != natural ) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << *$3 << std::endl;
            error( ss.str().c_str() );
        }
        $$ = new type(boolean);
        delete $1;
        delete $3;
    }
|
    atom EGYENLO atom
    {
        std::cout << "kifejezes -> atom EGYENLO atom" << std::endl;
        if( *$1 != natural ) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << *$1 << std::endl;
            error( ss.str().c_str() );
        }
        if( *$3 != natural ) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << *$3 << std::endl;
            error( ss.str().c_str() );
        }
        $$ = new type(boolean);
        delete $1;
        delete $3;
    }
|
    kifejezes ES kifejezes
    {
        std::cout << "kifejezes -> kifejezes ES kifejezes" << std::endl;
        if(*$1 != boolean) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu kifejezes: " << *$1 << std::endl;
            error( ss.str().c_str() );
        }
        if(*$3 != boolean) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu kifejezes: " << *$3 << std::endl;
            error( ss.str().c_str() );
        }
        $$ = new type(boolean);
        delete $1;
        delete $3;
    }
|
    kifejezes VAGY kifejezes
    {
        std::cout << "kifejezes -> kifejezes VAGY kifejezes" << std::endl;
        if(*$1 != boolean) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu kifejezes: " << *$1 << std::endl;
            error( ss.str().c_str() );
        }
        if(*$3 != boolean) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu kifejezes: " << *$3 << std::endl;
            error( ss.str().c_str() );
        }
        $$ = new type(boolean);
        delete $1;
        delete $3;
    }
|
    NEM kifejezes
    {
        std::cout << "kifejezes -> NEM kifejezes" << std::endl;
        if(*$2 != boolean) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu kifejezes: " << *$2 << std::endl;
            error( ss.str().c_str() );
        }
        $$ = new type(boolean);
        delete $2;
    }
|
    BALZAROJEL kifejezes JOBBZAROJEL
    {
        std::cout << "kifejezes -> BALZAROJEL kifejezes JOBBZAROJEL" << std::endl;
        if(*$2 != boolean) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu kifejezes: " << *$2 << std::endl;
            error( ss.str().c_str() );
        }
        $$ = new type(boolean);
        delete $2;
    }
;
