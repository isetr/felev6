%baseclass-preinclude "semantics.h"
%lsp-needed

%union
{
    std::string *szoveg;
    kifejezes_leiro *kif;
    utasitas_leiro *ut;
}

%token <szoveg> NUMBER
%token OPEN
%token CLOSE

%left EQUAL
%left PLUS MINUS

%type<kif> exp
%type<ut> expressions

%%

start:
    expressions
    {
        std::cout << std::string("") +
            "extern ki_elojeles_egesz\n" +
            "extern ki_logikai\n" +
            "global main\n" +
            "section .text\n" +
            "main:\n" +
            $1->kod +
            "ret\n";
        delete $1;
    }
;

expressions:
    // ures
    {
        $$ = new utasitas_leiro( d_loc__.first_line, "" );
    }
|
    exp expressions
    {
        std::string fun;
        fun = ($1->ktip == Egesz ? "ki_elojeles_egesz" : "ki_logikai");
        $$ = new utasitas_leiro( $1->sor,
            $1->kod +
            "push eax\n" +
            "call " + fun + "\n" +
            "add esp,4\n" +
            $2->kod
            );
        delete $1;
        delete $2;
    }
;

exp:
    NUMBER
    {
        $$ = new kifejezes_leiro( d_loc__.first_line, Egesz, "mov eax," + *$1 + "\n" );
        delete $1;
    }
|
    exp PLUS exp
    {
        const std::string hibauzenet = ": Az osszeadas argumentuma csak egesz tipusu kifejezes lehet.\n";
        if( $1->ktip != Egesz )
        {
            std::cerr << $1->sor << hibauzenet;
            exit(1);
        }
        if( $3->ktip != Egesz )
        {
            std::cerr << $3->sor << hibauzenet;
            exit(1);
        }
        $$ = new kifejezes_leiro( $1->sor, Egesz,
            $3->kod +
            "push eax\n" +
            $1->kod +
            "pop edx\n" +
            "add eax,edx\n"
            );
        delete $1;
        delete $3;
    }
|
    exp MINUS exp
    {
        const std::string hibauzenet = ": A kivonas argumentuma csak egesz tipusu kifejezes lehet.\n";
        if( $1->ktip != Egesz )
        {
            std::cerr << $1->sor << hibauzenet;
            exit(1);
        }
        if( $3->ktip != Egesz )
        {
            std::cerr << $3->sor << hibauzenet;
            exit(1);
        }
        $$ = new kifejezes_leiro( $1->sor, Egesz,
            $3->kod +
            "push eax\n" +
            $1->kod +
            "pop edx\n" +
            "sub eax,edx\n"
            );
        delete $1;
        delete $3;
    }
|
    exp EQUAL exp
    {
        if( $1->ktip != $3->ktip )
        {
            std::cerr << $1->sor << ": Az egyenloseg operatorral csak azonos tipusu kifejezeseket lehet osszehasonlitani.\n";
            exit(1);
        }
        $$ = new kifejezes_leiro( $1->sor, Logikai,
            $3->kod +
            "push eax\n" +
            $1->kod +
            "pop edx\n" +
            "cmp eax,edx\n" +
            "sete al\n" +
            "movzx eax,al\n"
            );
        delete $1;
        delete $3;
    }
|
    OPEN exp CLOSE
    {
        $$ = $2;
    }
;
