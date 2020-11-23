DROP DATABASE IF EXISTS SR;
CREATE DATABASE SR;

use SR;

CREATE TABLE JUGADOR (
	ID INT PRIMARY KEY NOT NULL,	
	NOMBRE VARCHAR(60),
	CONTRASEÑA VARCHAR(60),
	VICTORIAS INT
)Engine=InnoDB;


CREATE TABLE PARTIDA (
	ID INT PRIMARY KEY NOT NULL,
	FECHA VARCHAR(60),
	HORA VARCHAR(60),
	DURACION INT,
	RESULTADO INT  /*1 -> GANAR -- 0 -> PERDER*/
)Engine=InnoDB;


CREATE TABLE JP (
	ID_J INT,
	ID_P INT,
	ROL VARCHAR(60), /* HACKER E INFILTRADO */
	FOREIGN KEY (ID_J) REFERENCES JUGADOR(ID),
	FOREIGN KEY (ID_P) REFERENCES PARTIDA(ID)
)Engine=InnoDB;

/*VALORES PARA RELLENAR LA TABLE I HACER LAS PRUBAS PARA LAS CONSULTAS*/

INSERT INTO JUGADOR VALUES(0, 'Aleix', 'Trompo', 8);
INSERT INTO JUGADOR VALUES(1, 'Marc', 'China', 3);
INSERT INTO JUGADOR VALUES(2, 'Eduard', 'Verdura', 10);
INSERT INTO JUGADOR VALUES(3, 'Arnau', 'Miau', 5);

INSERT INTO PARTIDA VALUES(0, '10/10/2020', '15:30', 1000, 1);
INSERT INTO PARTIDA VALUES(1, '14/09/2020', '17:30', 950, 1);
INSERT INTO PARTIDA VALUES(2, '10/10/2020', '10:30', 350, 0);


INSERT INTO JP VALUES(0, 0, 'INFILTRADO');
INSERT INTO JP VALUES(0, 2, 'INFILTRADO');
INSERT INTO JP VALUES(1, 0, 'INFILTRADO');
INSERT INTO JP VALUES(1, 1, 'INFILTRADO');
INSERT INTO JP VALUES(1, 2, 'HACKER');
INSERT INTO JP VALUES(2, 0, 'INFILTRADO');
INSERT INTO JP VALUES(2, 1, 'HACKER');
INSERT INTO JP VALUES(2, 2, 'INFILTRADO');
INSERT INTO JP VALUES(3, 0, 'HACKER');
INSERT INTO JP VALUES(3, 2, 'INFILTRADO');

