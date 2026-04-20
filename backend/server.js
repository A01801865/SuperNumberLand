const express = require('express');
const cors = require('cors');

const app = express();

app.use(cors());
app.use(express.json());

const db = require('./db');

// Ruta de prueba
app.get('/test', (req, res) => {
  res.json({ mensaje: "API funcionando 🚀" });
});

// Usuarios
app.get('/usuarios', (req, res) => {
  db.query('SELECT * FROM Usuario', (err, result) => {
    if (err) {
      console.error(err);
      return res.status(500).json(err);
    }
    res.json(result);
  });
});

app.listen(3000, () => {
  console.log('Servidor corriendo en puerto 3000');
});

// Login
app.post('/login', (req, res) => {
  const { usuario, contrasena } = req.body;

  const sql = `
    SELECT * FROM Usuario 
    WHERE nombre_usuario = ? AND contrasena = ?
  `;

  db.query(sql, [usuario, contrasena], (err, result) => {
    if (err) {
      console.error(err);
      return res.status(500).json(err);
    }

    if (result.length > 0) {
      res.json({ success: true, user: result[0] });
    } else {
      res.json({ success: false, message: "Credenciales incorrectas" });
    }
  });
});

// Preguntas
app.get('/preguntas/:nivel', (req, res) => {
  const nivel = req.params.nivel;

  const sql = `
    SELECT p.id_pregunta, p.enunciado, r.id_respuesta, r.respuesta_texto, r.es_correcta
    FROM Preguntas p
    JOIN Respuestas r ON p.id_pregunta = r.id_pregunta
    WHERE p.id_nivel = ?
  `;

  db.query(sql, [nivel], (err, result) => {
    if (err) {
      console.error(err);
      return res.status(500).json(err);
    }
    res.json(result);
  });
});

// Register
app.post('/register', (req, res) => {
  const { usuario, contrasena } = req.body;

  const sql = `
    INSERT INTO Usuario (nombre_usuario, contrasena, fecha_registro, dificultad_usuario)
    VALUES (?, ?, NOW(), 1)
  `;

  db.query(sql, [usuario, contrasena], (err, result) => {
    if (err) {
      console.error(err);
      return res.status(500).json({ success: false });
    }

    res.json({ success: true });
  });
});