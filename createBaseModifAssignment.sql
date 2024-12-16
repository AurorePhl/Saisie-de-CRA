-- Table Employee
CREATE TABLE Employee (
    id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    name NVARCHAR(100) NOT NULL CHECK(name NOT LIKE '%[^a-zA-Z]%'),
    surname NVARCHAR(100) NOT NULL CHECK(surname NOT LIKE '%[^a-zA-Z]%'),
    username NVARCHAR(100) NOT NULL CHECK(username NOT LIKE '%[^a-zA-Z]%'),
    password NVARCHAR(100) NOT NULL CHECK(
        password LIKE '%[A-Z]%'
        AND password LIKE '%[a-z]%'
        AND password LIKE '%[0-9]%'
        AND password LIKE '%[^a-zA-Z0-9]%'
    ),
    isConnected BIT NOT NULL,
    role BIT NOT NULL
);

-- Table Period
CREATE TABLE Period (
    id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    start DATE NOT NULL,
    [end] DATE NOT NULL
);

-- Table Admin
CREATE TABLE Admin (
    id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    name NVARCHAR(100) NOT NULL CHECK(name NOT LIKE '%[^a-zA-Z]%'),
    surname NVARCHAR(100) NOT NULL CHECK(surname NOT LIKE '%[^a-zA-Z]%'),
    username NVARCHAR(100) NOT NULL CHECK(username NOT LIKE '%[^a-zA-Z]%'),
    password NVARCHAR(100) NOT NULL CHECK(
        password LIKE '%[A-Z]%'
        AND password LIKE '%[a-z]%'
        AND password LIKE '%[0-9]%'
        AND password LIKE '%[^a-zA-Z0-9]%'
    ),
    isConnected BIT NOT NULL,
    role BIT NOT NULL
);

-- Table Schedule
CREATE TABLE Schedule (
    id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    isCopied BIT NOT NULL,
    isSaved BIT NOT NULL,
    isSent BIT NOT NULL,
    employeeId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Employee(id)
);

-- Table Assignment
CREATE TABLE Assignment (
    code UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    libelle NVARCHAR(200) NOT NULL,
    description NVARCHAR(MAX) ,
    isValidated BIT NULL,
    isAssigned BIT NULL,
    scheduleId UNIQUEIDENTIFIER NULL FOREIGN KEY REFERENCES Schedule(id),
    adminId UNIQUEIDENTIFIER NULL FOREIGN KEY REFERENCES Admin(id),
    periodId UNIQUEIDENTIFIER NULL FOREIGN KEY REFERENCES Period(id)
);

-- Table TimeSlot
CREATE TABLE TimeSlot (
    id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    start DATETIME NOT NULL,
    [end] DATETIME NOT NULL,
    state NVARCHAR(50) NOT NULL,
    assignmentCode UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Assignment(code)
);

-- Relations
-- 1 Admin embauche plusieurs Employees (relation non sp�cifi�e explicitement ici, peut �tre trait�e via des logs ou une table interm�diaire si n�cessaire).
-- 1 Schedule contient plusieurs Assignments
ALTER TABLE Assignment
ADD FOREIGN KEY (scheduleId) REFERENCES Schedule(id);

-- 1 Assignment peut avoir plusieurs TimeSlots
ALTER TABLE TimeSlot
ADD FOREIGN KEY (assignmentCode) REFERENCES Assignment(code);

-- Validation des r�gles de contraintes
-- Aucun script additionnel n�cessaire pour les relations mentionn�es au-dessus.