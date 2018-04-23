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