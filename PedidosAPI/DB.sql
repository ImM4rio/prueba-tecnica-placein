CREATE PROCEDURE sp_InsertarPedido
(
    @IdPedido VARCHAR(32),
    @Nombre VARCHAR(50),
    @Descripcion VARCHAR(500),
    @Ubicacion VARCHAR(50),
    @Sandbox BIT,
	@Temperatura int,
	@Humedad int
)
AS
BEGIN
    INSERT INTO Pedidos (IdPedido, Nombre, Descripcion, Ubicacion, Sandbox, Temperatura, Humedad)
    VALUES (@IdPedido, @Nombre, @Descripcion, @Ubicacion, @Sandbox, @Temperatura, @Humedad)
END


CREATE PROCEDURE sp_SeleccionarPedidos
AS
BEGIN
    SELECT IdPedido, Nombre, Descripcion, Ubicacion, Sandbox, Temperatura, Humedad
    FROM Pedidos
END

USE [Pedidos]
GO

INSERT INTO [dbo].[Pedidos]
           ([IdPedido]
           ,[Nombre]
           ,[Descripcion]
           ,[Ubicacion]
           ,[Sandbox],
		   ])
     VALUES
           ('1', 'Mario', 'Esta es la descripción', 'Santiago de Compostela', 0)
GO
