#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <mysql.h>
#include <pthread.h>

typedef struct {
	char nombre [20];
	int socket;
}Conectado;

typedef struct {
	Conectado conectados [100];
	int num;
}ListaConectados;

int contador;
ListaConectados miLista;
char misConectados[300];

pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;

void Registrar_Jugador (char *buff2, int err, MYSQL *conn, char *p, char nombre [20], char password [60]) {

	//Antes de nada, necesitamos saber el ID del último jugador que está en la base de datos, para
	//poder asignarle al nuevo jugador el numero siguiente. 
	char maxID_query [80];
	strcpy(maxID_query, "SELECT * FROM JUGADOR");
	err = mysql_query(conn, maxID_query);
	
	if (err != 0) {
		printf("Error al consultar los datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	//Ahora guardamos en una variable el resultado de esa consulta, que no es mas que una tabla.
	MYSQL_RES *resultado = mysql_store_result(conn);
	MYSQL_ROW row = mysql_fetch_row(resultado);
	int numero_jugadores = 0;; //Este entero anira aumentando su valor en cada salto de fila. A la primera fila se le asigna el valor 0.
	int name_found;
	
	if (row == NULL){
		printf("No se han obtenido datos en la consulta.\n");
	}
	else {
		//Ahora etamos colocados en la primera fila, a la cual asignamos un valor de 1.
		while (row != NULL)
		{
			if ((strcmp(row[1], nombre) == 0))
			{
				name_found = 1;
			}
			row = mysql_fetch_row(resultado); //Ahora saltamos a la siguiente linea.
			numero_jugadores = numero_jugadores + 1;
		}
	}
	if (name_found == 0) {
		numero_jugadores = numero_jugadores - 1; //Ya que se acaba excediendo de 1. 
		int ultima_ID = numero_jugadores; //Ya que la ID equivale a la posicion del jugador, empezando en 0.
		printf("La ID del ultimo jugador introducido es: %i\n", ultima_ID);
		//Por tanto, la ID del juagador que introduciremos ahora es:
		int ID = ultima_ID + 1;
		
		//Ahora insertamos a ese jugador en la base de datos, cocretamente en la tabla JUGADOR.
		char insert_query [80];
		char IDs [10];
		strcpy (insert_query, "INSERT INTO JUGADOR VALUES ('"); //Necesitamos la ID pero en formato string.
		sprintf(IDs, "%i", ID); 
		strcat(insert_query, IDs);
		strcat(insert_query, "','");
		strcat(insert_query, nombre);
		strcat(insert_query, "','");
		strcat(insert_query, password);
		strcat(insert_query, "',");
		strcat(insert_query, "0);");  //El número de victorias cuando el jugador se registra es 0.
		printf ("Orden de insercion = %s", insert_query);
		
		//Ahora ya podemos realizar la inserción.
		err = mysql_query(conn, insert_query);  //Enviamos la órden. 
		if (err != 0) {
			printf("Error al introducir los datos en la base %u %s\n", mysql_errno(conn), mysql_error(conn));
			exit (1);
		}
		else {
			printf ("%s se ha introducido correctamente en la base de datos.\n", nombre);
		}
		
		strcpy (buff2, "OK");   //Rellenamos el buffer de respuesta.
	}
	else {
		strcpy (buff2, "EXISTS");   //Rellenamos el buffer de respuesta.
	}
}
void Login (char *buff2, int err, MYSQL *conn, char *p, char nombre [20], char password [50]) {
	//Antes de nada, debemos mirar en la tabla de JUGADOR. para ver si hay alguno que tiene el
	//nombre y la contraseña que contiene el mensaje del cliente. 
	
	char search_query [100];
	strcpy (search_query, "SELECT JUGADOR.NOMBRE FROM (JUGADOR) WHERE JUGADOR.NOMBRE = '");
	strcat(search_query, nombre);
	strcat(search_query, "' AND JUGADOR.CONTRASE? = '");
	strcat(search_query, password);
	strcat(search_query,"';");
	
	//Y ahora enviamos la orden.
	err = mysql_query(conn, search_query);
	if (err != 0) {
		printf("Error al consultar la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit (1); 
	}
	
	MYSQL_RES *resultado = mysql_store_result(conn);
	MYSQL_ROW row = mysql_fetch_row(resultado);
	
	if (row == NULL) {
		printf ("No se ha encontrado el jugador en la base de datos.\n");
		strcpy(buff2, "NOT_FOUND");
	}
	else {
		printf ("Jugador encontrado en la base de datos.\n");
		strcpy(buff2, "FOUND");
	}
}
int  Realizar_Consulta1 (char *buff2, int err, MYSQL *conn) {
	int duracion = 0;
	int cont = 0;
	
	//Realizamos directamente la consulta ya que no requiere de ningun input
	int search_query = ("SELECT PARTIDA.DURACION FROM (PARTIDA)");
	
	//Y ahora enviamos la orden.
	err = mysql_query(conn, search_query);
	if (err != 0) {
		printf("Error al consultar la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit (1); 
	}
	
	MYSQL_RES *resultado = mysql_store_result(conn);
	MYSQL_ROW row = mysql_fetch_row(resultado);
	
	if (row == NULL) {
		strcpy(buff2, "No hay partidas con que hacer la media.");
		printf ("No se han obtenido datos en la consulta\n");
	}
	else {
		while (row != NULL) {
			duracion = duracion + atoi(row[0]);
			row = mysql_fetch_row(resultado);
			cont = cont + 1;
		}
	}
	
	//Retornamos la media de la duración
	return (duracion/cont);
}
int  Realizar_Consulta2 (char *buff2, int err, MYSQL *conn, char jugador[30]) {
	//Consulta de partida mas larga de un jugador.
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta[200];
	sprintf(consulta,"SELECT MAX(PARTIDA.DURACION) FROM (JUGADOR, PARTIDA, JP) WHERE PARTIDA.ID = JP.ID_P AND JP.ID_J = JUGADOR.ID AND JUGADOR.NOMBRE = '%s'", jugador);
	err=mysql_query (conn,consulta);
	
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		return 1;
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row[0] == NULL) {
		strcpy(buff2, "No hay resultados de ese jugador.");
		printf ("No se han obtenido datos en la consulta\n");
	}
	else {
		return (atoi(row[0]));
	}
}
void Realizar_Consulta3 (char *buff2, int err, MYSQL *conn, char rol [50], char fecha [50]) {
	char search_query [1000];
	strcpy(search_query, "SELECT DISTINCT JUGADOR.NOMBRE, JUGADOR.VICTORIAS FROM (JUGADOR) WHERE JUGADOR.VICTORIAS = (SELECT MAX(JUGADOR.VICTORIAS) FROM (JUGADOR, PARTIDA, JP) WHERE JUGADOR.ID = JP.ID_J AND JP.ROL = '");
	strcat(search_query, rol);
	strcat(search_query, "' AND JP.ID_P = PARTIDA.ID AND PARTIDA.FECHA = '");
	strcat(search_query, fecha);
	strcat(search_query,"')");
	
	//Y ahora enviamos la orden.
	err = mysql_query(conn, search_query);
	if (err != 0) {
		printf("Error al consultar la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit (1); 
	}
	
	MYSQL_RES *resultado = mysql_store_result(conn);
	MYSQL_ROW row = mysql_fetch_row(resultado);
	
	if (row == NULL) {
		strcpy(buff2, "No hay resultados de este dia.");
		printf ("No se han obtenido datos en la consulta\n");
	}
	else {
		strcpy(buff2, row[0]);
		printf("%s\n", buff2);
	}
}
void Realizar_Consulta4 (char *buff2, int err, MYSQL *conn, char jugador[30]) { 
	//Consulat el rol de la partida más larga de un jugador
	char search_query [1000];
	strcpy(search_query, "SELECT JP.ROL FROM (JP) WHERE JP.ID_J = (SELECT JUGADOR.ID FROM (JUGADOR) WHERE JUGADOR.NOMBRE = '");
	strcat(search_query, jugador);
	strcat(search_query, "') AND JP.ID_P = (SELECT PARTIDA.ID FROM (PARTIDA) WHERE PARTIDA.DURACION = (SELECT MAX(PARTIDA.DURACION) FROM (PARTIDA)))");
	
	//Y ahora enviamos la orden.
	
	err = mysql_query(conn, search_query);
	if (err != 0) {
		printf("Error al consultar la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit (1); 
	}
	
	MYSQL_RES *resultado = mysql_store_result(conn);
	MYSQL_ROW row = mysql_fetch_row(resultado);
	
	if (row == NULL) {
		strcpy(buff2, "No hay resultados de este día.");
		printf ("No se han obtenido datos en la consulta\n");
	}
	else {
		strcpy(buff2, row[0]);
		printf("%s\n", buff2);
	}
}
int  Pon_ConectadoLista (ListaConectados *lista, char nombre[20],int socket) {
	// A?? un nuevo conectado. Retorna 0 si ok y -1 si la lista ya estaba llena y no lo ha podido a??r.
	if (lista->num==100)
		return -1;
	else{
		strcpy(lista->conectados[lista->num].nombre, nombre);
		lista->conectados[lista->num].socket = socket;
		lista->num++;
		return 0;
	}
}
int  DamePosicion_ConectadoLista (ListaConectados *lista, char nombre [20]) {
	//Devuelve la posicion de la lista o -1 si no est?n la lista
	int i = 0;
	int encontrado = 0;
	while ((i<lista->num) && !encontrado) {
		if (strcmp(lista->conectados[i].nombre,nombre) == 0)
			encontrado =1;
		if(!encontrado)
			i=i+1;
	}
	if (encontrado)
		return i;
	else
		return -1;
}
int  Elimina_ConectadoLista (ListaConectados *lista, char nombre[20]) {
	//	Retorna 0 si elimina y -1 si ese usuario no est?n la lista
	int pos = DamePosicion_ConectadoLista (lista, nombre);
	if (pos == -1)
		return -1;
	else {
		int i;
		for (i=pos; i<lista->num-1; i++)
		{
			lista->conectados[i] = lista->conectados[i+1];
			//strcpy(lista->conectados[i].nombre,lista->conectados[i+1].nombre);
			//lista->conectados[i].socket = lista->conectados[i+1].socket;
		}
		lista->num--;
		return 0;
	}
}
void DameConectadosLista (ListaConectados *lista, char conectados[300]) {
	// Pone en conectados los nombres de todos los conectados separados po/. Primero pone el número de conectados.
	sprintf (conectados, "%d", lista->num);
	int i;
	for (i = 0; i < lista->num; i++)
	{
		sprintf (conectados, "%s/%s", conectados, lista->conectados[i].nombre);
	}
}
void *Atender_Cliente (void *socket) {
	
	int sock_conn;
	int *s;
	s = (int *) socket;
	sock_conn = *s;
	
	char buff[512];   //El buffer de peticion.int sock_conn = * (int *) socket;
	char buff2[512];  //El buffer de respuesta.
	int ret;
	int err;
	
	
	//Antes de nada, vamos a conectarnos a la base de datos SR ("Squad Raid").
	MYSQL *conn;
	//Creamos una conexion al servidor MYSQL 
	conn = mysql_init(NULL);
	
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//inicializar la conexion, entrando nuestras claves de acceso y
	//el nombre de la base de datos a la que queremos acceder 
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "SR", 0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	
	
	int terminar = 0;
	
	while (terminar == 0) {
		// Ahora recibimos su nombre, que dejamos en buff
		ret=read(sock_conn,buff, sizeof(buff));
		printf ("Recibido\n");
		
		// Tenemos que añadirle la marca de fin de string 
		// para que no escriba lo que hay despues en el buffer
		buff[ret]='\0';
		
		printf ("Peticion: %s\n", buff);
		
		char *p = strtok(buff, "/");
		int codigo =  atoi (p);
		// Ya tenemos el codigo de la petición
		char consulta [20];
		char nombre[20];
		char password[50];
		int res;
		
		if      (codigo == 0)   { //Petición de desconexión
			terminar = 1;
			
			res = Elimina_ConectadoLista(&miLista, nombre);
			
			if (res == 0) {
				printf ("%s se ha eliminado de forma correcta de la lista\n", nombre);
			}
			else {
				printf ("El nombre no est?n la lista");
			}
		}
		else if (codigo == 1)   { //Se desea registar en la base de datos a un usario. 
			//Activamos la funcion Registrar_Jugador.
			p = strtok(NULL, "/");
			strcpy (nombre, p);
			printf ("Codigo: 0, Nombre: %s\n", nombre);
			
			p = strtok(NULL, "/");
			strcpy(password, p);
			Registrar_Jugador(buff2, err, conn, p, nombre, password);
			
			res = Pon_ConectadoLista(&miLista, nombre, sock_conn);
			
			if (res == 0) {
				printf ("%s se ha introducido bien en la lista\n", nombre);
			}
			else {
				printf ("La lista esta llena");
			}
		}	
		else if (codigo == 2)   { //Se desea hacer un login de un usario ya registrado. 
			//Activamos la funcion 	Login.
			p = strtok(NULL, "/");
			strcpy (nombre, p);
			printf ("Codigo: 2, Nombre: %s\n", nombre);
			
			p = strtok(NULL, "/");
			strcpy(password, p);
			printf ("Contrase??%s\n", password);
			Login(buff2, err, conn, p, nombre, password);
			
			res = Pon_ConectadoLista(&miLista, nombre, sock_conn);
			
			if (res == 0) {
				printf ("%s se ha introducido bien en la lista\n", nombre);
			}
			else {
				printf ("La lista esta llena");
			}
		}
		else if (codigo == 3)   { //Se desea llamar la Consulta1
			
			p = strtok(NULL, "/");
			strcpy (consulta, p);
			printf ("Esta es la %s\n", consulta);
			
			int res = Realizar_Consulta1(buff2, err, conn);
			
			//Enviamos la consulta al cliente
			
			sprintf(buff2, "%d", res);
			printf ("El valor es %s\n", buff2);
		}
		else if (codigo == 4)   { //Se desea llamar la Consulta2
			
			char jugador [30];
			
			p = strtok(NULL, "/");
			strcpy (consulta, p);
			printf ("Esta es el jugador %s\n", consulta);
			
			p = strtok(NULL, "/");
			strcpy(jugador, p);  //Añadimos el rol
			printf("El jugador escogido es: %s\n", jugador);
			
			int res = Realizar_Consulta2(buff2, err, conn, jugador);
			
			//Enviamos la consulta al cliente
			
			sprintf(buff2, "%d", res);
			printf ("El valor es %s\n", buff2);
		}
		else if (codigo == 5)   { //Se desea llamar la Consluta3
			
			char rol [50];
			char fecha [50];
			
			p = strtok(NULL, "/");
			strcpy (consulta, p);
			printf ("Esta es la %s\n", consulta);
			
			//Debido al formato de fecha escogido tenemos que repetir varias veces los mismo.
			p = strtok(NULL, "/");
			strcpy(rol, p);  //Añadimos el rol
			printf("El rol escogido es: %s\n", rol);
			
			p = strtok(NULL, "/");
			strcpy(fecha, p);  //Vamos modificando char dia para tener la fecha completa
			strcat(fecha, "/");
			p = strtok(NULL, "/");
			strcat(fecha, p);
			strcat(fecha, "/");
			p = strtok(NULL, "/");
			strcat(fecha, p);
			printf("El dia escogido es: %s\n", fecha);
			
			
			Realizar_Consulta3 (buff2, err, conn, rol, fecha);
		}		
		else if (codigo == 6)   { //Se desea llamar la Consluta4

			
			char jugador [30];
			
			p = strtok(NULL, "/");
			strcpy (consulta, p);
			printf ("Esta es la %s\n", consulta);
			
			//Debido al formato de fecha escogido tenemos que repetir varias veces los mismo.
			p = strtok(NULL, "/");
			strcpy(jugador, p);  //Añadimos el rol
			printf("El jugador escogido es: %s\n", jugador);
			
			Realizar_Consulta4 (buff2, err, conn, jugador);
		}
		else if (codigo == 110) { //Se desea saber los conectados al servidor
			DameConectadosLista(&miLista, misConectados);
			sprintf(buff2, "%s", misConectados);
			printf("%s\n", buff2);
		}
		else if (codigo == 111) { //Se desea saber el nº de servicios
			sprintf(buff2, "%d", contador);
		}
		else if (codigo == 112) { //En caso que no haya consultas
			printf( "No hay ninguna consulta\n");
			strcpy (buff2, "NOTHING");
		}
		
		// Y lo enviamos
		if (codigo != 0) {
			printf ("Respuesta: %s\n", buff2);
			write (sock_conn, buff2, strlen(buff2));
		}
		if ((codigo == 3) || (codigo == 4) || (codigo == 5) || (codigo == 6)) {
			pthread_mutex_lock(&mutex);
			contador = contador + 1;
			pthread_mutex_unlock(&mutex);
		}
	}
	
	// Se acabo el servicio para este cliente.
	close(sock_conn);
	mysql_close (conn);
}


int main(int argc, char *argv[]) {
	
	int sock_conn, sock_listen;
	struct sockaddr_in serv_adr;
	
	// INICIALITZACIONS
	// Obrim el socket
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0) {
		printf("Error creant socket");
	} // Fem el bind al port
	
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;
	
	// asocia el socket a cualquiera de las IP de la m?quina. 
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// escucharemos en el port 9050
	serv_adr.sin_port = htons(9080);
	
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0) {
		printf ("Error al bind");
	}
	//La cola de peticiones pendientes no podr? ser superior a 4
	if (listen(sock_listen, 3) < 0) {
		printf("Error en el Listen");
	}
	
	contador = 0;
	int i;
	int sockets[100];
	pthread_t thread[100];  // Definimos el thread con la estructura para crear threads
	
	for(i = 0; i < 5; i++) {
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexion\n");
		
		
		
		sockets[i] = sock_conn;
		// sock_conn es el socket que usaremos para este cliente
		
		// Crear thread y decirle lo que tiene que hacer
		pthread_create (&thread[i], NULL, Atender_Cliente, &sockets[i]);
	}
}
