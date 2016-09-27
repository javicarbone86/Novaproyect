
Delete from EncuestaRespuesta
Delete from EncuestaPregunta
Delete from encuesta
Delete from Opcion
Delete from Pregunta
GO

select * from encuesta
select * from EncuestaPregunta
select * from EncuestaRespuesta
select * from Materia
select * from Pregunta
select * from Opcion


-----------------------------------------------------------------------------------------
--                              AGREGAR ENCUESTA MODELO 1
-----------------------------------------------------------------------------------------
Declare @EncuestaID int, @PreguntaID int   

--[Encuesta]
INSERT INTO [Encuesta]([MateriaID],[UsuarioID],[Descripcion],[FechaHora],[ClaveAcceso],[EsTemplate],[Estado],[TemplateID],[Curso])
	VALUES(1, '0', 'Perfil de estudiante de sistemas.', getdate(), 'perfil', 1, 1, NULL, NULL)

Set @EncuestaID = @@IDENTITY 


-------------------------------------------------------------------
-- Pregunta 01
-------------------------------------------------------------------
--[Pregunta]
INSERT INTO [Pregunta]([Descripcion],[Tipo],[Requerido],[ValorMinimo],[ValorMaximo],[ValorNormalInicial],[ValorNormalFinal],
						[EstadoInactivo],[Compartir],[UsuarioID])
     VALUES('¿Cuándo ingresaste en la carrera, ¿estabas trabajando en algún área de tecnología (desarrollo de software, testing, infraestructura, etc.)?', 'radio', 1,0,0,3,4,0, NULL, 0)
Set @PreguntaID = @@IDENTITY 

--[Opcion]
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'a.Si', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'b.No', 10)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'c.No estaba trabajando', 50)

--[EncuestaPregunta]
INSERT INTO [EncuestaPregunta]([EncuestaID],[PreguntaID])
	VALUES(@EncuestaID, @PreguntaID)

-------------------------------------------------------------------
-- Pregunta 02 
-------------------------------------------------------------------
INSERT INTO [Pregunta]([Descripcion],[Tipo],[Requerido],[ValorMinimo],[ValorMaximo],[ValorNormalInicial],[ValorNormalFinal],
						[EstadoInactivo],[Compartir],[UsuarioID])
     VALUES('¿En la actualidad, ¿estás trabajando en algún área de tecnología?', 'radio', 1,0,0,3,4,0, NULL, 0)

Set @PreguntaID = @@IDENTITY 

--[Opcion]
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'a.Si', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'b.No', 10)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'c.No estoy trabajando', 50)

INSERT INTO [EncuestaPregunta]([EncuestaID],[PreguntaID])
	VALUES(@EncuestaID, @PreguntaID)

-------------------------------------------------------------------
-- Pregunta 03
-------------------------------------------------------------------
INSERT INTO [Pregunta]([Descripcion],[Tipo],[Requerido],[ValorMinimo],[ValorMaximo],[ValorNormalInicial],[ValorNormalFinal],
						[EstadoInactivo],[Compartir],[UsuarioID])
     VALUES('Por favor elegí los tres temas que son de tu mayor interés. Si en la siguiente lista no encontrás alguno de los temas, podés agregarlo en la siguiente pregunta:', 'check', 1,0,0,3,4,0, NULL, 0)

Set @PreguntaID = @@IDENTITY 

--[Opcion]
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'a. Análisis funcional', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'b. Diseño de sistemas', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'c. Desarrollo de software en general (esta opción incluye las siguientes 5)', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'd. Desarrollo mobile', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'e. Desarrollo web', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'f. Desarrollo cliente-servidor', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'g. Desarrollo de juegos', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'h. Desarrollo de aplicaciones para redes sociales', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'i. Gestión de proyectos', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'j. Seguridad de sistemas', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'k. Calidad de software', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'l. Redes', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'm. Sistemas operativos', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'n. Infraestructura', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'o. Base de datos', 100)

INSERT INTO [EncuestaPregunta]([EncuestaID],[PreguntaID])
	VALUES(@EncuestaID, @PreguntaID)

-------------------------------------------------------------------
-- Pregunta 04
-------------------------------------------------------------------
INSERT INTO [Pregunta]([Descripcion],[Tipo],[Requerido],[ValorMinimo],[ValorMaximo],[ValorNormalInicial],[ValorNormalFinal],
						[EstadoInactivo],[Compartir],[UsuarioID])
     VALUES('En la pregunta anterior, ¿no encontraste todos los temas que te interesan?
Escribí un tema debajo del otro. Recordá que entre la pregunta anterior y esta no podés superar los tres temas que son tu mayor interés.', 'text', 1,0,0,3,4,0, NULL, 0)

Set @PreguntaID = @@IDENTITY 

--[Opcion]
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, '', 100)

