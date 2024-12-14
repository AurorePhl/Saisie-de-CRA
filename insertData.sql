-- Insertion de données dans la table Employee
INSERT INTO Employee (name, surname, username, password, isConnected, role) VALUES
('John', 'Doe', 'johndoe', 'Password123!', 0, 1),
('Jane', 'Smith', 'janesmith', 'SecurePass1!', 0, 0);

-- Insertion de données dans la table Period
INSERT INTO Period (start, [end]) VALUES
('2024-01-01', '2024-01-31'),
('2024-02-01', '2024-02-28');

-- Insertion de données dans la table Admin
INSERT INTO Admin (name, surname, username, password, isConnected, role) VALUES
('Alice', 'Johnson', 'alicejohnson', 'AdminPass1!', 0, 1),
('Bob', 'Brown', 'bobbrown', 'AdminPass2!', 0, 1);

-- Insertion de données dans la table Schedule
INSERT INTO Schedule (isCopied, isSaved, isSent, employeeId) VALUES
(0, 1, 0, (SELECT id FROM Employee WHERE username = 'johndoe')),
(1, 1, 1, (SELECT id FROM Employee WHERE username = 'janesmith'));

-- Insertion de données dans la table Assignment
INSERT INTO Assignment (libelle, description, isValidated, isAssigned, scheduleId, adminId, periodId) VALUES
('Task 1', 'Description for Task 1', 1, 1, (SELECT id FROM Schedule WHERE isCopied = 0), (SELECT id FROM Admin WHERE username = 'alicejohnson'), (SELECT id FROM Period WHERE start = '2024-01-01')),
('Task 2', 'Description for Task 2', 0, 1, (SELECT id FROM Schedule WHERE isCopied = 1), (SELECT id FROM Admin WHERE username = 'bobbrown'), (SELECT id FROM Period WHERE start = '2024-02-01'));

-- Insertion de données dans la table TimeSlot
INSERT INTO TimeSlot (start, [end], state, assignmentCode) VALUES
('2024-01-01 09:00:00', '2024-01-01 11:00:00', 'Scheduled', (SELECT code FROM Assignment WHERE libelle = 'Task 1')),
('2024-02-01 14:00:00', '2024-02-01 16:00:00', 'Completed', (SELECT code FROM Assignment WHERE libelle = 'Task 2'));