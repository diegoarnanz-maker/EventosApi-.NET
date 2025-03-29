-- Inserts de TIPOS
INSERT INTO TIPOS (NOMBRE, DESCRIPCION) VALUES
('Conferencia', 'Evento acad�mico con expertos.'),
('Concierto', 'Presentaci�n musical en vivo.'),
('Taller', 'Sesi�n pr�ctica para aprender haciendo.'),
('Seminario', 'Sesi�n educativa de corta duraci�n.');
GO

-- Inserts de EVENTOS
INSERT INTO EVENTOS (NOMBRE, DESCRIPCION, FECHA_INICIO, DURACION, DIRECCION, ESTADO, DESTACADO, AFORO_MAXIMO, MINIMO_ASISTENCIA, PRECIO, ID_TIPO) VALUES
('ConfInteligencia Artificial', 'Conferencia sobre avances en IA.', '2025-04-15', 2, 'Centro Tecnol�gico', 'ACEPTADO', 'S', 100, 10, 25.00, 1),
('RockFest 2025', 'Festival de rock nacional e internacional.', '2025-07-20', 1, 'Parque del Sol', 'ACEPTADO', 'N', 500, 50, 35.00, 2),
('Taller de Cocina Vegana', 'Aprende recetas saludables.', '2025-05-10', 3, 'Escuela Culinaria', 'CANCELADO', 'S', 30, 5, 15.00, 3);
GO