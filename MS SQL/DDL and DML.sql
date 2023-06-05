CREATE TABLE Address (
	id INT NOT NULL PRIMARY KEY IDENTITY,
	street VARCHAR(255),
	city VARCHAR(255),
	state VARCHAR(255),
	zip_code INT,
	country VARCHAR(255)
)

CREATE TABLE Employee (
 id INT NOT NULL PRIMARY KEY IDENTITY,
 first_name VARCHAR(255),
 last_name VARCHAR(255),
 email VARCHAR(255),
 address_id INT,
 CONSTRAINT employee_address
	foreign key (address_id) references Address (id)
)

INSERT INTO Address (street, city, state, zip_code, country)
VALUES
('5th Ave', 'New York', 'NY', 10014, 'USA'),
('Monroe Ave', 'Seattle', 'Washington', 33451, 'USA')

INSERT INTO Employee (first_name, last_name, email, address_id)
VALUES
('John', 'Doe', 'jd14@gmail.com', 1),
('Laura', 'Pence', 'lp334@hotmail.com', 2)