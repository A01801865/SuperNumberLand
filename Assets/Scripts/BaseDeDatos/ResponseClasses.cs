using UnityEngine;
using System.Collections.Generic;

// Respuesta del servidor al consultar el saldo de monedas del jugador
[System.Serializable]
public class MonedasResponse
{
    public bool success;
    public int  monedas;
}

// Respuesta del servidor al consultar los ítems disponibles en la tienda
[System.Serializable]
public class TiendaResponse
{
    public bool             success;
    public List<ItemTienda> items;
}

// Datos que se envían al servidor al realizar una compra
[System.Serializable]
public class CompraData
{
    public int id_usuario;
    public int id_item;
}

// Respuesta del servidor tras procesar una compra
[System.Serializable]
public class CompraResponse
{
    public bool   success;
    public string message;          // Mensaje de confirmación o error
    public int    monedas_restantes; // Saldo del jugador después de la compra
}