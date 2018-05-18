#include <iostream>
#include <string>
#include <sstream>
#include <map>

enum type {natural, boolean};

struct var_data {
    var_data() {}
    var_data(int i, type t) : decl_row(i), var_type(t) {}
    int decl_row;
    type var_type;
};

struct expr_data {
    expr_data() {}
    expr_data(int i, type t, std::string str) : decl_row(i), var_type(t), code(str) {}
    int decl_row;
    type var_type;
    std::string code;
};

struct instr_data {
    instr_data() {}
    instr_data(int i, std::string t) : row(i), code(t) {}
    int row;
    std::string code;
};