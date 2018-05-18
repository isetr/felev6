#include <iostream>
#include <fstream>
#include <sstream>
#include "Parser.h"
#include <FlexLexer.h>
#include <cstdlib>

using namespace std;

int main( int argc, char* argv[] )
{
	if( argc != 2 )
	{
		cerr << "A forditando fajl nevet parancssori parameterben kell megadni." << endl;
		return 1;
	}
	ifstream in( argv[1] );
	if( !in )
	{
		cerr << "A " << argv[1] << "fajlt nem sikerult megnyitni." << endl;
		return 1;
	}
	
	Parser pars(in);
	pars.parse();
	return 0;
}
