CREATE TABLE "Libro" (
	"ISBN"	text NOT NULL,
	"Titulo"	text NOT NULL,
	"Autor"	TEXT NOT NULL,
	"CantCopias"	INTEGER NOT NULL,
	PRIMARY KEY("ISBN")
);

CREATE TABLE "Generos" (
	"genero_id"	INTEGER NOT NULL,
	"nombre"	text NOT NULL,
	PRIMARY KEY("genero_id")
);

CREATE TABLE "Libros_Generos" (
	"libro_id"	text NOT NULL,
	"genero_id"	INTEGER NOT NULL,
	PRIMARY KEY("libro_id","genero_id"),
	FOREIGN KEY("genero_id") REFERENCES "Generos"("genero_id") ON DELETE CASCADE,
	FOREIGN KEY("libro_id") REFERENCES "Libro"("ISBN") ON DELETE CASCADE
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

CREATE TABLE "Multas" (
	"Socio_id"	INTEGER NOT NULL,
	"valor"	INTEGER NOT NULL,
	PRIMARY KEY("Socio_id"),
	FOREIGN KEY("Socio_id") REFERENCES "Socio"("NroSocio") ON DELETE CASCADE
);

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

INSERT INTO TipoSocio (id, descripcion, MaxLibrosSimutaneos, DiasPrestamo, MultaPorDia)
VALUES
(1, 'Comun', 3, 7, 150),
(2, 'Estudiante', 5, 14, 75),
(3, 'Docente', 8, 30, 50);

INSERT INTO Estado_prestamo (id, descripcion)
VALUES
(1, 'Activo'),
(2, 'Devuelto'),
(3, 'Vencido');

INSERT INTO Estado_reserva (id, descripcion)
VALUES
(1, 'Pendiente'),
(2, 'Cumplida'),
(3, 'Cancelada');

INSERT INTO Generos (genero_id, nombre)
VALUES
(1, 'Ciencia Ficcion'),
(2, 'Fantasia'),
(3, 'Novela'),
(4, 'Historia'),
(5, 'Programacion');

INSERT INTO Libro (ISBN, Titulo, Autor, CantCopias)
VALUES
('978950001', 'El Aleph', 'Jorge Luis Borges', 5),
('978950002', '1984', 'George Orwell', 8),
('978950003', 'Dune', 'Frank Herbert', 6),
('978950004', 'Clean Code', 'Robert C. Martin', 4),
('978950005', 'El Señor de los Anillos', 'J. R. R. Tolkien', 7);


INSERT INTO Libros_Generos (libro_id, genero_id)
VALUES
('978950001', 3), -- El Aleph -> Novela
('978950002', 1), -- 1984 -> Ciencia Ficcion
('978950003', 1), -- Dune -> Ciencia Ficcion
('978950004', 5), -- Clean Code -> Programacion
('978950005', 2); -- El Señor de los Anillos -> Fantasia

INSERT INTO Socio
(NroSocio, Nombre, Apellido, Email, tipoSocio, activo)
VALUES
(1001, 'Juan', 'Perez', 'juan.perez@email.com', 1, 1),
(1002, 'Maria', 'Gomez', 'maria.gomez@email.com', 2, 1),
(1003, 'Carlos', 'Lopez', 'carlos.lopez@email.com', 3, 1),
(1004, 'Ana', 'Martinez', 'ana.martinez@email.com', 2, 1),
(1005, 'Pedro', 'Fernandez', 'pedro.fernandez@email.com', 1, 0);


INSERT INTO Prestamo
(Socio_id, Libro_id, FechaPrestamo, FechaVencimiento, FechaDevolucion, Estado)
VALUES
-- Activos
(1001, '978950002', '2025-11-01', '2025-11-08', NULL, 1),
(1002, '978950004', '2025-11-10', '2025-11-24', NULL, 1),

-- Devuelto
(1003, '978950001', '2025-10-01', '2025-10-31', '2025-10-28', 2),

-- Vencidos
(1004, '978950003', '2025-09-01', '2025-09-15', NULL, 3),
(1005, '978950005', '2025-08-01', '2025-08-08', NULL, 3);
