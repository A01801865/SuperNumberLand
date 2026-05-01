using System;
using System.Collections.Generic;

public class SistemaJuego
{
    public const int EdadMinimaMultiplicacion = 8;
    public const int EdadMinimaDivision = 10;

    public interface IRepositorioUsuarios
    {
        bool ExisteUsuario(string apodo);
        void CrearUsuario(UsuarioCuenta usuario);
        UsuarioCuenta BuscarUsuario(string apodo);
    }

    public class UsuarioCuenta
    {
        public string Apodo { get; private set; }
        public string Contrasena { get; private set; }
        public int Edad { get; private set; }

        public UsuarioCuenta(string apodo, string contrasena, int edad)
        {
            Apodo = apodo;
            Contrasena = contrasena;
            Edad = edad;
        }
    }

    public class ResultadoOperacion
    {
        public bool Exitoso { get; private set; }
        public string Mensaje { get; private set; }

        public ResultadoOperacion(bool exitoso, string mensaje)
        {
            Exitoso = exitoso;
            Mensaje = mensaje;
        }
    }

    public class ResultadoRespuesta
    {
        public bool Correcta { get; private set; }
        public bool SiguientePreguntaDesbloqueada { get; private set; }
        public int VidasRestantes { get; private set; }
        public bool JuegoTerminado { get; private set; }

        public ResultadoRespuesta(bool correcta, bool siguientePreguntaDesbloqueada, int vidasRestantes, bool juegoTerminado)
        {
            Correcta = correcta;
            SiguientePreguntaDesbloqueada = siguientePreguntaDesbloqueada;
            VidasRestantes = vidasRestantes;
            JuegoTerminado = juegoTerminado;
        }
    }

    public class ResultadoCompra
    {
        public bool CompraPermitida { get; private set; }
        public int MonedasRestantes { get; private set; }

        public ResultadoCompra(bool compraPermitida, int monedasRestantes)
        {
            CompraPermitida = compraPermitida;
            MonedasRestantes = monedasRestantes;
        }
    }

    public ResultadoOperacion Registrar(string apodo, string contrasena, int edad, IRepositorioUsuarios repositorio)
    {
        if (repositorio == null)
            throw new ArgumentNullException(nameof(repositorio));

        apodo = Normalizar(apodo);
        contrasena = Normalizar(contrasena);

        if (!CredencialesValidas(apodo, contrasena))
            return new ResultadoOperacion(false, "Apodo y contrasena requeridos");

        if (edad <= 0)
            return new ResultadoOperacion(false, "Edad invalida");

        if (repositorio.ExisteUsuario(apodo))
            return new ResultadoOperacion(false, "El usuario ya existe");

        repositorio.CrearUsuario(new UsuarioCuenta(apodo, contrasena, edad));
        return new ResultadoOperacion(true, "Cuenta creada");
    }

    public ResultadoOperacion Login(string apodo, string contrasena, IRepositorioUsuarios repositorio)
    {
        if (repositorio == null)
            throw new ArgumentNullException(nameof(repositorio));

        apodo = Normalizar(apodo);
        contrasena = Normalizar(contrasena);

        if (!CredencialesValidas(apodo, contrasena))
            return new ResultadoOperacion(false, "Completa todos los campos");

        UsuarioCuenta usuario = repositorio.BuscarUsuario(apodo);
        if (usuario == null || usuario.Contrasena != contrasena)
            return new ResultadoOperacion(false, "Credenciales invalidas");

        return new ResultadoOperacion(true, "Acceso permitido");
    }

    public HashSet<string> ObtenerTemasHabilitados(int edad, bool multiplicacionPorRendimiento = false, bool divisionPorRendimiento = false)
    {
        HashSet<string> temas = new HashSet<string> { "suma", "resta" };

        if (edad >= EdadMinimaMultiplicacion || multiplicacionPorRendimiento)
            temas.Add("multiplicacion");

        if (edad >= EdadMinimaDivision || divisionPorRendimiento)
            temas.Add("division");

        return temas;
    }

    public bool PuedeSeleccionarTema(string tema, int edad, bool multiplicacionPorRendimiento = false, bool divisionPorRendimiento = false)
    {
        return ObtenerTemasHabilitados(edad, multiplicacionPorRendimiento, divisionPorRendimiento)
            .Contains(Normalizar(tema));
    }

    public ResultadoRespuesta ValidarRespuesta(int respuestaJugador, int respuestaCorrecta, int vidasActuales)
    {
        if (respuestaJugador == respuestaCorrecta)
            return new ResultadoRespuesta(true, true, Math.Max(0, vidasActuales), vidasActuales <= 0);

        int vidasRestantes = PerderVida(vidasActuales);
        return new ResultadoRespuesta(false, false, vidasRestantes, JuegoTerminado(vidasRestantes));
    }

    public int PerderVida(int vidasActuales)
    {
        return Math.Max(0, vidasActuales - 1);
    }

    public bool JuegoTerminado(int vidasActuales)
    {
        return vidasActuales <= 0;
    }

    public int RecolectarMoneda(int monedasActuales, int cantidad = 1)
    {
        return Math.Max(0, monedasActuales) + Math.Max(0, cantidad);
    }

    public ResultadoCompra ComprarConMonedas(int monedasActuales, int costo)
    {
        if (costo < 0)
            throw new ArgumentOutOfRangeException(nameof(costo), "El costo no puede ser negativo");

        if (monedasActuales < costo)
            return new ResultadoCompra(false, Math.Max(0, monedasActuales));

        return new ResultadoCompra(true, monedasActuales - costo);
    }

    private static bool CredencialesValidas(string apodo, string contrasena)
    {
        return !string.IsNullOrWhiteSpace(apodo) && !string.IsNullOrWhiteSpace(contrasena);
    }

    private static string Normalizar(string valor)
    {
        return string.IsNullOrWhiteSpace(valor) ? string.Empty : valor.Trim();
    }
}
