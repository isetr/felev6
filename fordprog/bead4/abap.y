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
%token <szoveg> SZAM
%token IGAZ
%token HAMIS
%token <szoveg> AZONOSITO
%type <t> atom
%type <t> kifejezes
%type <instr> deklaracio
%type <instr> deklaraciok
%type <instr> deklaraciolista
%type <instr> utasitas
%type <instr> utasitasok
%type <instr> ertekadas
%type <instr> be
%type <instr> ki
%type <instr> ha
%type <instr> kulonben
%type <instr> kulonbenha
%type <instr> osszeadas
%type <instr> kivonas
%type <instr> szorzas
%type <instr> osztas
%type <instr> elagazas
%type <instr> ciklus

%left VAGY
%left ES
%left NEM
%left EGYENLO
%left KISEBB NAGYOBB

%union {
    std::string *szoveg;
    expr_data *t;
    instr_data *instr;
}

%%

start:
    PROGRAM AZONOSITO PONT deklaraciok utasitasok
    {
        std::cout << std::string("") +
        "extern ki_egesz\n" +
        "extern ki_logikai\n" +
        "extern be_egesz\n" +
        "extern be_logikai\n" +
        "global main\n" +
        "section .bss\n" +
        $4->code +
        "section .text\n" +
        "main:\n" +
        $5->code +
        "ret\n";
        delete $4;
        delete $5;    
    }
;

deklaraciok:
    // ures
    {
        $$ = new instr_data(d_loc__.first_line, "");
    }
|
    DATA KETTOSPONT deklaracio deklaraciolista PONT
    {
        $$ = new instr_data(d_loc__.first_line, 
            $3->code + 
            $4->code);
        delete $3;
        delete $4;
    }
;

deklaraciolista:
    // ures
    {
        $$ = new instr_data(d_loc__.first_line, "");
    }
|
    VESSZO deklaracio deklaraciolista
    {
        $$ = new instr_data(d_loc__.first_line, $2->code + $3->code);
        delete $2;
        delete $3;
    }
;

deklaracio:
    AZONOSITO TIPUS EGESZ
    {
        if( szimbolumtabla.count(*$1) > 0 )
        {
            std::stringstream ss;
            ss << "Ujradeklaralt valtozo: " << *$1 << ".\n"
            << "Korabbi deklaracio sora: " << szimbolumtabla[*$1].decl_row << std::endl;
            error( ss.str().c_str() );
        }
        szimbolumtabla[*$1] = var_data(d_loc__.first_line, natural);
        $$ = new instr_data(d_loc__.first_line, std::string("") + *$1 + ": resd 1\n");
        delete $1;
    }
|
    AZONOSITO TIPUS LOGIKAI
    {
        if( szimbolumtabla.count(*$1) > 0 )
        {
            std::stringstream ss;
            ss << "Ujradeklaralt valtozo: " << *$1 << ".\n"
            << "Korabbi deklaracio sora: " << szimbolumtabla[*$1].decl_row << std::endl;
            error( ss.str().c_str() );
        }
        szimbolumtabla[*$1] = var_data(d_loc__.first_line, boolean);
        $$ = new instr_data(d_loc__.first_line, std::string("") + *$1 + ": resb 1\n");
        delete $1;
    }
;

utasitasok:
    // ures
    {
        $$ = new instr_data(d_loc__.first_line, "");
    }
|
    utasitas utasitasok
    {
        $$ = new instr_data(d_loc__.first_line, $1->code + $2->code);
        delete $1;
        delete $2;
    }
;

utasitas:
    ertekadas
    {
        $$ = new instr_data(d_loc__.first_line, $1->code);
        delete $1;
    }
|
    be
    {
        $$ = new instr_data(d_loc__.first_line, $1->code);
        delete $1;
    }
|
    ki
    {
        $$ = new instr_data(d_loc__.first_line, $1->code);
        delete $1;
    }
|
    osszeadas
    {
        $$ = new instr_data(d_loc__.first_line, $1->code);
        delete $1;
    }
|
    kivonas
    {
        $$ = new instr_data(d_loc__.first_line, $1->code);
        delete $1;
    }
|
    szorzas
    {
        $$ = new instr_data(d_loc__.first_line, $1->code);
        delete $1;
    }
