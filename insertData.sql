-- Insertion de donn es dans la table Employee
INSERT INTO Employee (name, surname, username, password, isConnected, role) VALUES
('John', 'Doe', 'johndoe', 'Password123!', 0, 1),
('Jane', 'Smith', 'janesmith', 'SecurePass1!', 0, 0);

-- Insertion de donn es dans la table Period
INSERT INTO Period (start, [end]) VALUES
('2024-01-01', '2024-01-31'),
('2024-02-01', '2024-02-28');

-- Insertion de donn es dans la table Admin
INSERT INTO Admin (name, surname, username, password, isConnected, role) VALUES
('Alice', 'Johnson', 'alicejohnson', 'AdminPass1!', 0, 1),
('Bob', 'Brown', 'bobbrown', 'AdminPass2!', 0, 1);

-- Insertion de donn es dans la table Schedule
INSERT INTO Schedule (isCopied, isSaved, isSent, employeeId) VALUES
(0, 1, 0, (SELECT id FROM Employee WHERE username = 'johndoe')),
(1, 1, 1, (SELECT id FROM Employee WHERE username = 'janesmith'));

-- Insertion de donn es dans la table Assignment
INSERT INTO Assignment (libelle, description, isValidated, isAssigned, scheduleId, adminId, periodId) VALUES
('System Maintenance Mission', 'Perform updates and repairs on production servers.', 1, 1, (SELECT id FROM Schedule WHERE isCopied = 0), (SELECT id FROM Admin WHERE username = 'alicejohnson'), (SELECT id FROM Period WHERE start = '2024-01-01')),
('Security Audit', 'Conduct a security review and identify vulnerabilities.', 0, 1, (SELECT id FROM Schedule WHERE isCopied = 1), (SELECT id FROM Admin WHERE username = 'bobbrown'), (SELECT id FROM Period WHERE start = '2024-02-01'));

-- Insertion de donn es dans la table TimeSlot
INSERT INTO TimeSlot (start, [end], state, assignmentCode) VALUES
('2024-01-01 09:00:00', '2024-01-01 11:00:00', 'Scheduled', (SELECT code FROM Assignment WHERE libelle = 'System Maintenance Mission')),
('2024-02-01 14:00:00', '2024-02-01 16:00:00', 'Completed', (SELECT code FROM Assignment WHERE libelle = 'Security Audit')),
('2024-01-02 10:00:00', '2024-01-02 12:00:00', 'ADDED', (SELECT code FROM Assignment WHERE libelle = 'System Maintenance Mission')),
('2024-01-03 11:00:00', '2024-01-03 13:00:00', 'SAVED', (SELECT code FROM Assignment WHERE libelle = 'System Maintenance Mission')),
('2024-01-04 12:00:00', '2024-01-04 14:00:00', 'VALIDATED', (SELECT code FROM Assignment WHERE libelle = 'System Maintenance Mission')),
('2024-01-05 13:00:00', '2024-01-05 15:00:00', 'REJECTED', (SELECT code FROM Assignment WHERE libelle = 'System Maintenance Mission')),
('2024-01-06 14:00:00', '2024-01-06 16:00:00', 'SENT', (SELECT code FROM Assignment WHERE libelle = 'System Maintenance Mission')),
('2024-02-02 09:00:00', '2024-02-02 11:00:00', 'ADDED', (SELECT code FROM Assignment WHERE libelle = 'Security Audit')),
('2024-02-03 10:00:00', '2024-02-03 12:00:00', 'SAVED', (SELECT code FROM Assignment WHERE libelle = 'Security Audit')),
('2024-02-04 11:00:00', '2024-02-04 13:00:00', 'VALIDATED', (SELECT code FROM Assignment WHERE libelle = 'Security Audit')),
('2024-02-05 12:00:00', '2024-02-05 14:00:00', 'REJECTED', (SELECT code FROM Assignment WHERE libelle = 'Security Audit')),
('2024-02-06 13:00:00', '2024-02-06 15:00:00', 'SENT', (SELECT code FROM Assignment WHERE libelle = 'Security Audit'));