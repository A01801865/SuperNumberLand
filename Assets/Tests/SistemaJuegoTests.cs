using System.Collections.Generic;
using NUnit.Framework;

public class SistemaJuegoTests
{
    private SistemaJuego sistema;
    private RepositorioUsuariosEnMemoria repositorio;

    [SetUp]
    public void Preparar()
    {
        sistema = new SistemaJuego();
        repositorio = new RepositorioUsuariosEnMemoria();
    }

    [Test]
    public void RegistroValido_CreaCuentaYCreaRegistroEnBaseDeDatos()
    {
        SistemaJuego.ResultadoOperacion resultado = sistema.Registrar("Mateo", "clave123", 9, repositorio);

        Assert.IsTrue(resultado.Exitoso);
        Assert.IsTrue(repositorio.ExisteUsuario("Mateo"));
        Assert.AreEqual(1, repositorio.TotalUsuarios);
    }

    [Test]
    public void LoginCorrecto_CredencialesValidasPermitenAcceso()
    {
        sistema.Registrar("Luna", "abc123", 10, repositorio);

        SistemaJuego.ResultadoOperacion resultado = sistema.Login("Luna", "abc123", repositorio);

        Assert.IsTrue(resultado.Exitoso);
    }

    [Test]
    public void LoginIncorrecto_CredencialesInvalidasRegresanError()
    {
        sistema.Registrar("Sofia", "secreto", 8, repositorio);

        SistemaJuego.ResultadoOperacion resultado = sistema.Login("Sofia", "otra", repositorio);

        Assert.IsFalse(resultado.Exitoso);
        Assert.AreEqual("Credenciales invalidas", resultado.Mensaje);
    }

    [Test]
    public void RestriccionPorEdad_BloqueaNivelRestringidoYPermiteSoloHabilitados()
    {
        HashSet<string> temasParaSieteAnios = sistema.ObtenerTemasHabilitados(7);

        Assert.IsTrue(temasParaSieteAnios.Contains("suma"));
        Assert.IsTrue(temasParaSieteAnios.Contains("resta"));
        Assert.IsFalse(sistema.PuedeSeleccionarTema("multiplicacion", 7));
        Assert.IsFalse(sistema.PuedeSeleccionarTema("division", 7));
        Assert.IsTrue(sistema.PuedeSeleccionarTema("multiplicacion", 8));
        Assert.IsTrue(sistema.PuedeSeleccionarTema("division", 10));
    }

    [Test]
    public void RespuestaCorrecta_ValidaYDesbloqueaSiguientePregunta()
    {
        SistemaJuego.ResultadoRespuesta resultado = sistema.ValidarRespuesta(12, 12, 3);

        Assert.IsTrue(resultado.Correcta);
        Assert.IsTrue(resultado.SiguientePreguntaDesbloqueada);
        Assert.AreEqual(3, resultado.VidasRestantes);
    }

    [Test]
    public void RespuestaIncorrecta_RestaUnaVida()
    {
        SistemaJuego.ResultadoRespuesta resultado = sistema.ValidarRespuesta(5, 9, 3);

        Assert.IsFalse(resultado.Correcta);
        Assert.IsFalse(resultado.SiguientePreguntaDesbloqueada);
        Assert.AreEqual(2, resultado.VidasRestantes);
    }

    [Test]
    public void SinVidas_CuandoVidasEsCeroJuegoTermina()
    {
        Assert.IsTrue(sistema.JuegoTerminado(0));

        SistemaJuego.ResultadoRespuesta resultado = sistema.ValidarRespuesta(1, 2, 1);

        Assert.AreEqual(0, resultado.VidasRestantes);
        Assert.IsTrue(resultado.JuegoTerminado);
    }

    [Test]
    public void Monedas_AlRecolectarMonedaAumentaContador()
    {
        int total = sistema.RecolectarMoneda(4);

        Assert.AreEqual(5, total);
    }

    [Test]
    public void CompraConMonedasSuficientes_DescuentaCostoYPermiteCompra()
    {
        SistemaJuego.ResultadoCompra resultado = sistema.ComprarConMonedas(100, 40);

        Assert.IsTrue(resultado.CompraPermitida);
        Assert.AreEqual(60, resultado.MonedasRestantes);
    }

    private class RepositorioUsuariosEnMemoria : SistemaJuego.IRepositorioUsuarios
    {
        private readonly Dictionary<string, SistemaJuego.UsuarioCuenta> usuarios =
            new Dictionary<string, SistemaJuego.UsuarioCuenta>();

        public int TotalUsuarios
        {
            get { return usuarios.Count; }
        }

        public bool ExisteUsuario(string apodo)
        {
            return usuarios.ContainsKey(apodo);
        }

        public void CrearUsuario(SistemaJuego.UsuarioCuenta usuario)
        {
            usuarios[usuario.Apodo] = usuario;
        }

        public SistemaJuego.UsuarioCuenta BuscarUsuario(string apodo)
        {
            usuarios.TryGetValue(apodo, out SistemaJuego.UsuarioCuenta usuario);
            return usuario;
        }
    }
}
