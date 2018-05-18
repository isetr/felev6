#include "stdio.h"

unsigned be_egesz()
{
	unsigned ret;
	fscanf( stdin, "%u", &ret );
	return ret;
}

int be_elojeles_egesz()
{
	int ret;
	fscanf( stdin, "%d", &ret );
	return ret;
}

int be_logikai()
{
	char ret[256];
	fscanf( stdin, "%s", ret );
	return strcmp(ret,"hamis");
}

void ki_egesz( unsigned szam )
{
	fprintf( stdout, "%u\n", szam );
}

void ki_elojeles_egesz( int szam )
{
	fprintf( stdout, "%d\n", szam );
}

void ki_logikai( int szam )
{
	szam &= 1;
	if( szam )
		fprintf( stdout, "igaz\n" );
	else
		fprintf( stdout, "hamis\n" );
}
