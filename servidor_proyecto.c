#include <string.h>
#include <math.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <mysql.h>
#include <pthread.h>
//#include <my_global.h>

// Antes de empezar a programar el codigo del servidor vamos a definir las estructuras y las variables globales que vamos a usar

typedef struct { // Estructura con el nombre de usuario i el socket del usuario
	char nombre [20];
	int socket;
} Conectado;

typedef struct { // Estructura para crear una lista de usuarios
	Conectado Conectados[100];
	int num;
} ListaConectados;

typedef struct { // Estructura que contendra los usuarios que hay en una partida (definida por una ID)
	int ID;
	ListaConectados Conectados;
} Partida;

typedef struct { // Estructura para crear una lista partidas
	Partida Partidas[50];
	int num;
} ListaPartidas;

MYSQL *conn;
ListaConectados misConectados;
ListaConectados miLista;
ListaPartidas misPartidas;

pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;
int sockets[100];

int i;
int contador = 0;
int contador_conectados = 0;


// Una vez tenemos todas las variables definidas tenemos las funcioes que haremos servir en el servidor

void Inicializa_Conectados (ListaConectados *lista) { // Función que inicializa la lista de conectados.
	
	lista->num = 0;
}
int Pon_Conectado (ListaConectados *lista, char nombre[20], int socket) { // Funcion que incorpora a un nuevo conectado. Retorna 0 si OK y -1 si la lista ya estaba llena y no lo ha podido incorporar.
	
	if (lista->num == 100) {
		return -1;
	}
	else {
		strcpy(lista->Conectados[lista->num].nombre, nombre);
		lista->Conectados[lista->num].socket = socket;
		lista->num++;
		return 0;
	}
}
int Elimina_Conectado (ListaConectados *lista, char nombre[20]) { // Funcion que elimina un conectado de la lista. Retorna 0 si OK y -1 si no lo ha podido eliminar.
	
	int encontrado = 0;
	int i = 0;
	int socket = 0;
	
	while ((encontrado == 0) & (i <= lista->num)) {
		
		if (strcmp(lista->Conectados[i].nombre, nombre) == 0) {
			
			socket = lista->Conectados[i].socket;
			encontrado = 1;
		}
		else {
			
			i++;
		}
	}
	if (encontrado == 1) {
		
		int j = i + 1;
		
		while (j <= lista->num) {
			
			strcpy(lista->Conectados[i].nombre, lista->Conectados[j].nombre);
			lista->Conectados[i].socket = lista->Conectados[j].socket;
			
			if (j == lista->num) {
				
				lista->Conectados[j].socket = 0;
				strcpy(lista->Conectados[j].nombre, "");
			}
			
			i++;
			j++;
		}
		
		lista->num--;
		return 0;
	}
	else {
		return -1;
	}
}
int Busca_Conectado (ListaConectados *lista, char nombre[20]) { // Funcion que busca un conectado de la lista. Retorna la posición del jugador y -1 si no está en la lista.
	
	int i = 0;
	int encontrado = 0;
	
	while ((encontrado == 0) & (i < lista->num)) {
		
		if (strcmp(lista->Conectados[i].nombre, nombre) == 0) {
			encontrado = 1;
		}
		i++;
	}
	if (encontrado) {
		return i;
	}
	else {
		return -1;
	}
}
int Devuelve_Conectados (ListaConectados *lista, char conectados[300]) { // Funcion que escribe el numero de conectados y la lista de conectados en "conectados" (separados por un /).
	
	char con_buff1[300];
	char con_buff2[300];
	
	sprintf(con_buff1, "%s", lista->Conectados[0].nombre);
	
	int i = 1;
	{
		while (i < lista->num) {
			
			sprintf (con_buff1, "%s/%s", con_buff1, lista->Conectados[i].nombre);
			i++;
		}
		
		sprintf(con_buff2, "%d/%s", lista->num, con_buff1);
		strcpy(conectados, con_buff2);
	}
	return 0;
	
}
void Inicializa_Partidas (ListaPartidas *listaP) { // Funcion que inicializa la lista de partidas.
	
	listaP->num = 0;
	listaP->Partidas[0].ID = 0;
	
	for (int i = 0; i < listaP->num; i++) {
		
		listaP->Partidas[i].Conectados.num = 0;
	}
}
void Crea_Partida (ListaConectados *lista, ListaPartidas *listaP, char invitacion[300]) { // Función que crea una partida debido a una invitacion
	
	
}
int Elimina_Partida (ListaPartidas *listaP, int ID) { // Funcion que elimina una partida de la lista de partidas. Retorna 0 si OK y -1 si no la ha podido eliminar.
	
	int i = 0;
	int encontrado = 0;
	
	while ((encontrado == 0) & (i < listaP->num)) {
		
		if (listaP->Partidas[i]. ID == ID) {
			encontrado = 1;
		}
		else {
		i++;
		}
	}
	if (encontrado == 1) {
		
		listaP->Partidas[i].ID = 0;
		
		for (int j = 0; j <= listaP->Partidas[i].Conectados.num; j++) {
			
			int cont = Elimina_Conectado(&(listaP->Partidas[i].Conectados), listaP->Partidas[i].Conectados.Conectados[j].nombre);
		}
		
		listaP->Partidas[i].Conectados.num = 0;
		listaP->num--;
		return 0;
	}
	else {
		return -1;
	}
}

