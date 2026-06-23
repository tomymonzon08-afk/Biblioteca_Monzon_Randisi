CREATE TABLE "Estado_prestamo" (
	"id"	INTEGER NOT NULL,
	"descripcion"	TEXT NOT NULL,
	PRIMARY KEY("id")
);

CREATE TABLE "Estado_reserva" (
	"id"	INTEGER NOT NULL,
	"descripcion"	text NOT NULL,
	PRIMARY KEY("id")
);

CREATE TABLE "Generos" (
	"genero_id"	INTEGER NOT NULL,
	"nombre"	text NOT NULL,
	PRIMARY KEY("genero_id")
);

CREATE TABLE "Libro" (
	"ISBN"	text NOT NULL,
	"Titulo"	text NOT NULL,
	"Autor"	TEXT NOT NULL,
	"CantCopias"	INTEGER NOT NULL,
	PRIMARY KEY("ISBN")
);

CREATE TABLE "Libros_Generos" (
	"libro_id"	text NOT NULL,
	"genero_id"	INTEGER NOT NULL,
	PRIMARY KEY("libro_id","genero_id"),
	FOREIGN KEY("genero_id") REFERENCES "Generos"("genero_id") ON DELETE CASCADE,
	FOREIGN KEY("libro_id") REFERENCES "Libro"("ISBN") ON DELETE CASCADE
);

CREATE TABLE "Multas" (
	"Socio_id"	INTEGER NOT NULL,
	"valor"	INTEGER NOT NULL,
	PRIMARY KEY("Socio_id"),
	FOREIGN KEY("Socio_id") REFERENCES "Socio"("NroSocio") ON DELETE CASCADE
);

CREATE TABLE "Prestamo" (
	"Socio_id"	INTEGER NOT NULL,
	"Libro_id"	text NOT NULL,
	"FechaPrestamo"	text NOT NULL,
	"FechaVencimiento"	text NOT NULL,
	"FechaDevolucion"	text,
	"Estado"	INTEGER NOT NULL,
	PRIMARY KEY("Libro_id","Socio_id"),
	FOREIGN KEY("Estado") REFERENCES "Estado_prestamo"("id") ON DELETE CASCADE,
	FOREIGN KEY("Libro_id") REFERENCES "Libro"("ISBN") ON DELETE CASCADE,
	FOREIGN KEY("Socio_id") REFERENCES "Socio"("NroSocio") ON DELETE CASCADE
);

CREATE TABLE "Reserva" (
	"Socio_id"	INTEGER NOT NULL,
	"Libro_id"	text NOT NULL,
	"FechaReserva"	text NOT NULL,
	"Estado"	INTEGER NOT NULL,
	PRIMARY KEY("Libro_id","Socio_id"),
	FOREIGN KEY("Estado") REFERENCES "Estado_reserva"("id") ON DELETE CASCADE,
	FOREIGN KEY("Libro_id") REFERENCES "Libro"("ISBN") ON DELETE CASCADE,
	FOREIGN KEY("Socio_id") REFERENCES "Socio"("NroSocio") ON DELETE CASCADE
);

CREATE TABLE "Socio" (
	"NroSocio"	INTEGER NOT NULL,
	"Nombre"	text NOT NULL,
	"Apellido"	text NOT NULL,
	"Email"	text NOT NULL,
	"tipoSocio"	INTEGER NOT NULL,
	"activo"	INTEGER NOT NULL,
	PRIMARY KEY("NroSocio"),
	FOREIGN KEY("tipoSocio") REFERENCES "TipoSocio"("id") ON DELETE CASCADE
);

CREATE TABLE "TipoSocio" (
	"id"	INTEGER NOT NULL,
	"descripcion"	text NOT NULL,
	"MaxLibrosSimutaneos"	INTEGER NOT NULL,
	"DiasPrestamo"	INTEGER NOT NULL,
	"MultaPorDia"	INTEGER NOT NULL,
	PRIMARY KEY("id")
);