|
    osztas
    {
        $$ = new instr_data(d_loc__.first_line, $1->code);
        delete $1;
    }
|
    elagazas
    {
        $$ = new instr_data(d_loc__.first_line, $1->code);
        delete $1;
    }
|
    ciklus
    {
        $$ = new instr_data(d_loc__.first_line, $1->code);
        delete $1;
    }
;

ertekadas:
    ERTEKADAS atom TO AZONOSITO PONT
    {
        if( szimbolumtabla.count(*$4) == 0 )
        {
            std::stringstream ss;
            ss << "Nem deklaralt valtozo: " << *$4 << std::endl;
            error( ss.str().c_str() );
        }
        if( ($2->var_type) != szimbolumtabla[*$4].var_type )
        {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << std::endl;
            error( ss.str().c_str() );
        }
        $$ = new instr_data(d_loc__.first_line, 
            $2->code +
            "mov [" + *$4 + "],eax\n");
        delete $2;
        delete $4;
    }
;

be:
    OLVAS TO AZONOSITO PONT
    {
        if( szimbolumtabla.count(*$3) == 0 )
        {
            std::stringstream ss;
            ss << "Nem deklaralt valtozo: " << *$3 << std::endl;
            error( ss.str().c_str() );
        }
        std::string fun;
        fun = (szimbolumtabla[*$3].var_type == natural ? "be_egesz" : "be_logikai");
        
        $$ = new instr_data(d_loc__.first_line, 
            "call " + fun + "\n" +
            "mov [" + *$3 + "],eax\n");
        delete $3;
    }
;

ki:
    IR atom PONT
    {
        std::string fun;
        fun = ($2->var_type == natural ? "ki_egesz" : "ki_logikai");
        
        $$ = new instr_data(d_loc__.first_line, 
            $2->code +
            "push eax\n" +
            "call " + fun + "\n" +
            "add esp,4\n");
        delete $2;
    }
;

elagazas:
    ha kulonbenha kulonben
    {
        $$ = new instr_data(d_loc__.first_line, 
            $1->code + 
            $2->code + 
            $3->code
        );
        delete $1;
        delete $2;
        delete $3;
    }
;

ha:
    HA kifejezes PONT utasitas utasitasok
    {
        ++cimke;
        $$ = new instr_data(d_loc__.first_line, 
            $2->code +
            "cmp al,1\n" +
            "jne near hamis_ag_" + std::to_string(cimke) + "\n" +
            $4->code +
            $5->code +
            "hamis_ag_" + std::to_string(cimke) + ":\n"
        );
        delete $2;
        delete $4;
        delete $5;
    }
;

kulonbenha:
    // ures
    {
        $$ = new instr_data(d_loc__.first_line, "");
    }
|
    KULONBENHA kifejezes PONT utasitas utasitasok kulonbenha
    {
        ++cimke;
        $$ = new instr_data(d_loc__.first_line, 
            $2->code + 
            "cmp al,1\n" +
            "jne near hamis_ag_" + std::to_string(cimke) + "\n" +
            $4->code +
            $5->code +
            "jmp vege_" + std::to_string(cimke) + "\n" +
            "hamis_ag_" + std::to_string(cimke) + ":\n" +
            $6->code +
            "vege_" + std::to_string(cimke) + ":\n"
        );
        delete $2;
        delete $4;
        delete $5;
        delete $6;
    }
;

kulonben:
    HA_VEGE PONT
    {
        $$ = new instr_data(d_loc__.first_line, "");
    }
|
    KULONBEN PONT utasitas utasitasok HA_VEGE PONT
    {
        $$ = new instr_data(d_loc__.first_line, 
            $3->code + $4->code
        );
        delete $3;
    }
;

ciklus:
    AMIG kifejezes PONT utasitas utasitasok CIKLUS_VEGE PONT
    {
        ++cimke;
        $$ = new instr_data(d_loc__.first_line, 
            "eleje_" + std::to_string(cimke) + ":\n" +
            $2->code +
            "cmp al,1\n" +
            "jne near vege_" + std::to_string(cimke) + "\n" +
            $4->code +
            $5->code +
            "jmp eleje_" + std::to_string(cimke) + "\n" +
            "vege_" + std::to_string(cimke) + ":\n"
        );
        delete $2;
    }
