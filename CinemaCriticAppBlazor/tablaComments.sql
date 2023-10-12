CREATE TABLE Comments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(50),
    Rating INT,
    Critic NVARCHAR(500)
);