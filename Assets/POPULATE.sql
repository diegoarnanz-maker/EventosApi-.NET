USE EVENTOS_BBDD;
GO

-- TIPOS
INSERT INTO TIPOS (NOMBRE, DESCRIPCION) VALUES
('Conferencia', 'Evento acad�mico con expertos.'),
('Concierto', 'Presentaci�n musical en vivo.'),
('Taller', 'Sesi�n pr�ctica para aprender haciendo.'),
('Seminario', 'Sesi�n educativa de corta duraci�n.');

-- PERFILES
INSERT INTO PERFILES (NOMBRE) VALUES
('ADMIN'),
('USER');

-- USUARIOS (contrase�a: 12345678)
INSERT INTO USUARIOS (USERNAME, PASSWORD, EMAIL, NOMBRE, APELLIDOS, DIRECCION, ENABLED, FECHA_REGISTRO) VALUES
('admin', '12345678', 'admin@eventos.com', 'Admin', 'General', 'Calle Central 1', 1, '2024-01-15'),
('juan', '12345678', 'juan@correo.com', 'Juan', 'P�rez', 'Calle Sol 5', 1, '2024-02-10'),
('ana',  '12345678', 'ana@correo.com',  'Ana',  'L�pez',  'Avenida Luna 9', 1, '2024-02-18');

-- USUARIO_PERFILES
INSERT INTO USUARIO_PERFILES (USERNAME, ID_PERFIL) VALUES
('admin', 1), -- ADMIN
('juan', 2),  -- USER
('ana', 2);   -- USER

-- EVENTOS
INSERT INTO EVENTOS (NOMBRE, DESCRIPCION, FECHA_INICIO, DURACION, DIRECCION, ESTADO, DESTACADO, AFORO_MAXIMO, MINIMO_ASISTENCIA, PRECIO, ID_TIPO) VALUES
('ConfInteligencia Artificial', 'Conferencia sobre avances en IA.', '2025-04-15', 2, 'Centro Tecnol�gico', 'ACEPTADO', 'S', 100, 10, 25.00, 1),
('RockFest 2025', 'Festival de rock nacional e internacional.', '2025-07-20', 1, 'Parque del Sol', 'ACEPTADO', 'N', 500, 50, 35.00, 2),
('Taller de Cocina Vegana', 'Aprende recetas saludables.', '2025-05-10', 3, 'Escuela Culinaria', 'CANCELADO', 'S', 30, 5, 15.00, 3);

-- RESERVAS
INSERT INTO RESERVAS (ID_EVENTO, USERNAME, PRECIO_VENTA, OBSERVACIONES, CANTIDAD) VALUES
(1, 'juan', 25.00, 'Asiento cerca del escenario', 1),
(2, 'ana', 35.00, 'Entrada general', 2),
(1, 'ana', 25.00, 'Muy interesada en el tema', 1);

-- USUARIOS (contrase�a: 12345678 encriptada con PasswordHasher)

ALTER TABLE USUARIOS
ALTER COLUMN PASSWORD NVARCHAR(150) NOT NULL;

UPDATE USUARIOS SET PASSWORD = 'AQAAAAIAAYagAAAAEC6UjBW6XrS9eTxCG+4Yw7aV2MZx8p6EJMo8FBapByucvu7ypm9OB/xoUWEgQlNx1w==' WHERE USERNAME = 'admin';
UPDATE USUARIOS SET PASSWORD = 'AQAAAAIAAYagAAAAEKPBSpdFS9mQH64YQy1o4rAalv+bULgVLFh5e5dFqsA2dE2zmnKd6U9H4yDoKf9PZw==' WHERE USERNAME = 'juan';
UPDATE USUARIOS SET PASSWORD = 'AQAAAAIAAYagAAAAELoeSPWXxtKAjh7xTgWEGU7VCu3YBt+u/NOr9eHeMTNslq7n/1bzzU/UrJG+oPQnJg==' WHERE USERNAME = 'ana';