;

atom:
    SZAM
    {
        $$ = new expr_data(d_loc__.first_line, natural, 
            "mov eax," + *$1 + "\n"
        );
        delete $1;
    }
|
    IGAZ
    {
        $$ = new expr_data(d_loc__.first_line, boolean, 
            "mov eax,1\n"
        );
    }
|
    HAMIS
    {
        $$ = new expr_data(d_loc__.first_line, boolean, 
            "mov eax,0\n"
        );
    }
|
    AZONOSITO
    {
        if( szimbolumtabla.count(*$1) == 0 )
        {
            std::stringstream ss;
            ss << "Nem deklaralt valtozo: " << *$1 << std::endl;
            error( ss.str().c_str() );
        }
        $$ = new expr_data(d_loc__.first_line, szimbolumtabla[*$1].var_type, 
            "mov eax,[" + *$1 + "]\n"
        );
        delete $1;
    }
;

osszeadas:
    PLUSZ atom TO AZONOSITO PONT
    {
        if( szimbolumtabla.count(*$4) == 0 )
        {
            std::stringstream ss;
            ss << "Nem deklaralt valtozo: " << *$4 << std::endl;
            error( ss.str().c_str() );
        }
        if( $2->var_type != szimbolumtabla[*$4].var_type )
        {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << std::endl;
            error( ss.str().c_str() );
        }
        $$ = new instr_data(d_loc__.first_line, 
            $2->code + 
            "add [" + *$4 + "],eax\n"
        );
        delete $2;
        delete $4;
    }
;

kivonas:
    MINUSZ atom FROM AZONOSITO PONT
    {
        if( szimbolumtabla.count(*$4) == 0 )
        {
            std::stringstream ss;
            ss << "Nem deklaralt valtozo: " << *$4 << std::endl;
            error( ss.str().c_str() );
        }
        if( $2->var_type != szimbolumtabla[*$4].var_type )
        {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << std::endl;
            error( ss.str().c_str() );
        }
        $$ = new instr_data(d_loc__.first_line, 
            $2->code + 
            "sub [" + *$4 + "],eax\n"
        );
        delete $2;
        delete $4;
    }
;

szorzas:
    SZORZAS AZONOSITO BY atom PONT
    {
        if( szimbolumtabla.count(*$2) == 0 )
        {
            std::stringstream ss;
            ss << "Nem deklaralt valtozo: " << *$2 << std::endl;
            error( ss.str().c_str() );
        }
        if( szimbolumtabla[*$2].var_type != $4->var_type )
        {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << std::endl;
            error( ss.str().c_str() );
        }
        $$ = new instr_data(d_loc__.first_line, 
            $4->code +
            "push eax\n" +
            "mov eax,[" + *$2 + "]\n" +
            "pop ebx\n" +
            "mul ebx\n" +
            "mov [" + *$2 + "],eax\n"
        );
        delete $2;
        delete $4;
    }
;

osztas:
    OSZTAS AZONOSITO BY atom PONT
    {
        if( szimbolumtabla.count(*$2) == 0 )
        {
            std::stringstream ss;
            ss << "Nem deklaralt valtozo: " << *$2 << std::endl;
            error( ss.str().c_str() );
        }
        if( szimbolumtabla[*$2].var_type != $4->var_type )
        {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << std::endl;
            error( ss.str().c_str() );
        }
        $$ = new instr_data(d_loc__.first_line, 
            "xor edx,edx\n" +
            $4->code +
            "push eax\n" +
            "mov eax,[" + *$2 + "]\n" +
            "pop ebx\n" +
            "div ebx\n"
            "mov [" + *$2 + "],eax\n"
        );
        delete $2;
        delete $4;
    }
;

kifejezes:
    atom
    {
        if( $1->var_type != boolean ) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << std::endl;
            error( ss.str().c_str() );
        }
        $$ = new expr_data(d_loc__.first_line, boolean, $1->code);
        delete $1;
    }
