using UnityEngine;

[System.Serializable]
public class ItemTienda
{
    public int id_item;
    public string nombre;
    public int precio;
    public bool comprado = false;
    public Sprite imagen;
}