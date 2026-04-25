using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MonedasResponse
{
    public bool success;
    public int  monedas;
}

[System.Serializable]
public class TiendaResponse
{
    public bool             success;
    public List<ItemTienda> items;
}

[System.Serializable]
public class CompraData
{
    public int id_usuario;
    public int id_item;
}

[System.Serializable]
public class CompraResponse
{
    public bool   success;
    public string message;
    public int    monedas_restantes;
}