INSERT INTO [EncuestaPregunta]([EncuestaID],[PreguntaID])
	VALUES(@EncuestaID, @PreguntaID)


-----------------------
--FIN Encuetas Modelo 1
GO
-----------------------



-----------------------------------------------------------------------------------------
--                              AGREGAR ENCUESTA MODELO 2
-----------------------------------------------------------------------------------------
Declare @EncuestaID int, @PreguntaID int   

--[Encuesta]
INSERT INTO [Encuesta]([MateriaID],[UsuarioID],[Descripcion],[FechaHora],[ClaveAcceso],[EsTemplate],[Estado],[TemplateID],[Curso])
	VALUES(1, '0', 'Evaluar la materia y el docente.', getdate(), 'perfil', 1, 1, NULL, NULL)

Set @EncuestaID = @@IDENTITY 

-------------------------------------------------------------------
-- Pregunta 01
-------------------------------------------------------------------
INSERT INTO [Pregunta]([Descripcion],[Tipo],[Requerido],[ValorMinimo],[ValorMaximo],[ValorNormalInicial],[ValorNormalFinal],
						[EstadoInactivo],[Compartir],[UsuarioID])
     VALUES('¿Considerás que el docente domina los contenidos de la materia? Seleccione una:', 'radio', 1,0,0,3,4,0, NULL, 0)

Set @PreguntaID = @@IDENTITY 

--[Opcion]
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'a. Siempre', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'b. Casi siempre', 80)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'c. Pocas veces', 60)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'd. Casi nunca', 40)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'e. Nunca', 20)

INSERT INTO [EncuestaPregunta]([EncuestaID],[PreguntaID])
	VALUES(@EncuestaID, @PreguntaID)

-------------------------------------------------------------------
-- Pregunta 02
-------------------------------------------------------------------
INSERT INTO [Pregunta]([Descripcion],[Tipo],[Requerido],[ValorMinimo],[ValorMaximo],[ValorNormalInicial],[ValorNormalFinal],
						[EstadoInactivo],[Compartir],[UsuarioID])
     VALUES('¿Es claro el docente con sus explicaciones, demostraciones y ejemplos? Seleccione una:', 'radio', 1,0,0,3,4,0, NULL, 0)

Set @PreguntaID = @@IDENTITY 

--[Opcion]
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'a. Siempre', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'b. Casi siempre', 80)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'c. Pocas veces', 60)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'd. Casi nunca', 40)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'e. Nunca', 20)

INSERT INTO [EncuestaPregunta]([EncuestaID],[PreguntaID])
	VALUES(@EncuestaID, @PreguntaID)

-------------------------------------------------------------------
-- Pregunta 03
-------------------------------------------------------------------
INSERT INTO [Pregunta]([Descripcion],[Tipo],[Requerido],[ValorMinimo],[ValorMaximo],[ValorNormalInicial],[ValorNormalFinal],
						[EstadoInactivo],[Compartir],[UsuarioID])
     VALUES('¿Existe relación entre lo enseñado y lo evaluado en los parciales y/o trabajos prácticos? Seleccione una:', 'radio', 1,0,0,3,4,0, NULL, 0)

Set @PreguntaID = @@IDENTITY 

--[Opcion]
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'a. Excelente relacion', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'b. Algunos temas no', 80)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'c. Varios temas no', 60)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'd. Muchos temas no', 40)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'e. Ninguna Relacion', 20)

INSERT INTO [EncuestaPregunta]([EncuestaID],[PreguntaID])
	VALUES(@EncuestaID, @PreguntaID)

-------------------------------------------------------------------
-- Pregunta 04
-------------------------------------------------------------------
INSERT INTO [Pregunta]([Descripcion],[Tipo],[Requerido],[ValorMinimo],[ValorMaximo],[ValorNormalInicial],[ValorNormalFinal],
						[EstadoInactivo],[Compartir],[UsuarioID])
     VALUES('¿Cómo es el trato con los alumnos? Seleccione una:', 'radio', 1,0,0,3,4,0, NULL, 0)

Set @PreguntaID = @@IDENTITY 

--[Opcion]
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'a. Excelente', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'b. Muy bueno', 80)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'c. Bueno', 60)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'd. Regular', 40)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'e. Malo', 20)

INSERT INTO [EncuestaPregunta]([EncuestaID],[PreguntaID])
	VALUES(@EncuestaID, @PreguntaID)

