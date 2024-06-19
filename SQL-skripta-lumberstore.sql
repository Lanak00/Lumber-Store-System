INSERT INTO Addresses
VALUES (1, "Blagoja Parovica", 92, "Gajdobra", "Srbija");

INSERT INTO Users 
VALUES (1, "Lana", "Kovacevic", "kovaceviclana2501@gmail.com", "pass123", "2000-01-25", "0604706707", 1),
	   (2, "Nemanja", "Kovacevic", "kovacevicnemanja1997@gmail.com", "pass123", "1997-05-21", "0656684150", 1);

INSERT INTO Clients
VALUES (1);

INSERT INTO Employees
VALUES (2, "2105999805021", "https://cdn-icons-png.flaticon.com/512/219/219970.png", 0);

INSERT INTO Dimensions
VALUES (1, 200, 80, 5),
	   (2, 180, 100, 5);

INSERT INTO Products
VALUES ("A954 MN", "Bela Iverica", "", 0, "Arcus", 0, 2000, "https://www.darexhome.rs/uploads/documents/empire_plugin/1360728.jpg", 1),
	   ("A852 MN", "Oplemenjena Iverica", "", 0, "Manu", 0, 2300, "https://www.arcus.rs/wp-content/uploads/2021/02/Egger_H3176_ST37.jpg", 1),
       ("A588 MN", "Hrast - Oplemenjena Iverica", "", 0, "Tarket", 0, 2800, "https://trgovina.slovenijales.si/images/1c/1c1943ab76b1f521fc56ce66469a7cec/furnirana-iverica-hrast-grca.webp", 1),
	   ("A589 NN", "Hrast - Oplemenjena Iverica", "", 0, "Arcus", 0, 2750, "https://trgovina.slovenijales.si/images/1c/1c1943ab76b1f521fc56ce66469a7cec/furnirana-iverica-hrast-grca.webp", 2),
       ("B582 SN", "Juzni Jasen - Oplemenjena Iverica", "", 0, "Manu", 0, 2500, "https://unistil.rs/wp-content/uploads/2022/04/South-Ash-570-e1554807563334.jpg", 1),
       ("B572 SM", "Orleas Braon - Oplemenjena Iverica", "", 0, "Manu", 0, 2500, "https://glorija.rs/assets/images/product/H1379.jpg", 1),
       ("H442 NH", "Hamilton Hrast - Oplemenjena Iverica", "", 0, "Manu", 0, 2800, "https://www.arcus.rs/wp-content/uploads/2018/12/arcus-oplemenjena-iverica-Egger-H3303-ST10-1.jpg", 1),
       ("H445 NH", "Crni Hrast - Oplemenjena Iverica", "", 0, "Arcus", 0, 2800, "https://www.arcus.rs/wp-content/uploads/2018/12/H1199ST24.jpg", 1);
       
INSERT INTO Orders
VALUES (1, "2024-06-19", 0, 1),
	   (2, "2024-06-17", 0, 1),
       (3, "2024-03-15", 1, 1);
       
INSERT INTO Orderitems
VALUES (9, 20, 3, "A954 MN"),
	   (1, 3, 1, "H445 NH"),
	   (2, 10, 1, "H442 NH"),
       (3, 3, 1, "B582 SN"),
       (4, 10, 2, "H445 NH"),
       (5, 5, 2, "B582 SN"),
       (6, 5, 2, "A852 MN"),
       (7, 10, 2, "H442 NH"),
       (8, 10, 2, "H445 NH");