|
    atom KISEBB atom
    {
        if( $1->var_type != natural ) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << std::endl;
            error( ss.str().c_str() );
        }
        if( $3->var_type != natural ) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << std::endl;
            error( ss.str().c_str() );
        }
        ++cimke;
        $$ = new expr_data(d_loc__.first_line, boolean, 
            $3->code +
            "push eax\n" +
            $1->code +
            "pop ebx\n"
            "cmp eax,ebx\n"
            "jb kisebb_" + std::to_string(cimke) + "\n" +
            "mov al,0\n"
            "jmp vege_" + std::to_string(cimke) + "\n" +
            "kisebb_" + std::to_string(cimke) + ":\n" +
            "mov al,1\n" +
            "vege_" + std::to_string(cimke) + ":\n"
        );
        delete $1;
        delete $3;
    }
|
    atom NAGYOBB atom
    {
        if( $1->var_type != natural ) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << std::endl;
            error( ss.str().c_str() );
        }
        if( $3->var_type != natural ) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << std::endl;
            error( ss.str().c_str() );
        }
        ++cimke;
        $$ = new expr_data(d_loc__.first_line, boolean, 
            $3->code +
            "push eax\n" +
            $1->code +
            "pop ebx\n"
            "cmp eax,ebx\n"
            "ja nagyobb_" + std::to_string(cimke) + "\n" +
            "mov al,0\n"
            "jmp vege_" + std::to_string(cimke) + "\n" +
            "nagyobb_" + std::to_string(cimke) + ":\n" +
            "mov al,1\n" +
            "vege_" + std::to_string(cimke) + ":\n"
        );
        delete $1;
        delete $3;
    }
|
    atom EGYENLO atom
    {
        if( $1->var_type != natural ) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: " << std::endl;
            error( ss.str().c_str() );
        }
        if( $3->var_type != natural ) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu atom: "<< std::endl;
            error( ss.str().c_str() );
        }
        ++cimke;
        $$ = new expr_data(d_loc__.first_line, boolean, 
            $3->code +
            "push eax\n" +
            $1->code +
            "pop ebx\n"
            "cmp eax,ebx\n"
            "je egyenlo_" + std::to_string(cimke) + "\n" +
            "mov al,0\n"
            "jmp vege_" + std::to_string(cimke) + "\n" +
            "egyenlo_" + std::to_string(cimke) + ":\n" +
            "mov al,1\n" +
            "vege_" + std::to_string(cimke) + ":\n"
        );
        delete $1;
        delete $3;
    }
|
    kifejezes ES kifejezes
    {
        if($1->var_type != boolean) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu kifejezes: " << std::endl;
            error( ss.str().c_str() );
        }
        if($3->var_type != boolean) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu kifejezes: " << std::endl;
            error( ss.str().c_str() );
        }
        $$ = new expr_data(d_loc__.first_line, boolean, 
            $3->code +
            "push ax\n" +
            $1->code +
            "pop bx\n"
            "and al,bl\n"
        );
        delete $1;
        delete $3;
    }
|
    kifejezes VAGY kifejezes
    {
        if($1->var_type != boolean) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu kifejezes: " << std::endl;
            error( ss.str().c_str() );
        }
        if($3->var_type != boolean) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu kifejezes: " << std::endl;
            error( ss.str().c_str() );
        }
        $$ = new expr_data(d_loc__.first_line, boolean, 
            $3->code +
            "push ax\n" +
            $1->code +
            "pop bx\n"
            "or al,bl\n");
        delete $1;
        delete $3;
    }
|
    NEM kifejezes
    {
        if($2->var_type != boolean) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu kifejezes: " << std::endl;
            error( ss.str().c_str() );
        }
        ++cimke;
        $$ = new expr_data(d_loc__.first_line, boolean, 
            $2->code + 
            "cmp al,1\n"
            "je hamis_" + std::to_string(cimke) + "\n" +
            "mov al,0\n"
            "jmp vege_" + std::to_string(cimke) + "\n" +
            "hamis_" + std::to_string(cimke) + ":\n" +
            "mov al,1\n" +
            "vege_" + std::to_string(cimke) + ":\n"
        );
        delete $2;
    }
|
    BALZAROJEL kifejezes JOBBZAROJEL
    {
        if($2->var_type != boolean) {
            std::stringstream ss;
            ss << "Nem megfelelo tipusu kifejezes: " << std::endl;
            error( ss.str().c_str() );
        }
        $$ = new expr_data(d_loc__.first_line, boolean, $2->code);
        delete $2;
    }
;
