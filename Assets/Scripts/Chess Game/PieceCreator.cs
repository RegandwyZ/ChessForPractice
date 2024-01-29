using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCreator : MonoBehaviour
{
   [SerializeField] private GameObject[] _piecesPrefabs;
   [SerializeField] private Material _blackMaterial;
   [SerializeField] private Material _whiteMaterial;

   private Dictionary<string, GameObject> _nameToPieceDict = new Dictionary<string, GameObject>();

   private void Awake()
   {
      foreach (var piece in _piecesPrefabs)
      {
         _nameToPieceDict.Add(piece.GetComponent<Piece>().GetType().ToString(), piece);
      }
   }

   public GameObject CreatePiece(Type type)
   {
      GameObject prefab = _nameToPieceDict[type.ToString()];
      if (prefab)
      {
         GameObject newPiece = Instantiate(prefab);
         return newPiece;
      }

      return null;
   }

   public Material GetTeamMaterial(TeamColor color)
   {
      return color == TeamColor.White ? _whiteMaterial : _blackMaterial;
   }
}