-------------------------------------------------------------------
-- Pregunta 05
-------------------------------------------------------------------
INSERT INTO [Pregunta]([Descripcion],[Tipo],[Requerido],[ValorMinimo],[ValorMaximo],[ValorNormalInicial],[ValorNormalFinal],
						[EstadoInactivo],[Compartir],[UsuarioID])
     VALUES('¿Cómo es la devolución de los trabajos prácticos / parciales corregidos? Seleccione una:', 'radio', 1,0,0,3,4,0, NULL, 0)

Set @PreguntaID = @@IDENTITY 

--[Opcion]
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'a. Inmediata', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'b. Muy rapida', 80)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'c. Rapida', 60)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'd. Lenta', 40)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'e. Muy lenta', 20)

INSERT INTO [EncuestaPregunta]([EncuestaID],[PreguntaID])
	VALUES(@EncuestaID, @PreguntaID)

-------------------------------------------------------------------
-- Pregunta 06
-------------------------------------------------------------------
INSERT INTO [Pregunta]([Descripcion],[Tipo],[Requerido],[ValorMinimo],[ValorMaximo],[ValorNormalInicial],[ValorNormalFinal],
						[EstadoInactivo],[Compartir],[UsuarioID])
     VALUES('El ayudante de trabajos prácticos, ¿es claro en sus explicaciones, demostraciones y ejemplos? Seleccione una:', 'radio', 1,0,0,3,4,0, NULL, 0)

Set @PreguntaID = @@IDENTITY 

--[Opcion]
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'a. Siempre', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'b. Casi siempre', 80)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'c. Pocas veces', 60)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'd. Casi nunca', 40)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'e. No hay ayudante ', 0)

INSERT INTO [EncuestaPregunta]([EncuestaID],[PreguntaID])
	VALUES(@EncuestaID, @PreguntaID)

-------------------------------------------------------------------
-- Pregunta 07
-------------------------------------------------------------------
INSERT INTO [Pregunta]([Descripcion],[Tipo],[Requerido],[ValorMinimo],[ValorMaximo],[ValorNormalInicial],[ValorNormalFinal],
						[EstadoInactivo],[Compartir],[UsuarioID])
     VALUES('¿Cómo es el trato con los alumnos del ayudante de trabajos prácticos? Seleccione una:', 'radio', 1,0,0,3,4,0, NULL, 0)

Set @PreguntaID = @@IDENTITY 

--[Opcion]
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'a. Excelente', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'b. Muy bueno', 80)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'c. Bueno', 60)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'd. Regular', 40)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'e. No hay ayudante ', 0)

INSERT INTO [EncuestaPregunta]([EncuestaID],[PreguntaID])
	VALUES(@EncuestaID, @PreguntaID)

-------------------------------------------------------------------
-- Pregunta 08
-------------------------------------------------------------------
INSERT INTO [Pregunta]([Descripcion],[Tipo],[Requerido],[ValorMinimo],[ValorMaximo],[ValorNormalInicial],[ValorNormalFinal],
						[EstadoInactivo],[Compartir],[UsuarioID])
     VALUES('Los trabajos prácticos te resultaron: Seleccione una:', 'radio', 1,0,0,3,4,0, NULL, 0)

Set @PreguntaID = @@IDENTITY 

--[Opcion]
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'a. Muy interesantes', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'b. Interesantes', 75)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'c. Poco interesantes', 50)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'd. Nada interesantes', 25)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'e. No se hicieron TPs ', 0)

INSERT INTO [EncuestaPregunta]([EncuestaID],[PreguntaID])
	VALUES(@EncuestaID, @PreguntaID)

-------------------------------------------------------------------
-- Pregunta 09
-------------------------------------------------------------------
INSERT INTO [Pregunta]([Descripcion],[Tipo],[Requerido],[ValorMinimo],[ValorMaximo],[ValorNormalInicial],[ValorNormalFinal],
						[EstadoInactivo],[Compartir],[UsuarioID])
     VALUES('La materia te resultó: Seleccione una:', 'radio', 1,0,0,3,4,0, NULL, 0)

Set @PreguntaID = @@IDENTITY 

--[Opcion]
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'a. Muy interesante', 100)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'b. Interesante', 75)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'c. Poco interesante', 50)
INSERT INTO [Opcion]([PreguntaID],[valor],[PesoEstadistico])
	VALUES(@PreguntaID, 'd. Si fuera optativa no la cursaría', 25)

INSERT INTO [EncuestaPregunta]([EncuestaID],[PreguntaID])
	VALUES(@EncuestaID, @PreguntaID)


-----------------------
--FIN Encuetas Modelo 1
GO
-----------------------
