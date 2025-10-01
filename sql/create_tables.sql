-- Run these SQL commands as the schema user (APPUSER)
CREATE TABLE departments (
  id NUMBER(19) PRIMARY KEY,
  name NVARCHAR2(200) NOT NULL,
  description NVARCHAR2(1000)
);

CREATE TABLE employees (
  id NUMBER(19) PRIMARY KEY,
  full_name NVARCHAR2(500) NOT NULL,
  email NVARCHAR2(200),
  department_id NUMBER(19),
  hire_date TIMESTAMP,
  CONSTRAINT fk_emp_dept FOREIGN KEY (department_id) REFERENCES departments(id)
);

-- Sequences for IDs
CREATE SEQUENCE seq_departments START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE SEQUENCE seq_employees START WITH 1 INCREMENT BY 1 NOCACHE;