// NOTA PER A VOSALTRES NOMES, PER LES PARTIDES EN FALTEN MES, PERO CREC QUE ES MILLOR ANAR-HO POSANT MENTRE ES VAGI FENT TOT EL TEMA DE LES INVITACIONS I TAL

int Busca_Socket (ListaConectados *lista, char nombre[20]) { // Funcion que busca el socket de un conectado. Retorna el socket y -1 si no se ha encontrado el usuario.
	
	int i = 0;
	int encontrado = 0;
	
	while ((encontrado == 0) & (i < lista->num)) {
		
		if (strcmp(lista->Conectados[i].nombre, nombre) == 0) {
			
			encontrado = 1;
			return lista->Conectados[i].socket;
		}
		else {
		i++;
		}
	}
	
	return -1;
}
MYSQL *Conecta_BaseDatos () { // Funcion que conecta la base de datos.
	
	MYSQL *conn; // Antes de nada, vamos a conectarnos a la base de datos SR ("Squad Raid"). 
	conn = mysql_init(NULL); // Creamos una conexion al servidor MYSQL
	
	if (conn == NULL) {
		printf ("Error al crear la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	conn = mysql_real_connect (conn, "localhost", "root", "mysql", "T9_BBDD", 0, NULL, 0); 	// Inicializar la conexion, entrando nuestras claves de acceso y el nombre de la base de datos a la que queremos acceder
	
	if (conn == NULL) {
		printf ("Error al inicializar la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	return conn;
}
int Conecta_Socket (int puerto) { // Funcion que inicia la conexion del socket con el puerto de escucha.
	int sock_conn, sock_listen;
	struct sockaddr_in serv_adr;
	
	// INICIALIZACION
	
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0) { // Abrimos el socket
		printf("Error creando el socket");
	}
	
	memset(&serv_adr, 0, sizeof(serv_adr)); // Inicialitza a zero serv_addr
	
	serv_adr.sin_family = AF_INET;
	
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY); // Asignación de la IP al socket
	
	serv_adr.sin_port = htons(puerto); // Asignación del puerto de escucha
	
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0) {
		printf ("Error al bind");
	}
	
	if (listen(sock_listen, 3) < 0) { //La cola de peticiones pendientes no podra ser superior a 3
		printf("Error en el Listen");
	}
	
	return sock_listen;
}
int Busca_Usuario_BBDD (char nombre[20]) { // Funcion que permite buscar un usuario de la base de datos.
	
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta[200];
	
	sprintf(consulta, "SELECT JUGADOR.NOMBRE FROM JUGADOR WHERE JUGADOR.NOMBRE = '%s'", nombre);
	err = mysql_query (conn, consulta);
	
	if (err != 0) {
		printf("Error al consultar la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		return -1;
	}
	
	//NO ESTA ACABADA
}
int Borra_Usuario_BBDD (char nombre[20]) { // Funcion que permite borrar un usuario de la base de datos.
	
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta[200];
	
	sprintf(consulta, "DELETE FROM JUGADOR WHERE JUGADOR.NOMBRE = '%s'", nombre);
	err = mysql_query (conn, consulta);
	
	if (err != 0) {
		printf("Error al consultar la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		return -1;
	}
	else {
		return 0;
	}
}
int Registro (char salida[512], char nombre[20], char password[50]) { // Funcion para registrar un usuario. Retorna 0 si OK, -1 si se ha producido algun error y 1 si ya estaba.
	
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	char consulta[256];
	
	err = mysql_query(conn, "SELECT JUGADOR.NOMBRE FROM JUGADOR");
	
	if (err != 0) {
		
		printf("Error al consultar los datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		strcpy (salida, "1/ERROR");   //Rellenamos el buffer de respuesta.
		return -1;
	}
	
	resultado = mysql_store_result(conn);
	row = mysql_fetch_row(resultado);
	int num_jugadores = 0;	
	int encontrado = 0;
	
	if (row == NULL) {
		
		printf("No se han obtenido datos en la consulta.\n");
	}
	else {
		
		while (row != NULL)
		{
			
			if ((strcmp(row[0], nombre) == 0))
			{
				encontrado = 1;
			}
			
			row = mysql_fetch_row(resultado); //Ahora saltamos a la siguiente linea.
			num_jugadores++;
		}
	}

	if (encontrado == 0) {
		
		int ID = num_jugadores;
		int victorias = 0;
		
		sprintf(consulta, "INSERT INTO JUGADOR VALUES ('%d', '%s', '%s', '%d')", ID, nombre, password, victorias);
		err = mysql_query(conn, consulta);
	
		if (err != 0) {
		
			printf("Error al introducir los datos en base %u %s\n", mysql_errno(conn), mysql_error(conn));
			strcpy (salida, "1/ERROR");   //Rellenamos el buffer de respuesta.
			return -1;
		}
		else {
		
			printf("%s se ha introducido correctamente en la base de datos.\n", nombre);
			strcpy (salida, "1/OK");   //Rellenamos el buffer de respuesta.
			return 0;
		}
	}
	else {
		
		printf("%s ya existe, no se ha registrado.\n", nombre);
		strcpy (salida, "1/NO");   //Rellenamos el buffer de respuesta.
		return 1;
	}
}
int Login (char salida[512], char nombre[20], char password[50]) { // Funcion para iniciar sesion con un usuario. Retorna 0 si OK, 1 si la contraseña no es correcta y -1 si no se ha encontrado.
	
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	char consulta[256];
	
	sprintf(consulta, "SELECT JUGADOR.CONTRASEÑA FROM (JUGADOR) WHERE JUGADOR.NOMBRE = '%s'", nombre);
	err = mysql_query(conn, consulta);
	
	if (err != 0) {
		
		printf("Error al consultar los datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		sprintf(salida, "2/ERROR");
	}
	
	resultado = mysql_store_result(conn);
	row = mysql_fetch_row(resultado);
	
	if (row == NULL) {
		
		printf ("No se ha encontrado el jugador en la base de datos.\n");
		sprintf(salida, "2/NOT_FOUND");
		return -1;
	}
	else {
		
		char copia[25];
		strcpy(copia, row[0]);
		
		if (strcmp(password, copia) == 0) {
			
			printf ("Jugador encontrado en la base de datos.\n");
			sprintf(salida, "2/FOUND");
			return 0;
		}
		else {
			printf ("Contraseña incorrecta.\n");
			sprintf(salida, "2/NOT_PASS");
			return 1;
		}
	}
}
int Realizar_Consulta1 (char salida[512]) { // Funcion que consulta la media de duracion. Retorna 1 si no hay partidas, -1 si se ha producido algun error y sino la media de la duracion.
	
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	int duracion = 0;
	int cont = 0;
	
	int search_query = ("SELECT PARTIDA.DURACION FROM (PARTIDA)");
	err = mysql_query(conn, search_query);
	
	if (err != 0) {
		
		printf("Error al consultar la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		sprintf(salida, "3/ERROR");
		return -1;
	}
	
	resultado = mysql_store_result(conn);
	row = mysql_fetch_row(resultado);
	
	if (row == NULL) {
		
		sprintf(salida, "3/NO");
		printf ("No se han obtenido datos en la consulta\n");
		return 1;
	}
	else {
		
		while (row != NULL) {
			
			duracion = duracion + atoi(row[0]);
			row = mysql_fetch_row(resultado);
			cont = cont + 1;
		}
		
		return (duracion/cont);
	}
}
int Realizar_Consulta2 (char salida[512], char jugador[30]) { // Funcion que consulta la duracion de la partida mas larga del jugador. Retorna la duracion y -1 si no existe el jugador.
	
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	char consulta[200];
	
	sprintf(consulta, "SELECT MAX(PARTIDA.DURACION) FROM (JUGADOR, PARTIDA, JP) WHERE JUGADOR.NOMBRE = '%s' AND JUGADOR.ID = JP.ID_J AND JP.ID_P = PARTIDA.ID", jugador);
	err = mysql_query (conn,consulta);
	
	if (err != 0) {
		
		printf("Error al consultar la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		sprintf(salida, "4/ERROR");
	}
	
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	
	if (row[0] == NULL) {
		
		sprintf(salida, "4/NO");
		printf ("No se han obtenido datos en la consulta\n");
		return -1;
	}
	else {
		
		return (atoi(row[0]));
	}
}
int Realizar_Consulta3 (char salida[512], char rol [50], char fecha [50]) { // Funcion que consulta el jugador con mas victorias en un cierto dia con un cierto rol. Retorna 1 si no hay información, -1 si se ha producido algun error y 0 si OK.
	
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	char search_query [1000];
	strcpy(search_query, "SELECT DISTINCT JUGADOR.NOMBRE, JUGADOR.VICTORIAS FROM (JUGADOR) WHERE JUGADOR.VICTORIAS = (SELECT MAX(JUGADOR.VICTORIAS) FROM (JUGADOR, PARTIDA, JP) WHERE JUGADOR.ID = JP.ID_J AND JP.ROL = '");
	strcat(search_query, rol);
	strcat(search_query, "' AND JP.ID_P = PARTIDA.ID AND PARTIDA.FECHA = '");
	strcat(search_query, fecha);
	strcat(search_query,"')");
	
	err = mysql_query(conn, search_query);
	
	if (err != 0) {
		
		printf("Error al consultar la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		sprintf(salida, "5/ERROR");
		return -1; 
	}
	
	resultado = mysql_store_result(conn);
	row = mysql_fetch_row(resultado);
	
	if (row == NULL) {
		
		sprintf(salida, "5/NO");
		printf ("No se han obtenido datos en la consulta\n");
		return 1;
	}
	else {
		
		sprintf(salida, "5/");
		strcat(salida, row[0]);
		return 0;
	}
}
int Realizar_Consulta4 (char salida[512], char jugador[30]) { // Funcion que consulta el rol de la partida más larga de un jugador. Retorna 1 si no hay información, -1 si se ha producido algun error y 0 si OK.
	
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	char search_query [1000];
	sprintf(search_query, "SELECT JP.ROL FROM (JP) WHERE JP.ID_J = (SELECT JUGADOR.ID FROM (JUGADOR) WHERE JUGADOR.NOMBRE = '%s') AND JP.ID_P = (SELECT PARTIDA.ID FROM (PARTIDA) WHERE PARTIDA.DURACION = (SELECT MAX(PARTIDA.DURACION) FROM (PARTIDA, JUGADOR, JP) WHERE JUGADOR.NOMBRE = '%s' AND JUGADOR.ID = JP.ID_J AND JP.ID_P = PARTIDA.ID))", jugador, jugador);

	err = mysql_query(conn, search_query);
	
	if (err != 0) {
		
		printf("Error al consultar la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		sprintf(salida, "6/ERROR");
		return -1; 
	}
	
	resultado = mysql_store_result(conn);
	row = mysql_fetch_row(resultado);
	
	if (row == NULL) {
		
		strcpy(salida, "6/NO");
		printf ("El jugador no existe o no ha jugado\n");
		return 1;
	}
	else {
		
		sprintf(salida, "6/");
		strcat(salida, row[0]);
		return 0;
	}
}
void notif_BCAST(ListaConectados *lista, char notificacion[200]) {
	
	int socket;
	
	for(int j = 0; j < lista->num; j++) {
		
		socket = lista->Conectados[j].socket;
		write(socket, notificacion, strlen(notificacion));
	}
}
void Envia_Invitacion(ListaConectados *lista, char invitacion[200]) {
	
	char lista_invitados[100];
	char nombre_huesped[20];
	
	char *p = strtok(invitacion, "/");
	p = strtok(NULL, "/");
	int num_jugadores = atoi(p);
	printf("El numero de jugadores es: %d\n", num_jugadores);
	
	p = strtok(NULL, ":");
	strcpy(nombre_huesped, p);
	printf("El huesped es: %s\n", nombre_huesped);
	
	p = strtok(NULL, "/");
	
	while (p != NULL) {
		
		char nombre_invitado[20];
		char notificacion[200];
		
		strcpy(nombre_invitado, p);
		printf("Un invitado es: %s\n", nombre_invitado);
		
		int socket_invitacion = Busca_Socket(lista, nombre_invitado);
		
		sprintf(notificacion, "7/%s", nombre_huesped);
		printf("La invitacion es: %s\n", notificacion);
		
		write(socket_invitacion, notificacion, strlen(notificacion));
		
		p = strtok(NULL, "/");
	}
	
}
void Enviar_Mensaje(ListaConectados *lista, char mensaje[200]) {
	
	char nombre_destino[20];
	
	
}
void *Atender_Cliente (void *socket) {
	
	int sock_conn = * (int *) socket;
	int ret;
	char entrada[512];   //El buffer de peticion.
	char salida[512];  //El buffer de respuesta.
	char buff2[512];
	char buff[512];
	char copia[512];
	
	int numeroRespuestas;
	
	int conexion = 0;
	int err;
	
	while (conexion == 0) {
		
		ret = read(sock_conn, entrada, sizeof(entrada));
		printf ("Recibido\n");
		
		entrada[ret]='\0'; // Tenemos que añadirle la marca de fin de string para que no escriba lo que hay despues en el buffer
		salida[ret]='\0';
		
		printf ("Peticion: %s\n", entrada);
		
		strcpy(copia, entrada);
		
		char *p = strtok(entrada, "/");
		int codigo =  atoi(p);
		
		// Ya tenemos el codigo de la petición
		char consulta [20];
		char nombre[20];
		char nombre_huesped [20];
		char nombre_invitado_individual [20];
		char invitacion_individual [50];
		char nombre_respuesta [20];  //Nombre de la persona que responde a la invitacion.
		char nombre_respuesta_individual [20];
		int respuesta_peticion;  //Respuesta a la invitacion (0: NO acepta, 1: Acepta).
		int respuesta_peticion_individual;
		int num_conectados_individuales;
		char respuesta_completa [20]; //Contiene el nombre del jugador + su respuesta: Nombre/0, o Nombre/1
		char respuesta_completa_individual [20];
		char password[50];
		int res;
		
		
		if (codigo == 0) { // Petición de desconexión
			conexion = 1;
			
			p = strtok(NULL, "/");
			strcpy (nombre, p);
			printf ("Codigo: %d, Nombre: %s\n", codigo, nombre);
			
			pthread_mutex_lock(&mutex);
			res = Elimina_Conectado(&misConectados, nombre);
			
			if (res == 0) {
				
				printf ("%s se ha eliminado de forma correcta de la lista.\n", nombre);
			}
			else {
				
				printf ("El nombre no esta la lista.\n");
			}
			
			pthread_mutex_unlock(&mutex);
		}
		if (codigo == 1) { // Se desea registar en la base de datos a un usario.
			
			p = strtok(NULL, "/");
			strcpy (nombre, p);
			printf ("Codigo: %d, Nombre: %s\n", codigo, nombre);
			
			p = strtok(NULL, "/");
			strcpy(password, p);

			pthread_mutex_lock(&mutex);
			res = Pon_Conectado(&misConectados, nombre, sock_conn);
			pthread_mutex_unlock(&mutex);
			
			if (res == 0) {
				
				int registro = Registro(salida, nombre, password);
				
			}
			else {
				
				printf ("La lista esta llena, no se ha realizado el registro.\n");
				sprintf(salida, "1/LLENA");
			}
			
			
		}
		if (codigo == 2) { // Se desea hacer un login de un usario ya registrado. 
			
			p = strtok(NULL, "/");
			strcpy (nombre, p);
			printf ("Codigo: %d, Nombre: %s\n", codigo, nombre);
			
			p = strtok(NULL, "/");
			strcpy(password, p);
			
			int login = Login(salida, nombre, password);
			
			if (login == 0) {
				
				pthread_mutex_lock(&mutex);
				res = Pon_Conectado(&misConectados, nombre, sock_conn);
				pthread_mutex_unlock(&mutex);
				if (res != 0) {

					printf ("La lista esta llena, no se ha realizado el login.\n");
					sprintf(salida, "2/LLENA");
				}
			}
			
		}
		if (codigo == 3) { // Se desea llamar la Consulta1
			
			p = strtok(NULL, "/");
			strcpy (consulta, p);
			printf ("Esta es la %s\n", consulta);
			
			int res = Realizar_Consulta1(salida);
			
			sprintf(salida, "3/%d", res);
		}
		if (codigo == 4) { // Se desea llamar la Consulta2
			
			char jugador [30];
			
			p = strtok(NULL, "/");
			strcpy (consulta, p);
			printf ("Esta es la %s\n", consulta);
			
			p = strtok(NULL, "/");
			strcpy(jugador, p);  //Añadimos el rol
			printf("El jugador escogido es: %s\n", jugador);
			
			int res = Realizar_Consulta2(salida, jugador);
			
			if (res != -1) {
				sprintf(salida, "4/%d", res);
			}
		}
		if (codigo == 5) { // Se desea llamar la Consluta3
			
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
			
			
			Realizar_Consulta3 (salida, rol, fecha);
		}		
		if (codigo == 6) { // Se desea llamar la Consluta4
			
			char jugador [30];
			
			p = strtok(NULL, "/");
			strcpy (consulta, p);
			
			printf ("Esta es la %s\n", consulta);
			
			p = strtok(NULL, "/");
			strcpy(jugador, p);
			printf("El jugador escogido es: %s\n", jugador);
			
			Realizar_Consulta4 (salida, jugador);
		}
		if (codigo == 7) { // Petición de invitación
			
			Envia_Invitacion(&misConectados, copia);
		}
		if (codigo == 70) { // Petición de invitación aceptada
			
			char nombre_invita[20];
			char nombre_acepta[20];
			char respuesta[30];
			
			p = strtok(NULL, "/");
			strcpy(nombre_invita, p);
			
			p = strtok(NULL, "/");
			strcpy(nombre_acepta, p);
			
			int socket_respuesta = Busca_Socket(&misConectados, nombre_invita);
			
			sprintf(respuesta, "70/%s", nombre_acepta);
			printf("La respuesta a la invitacion es: %s\n", respuesta);
			
			write(socket_respuesta, respuesta, strlen(respuesta));
		}
		if (codigo == 71) { // Petición de invitación denegada
			
			char nombre_invita[20];
			char nombre_rechaza[20];
			char respuesta[30];
			
			p = strtok(NULL, "/");
			strcpy(nombre_invita, p);
			
			p = strtok(NULL, "/");
			strcpy(nombre_rechaza, p);
			
			int socket_respuesta = Busca_Socket(&misConectados, nombre_invita);
			
			sprintf(respuesta, "71/%s", nombre_rechaza);
			printf("La respuesta a la invitacion es: %s\n", respuesta);
			
			write(socket_respuesta, respuesta, strlen(respuesta));
		}
		if (codigo == 80) {
			
			char respuesta[200];
			strcpy (respuesta, copia);
			notif_BCAST(&misConectados, respuesta);			
			
		}		
		
		if (codigo == 112) { //En caso que no haya consultas
			
			printf( "No hay ninguna consulta\n");
			strcpy (salida, "NOTHING");
		}
		if ((codigo != 0) && (codigo != 7) && (codigo != 70) && (codigo != 71) && (codigo != 80)) {
			printf ("Respuesta: %s\n", salida);
			write(sock_conn, salida, strlen(salida));
		}
		if ((codigo == 0) || (codigo == 1) || (codigo == 2)) {
			
			char lista_conectados[200];
			int list = Devuelve_Conectados(&misConectados, lista_conectados);
			char notificacion_conectados[200];
			sprintf(notificacion_conectados, "110/%s", lista_conectados);
			notif_BCAST(&misConectados, notificacion_conectados);
		}
		if ((codigo == 3) || (codigo == 4) || (codigo == 5) || (codigo == 6)) {
			pthread_mutex_lock(&mutex);
			contador++;
			pthread_mutex_unlock(&mutex);
			//notificar a los clientes conectados
			char notificacion_servicios [20];
			sprintf(notificacion_servicios, "111/%d", contador);
			notif_BCAST(&misConectados, notificacion_servicios);
		}
		
	}
	
	// Se acabo el servicio para este cliente.
	close(sock_conn);
}


int main(int argc, char *argv[]) {
	
	Inicializa_Conectados(&misConectados);
	conn = Conecta_BaseDatos();
	int sock_listen = Conecta_Socket(9060);
	int sock_conn, ret;
	pthread_t thread[100]; // Definimos el thread con la estructura para crear threads.
	
	contador = 0;
	
	for(i = 0;;i++) {
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		
		sockets[i] = sock_conn; // sock_conn es el socket que usaremos para este cliente
		
		printf ("He recibido conexion\n");
		
		pthread_create (&thread[i], NULL, Atender_Cliente, &sockets[i]); // Crear thread y decirle lo que tiene que hacer
	}
}
