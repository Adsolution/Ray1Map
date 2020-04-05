﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace R1Engine
{
    /// <summary>
    /// Defines a common tile-set
    /// </summary>
    public class Common_Tileset
    {
        /// <summary>
        /// Creates a tile set from a tile map
        /// </summary>
        /// <param name="tileMapColors">The tile map colors</param>
        /// <param name="tileMapWidth">The tile map width, in tiles</param>
        /// <param name="cellSize">The tile size</param>
        public Common_Tileset(IList<ARGBColor> tileMapColors, int tileMapWidth, int cellSize)
        {
            // Create the tile array
            Tiles = new Tile[tileMapColors.Count];

            // Create each tile
            for (var index = 0; index < tileMapColors.Count / (cellSize * cellSize); index++)
            {
                // Create the texture
                Texture2D tex = new Texture2D(cellSize, cellSize, TextureFormat.RGBA32, false)
                {
                    filterMode = FilterMode.Point,
                    wrapMode = TextureWrapMode.Clamp
                };

                // Get the tile x and y
                var tileY = (int)Math.Floor(index / (double)tileMapWidth);
                var tileX = (index - (tileMapWidth * tileY));

                var tileOffset = (tileY * tileMapWidth * cellSize * cellSize) + (tileX * cellSize);

                // Set every pixel
                for (int y = 0; y < cellSize; y++)
                {
                    for (int x = 0; x < cellSize; x++)
                    {
                        tex.SetPixel(x, y, tileMapColors[(tileOffset + (y * cellSize * tileMapWidth + x))].GetColor());
                    }
                }

                // Apply the pixels
                tex.Apply();

                // Create a tile
                Tile t = ScriptableObject.CreateInstance<Tile>();
                t.sprite = Sprite.Create(tex, new Rect(0, 0, cellSize, cellSize), new Vector2(0.5f, 0.5f), cellSize, 20);

                Tiles[index] = t;
            }
        }

        /// <summary>
        /// Creates a tile set from a tile-set texture
        /// </summary>
        /// <param name="tileSet">The tile-set texture</param>
        /// <param name="cellSize">The tile size</param>
        public Common_Tileset(Texture2D tileSet, int cellSize)
        {
            // Create the tile array
            Tiles = new Tile[(tileSet.width / cellSize) * (tileSet.height / cellSize)];

            // Keep track of the index
            var index = 0;

            // Extract every tile
            for (int y = 0; y < tileSet.height; y += cellSize)
            {
                for (int x = 0; x < tileSet.width; x += cellSize)
                {
                    // Create a tile
                    Tile t = ScriptableObject.CreateInstance<Tile>();
                    t.sprite = Sprite.Create(tileSet, new Rect(x, y, cellSize, cellSize), new Vector2(0.5f, 0.5f), cellSize, 20);

                    Tiles[index] = t;

                    index++;
                }
            }
        }

        /// <summary>
        /// Creates a tile set from a tile array
        /// </summary>
        /// <param name="tiles">The tiles in this set</param>
        public Common_Tileset(Tile[] tiles)
        {
            Tiles = tiles;
        }

        /// <summary>
        /// The tiles in this set
        /// </summary>
        public Tile[] Tiles { get; }

        /// <summary>
        /// Sets a tile from a texture at the specified index
        /// </summary>
        /// <param name="texture">The texture to use for the tile</param>
        /// <param name="size">The size of the tile</param>
        /// <param name="index">The index to set to</param>
        public void SetTile(Texture2D texture, int size, int index)
        {
            Tiles[index] = ScriptableObject.CreateInstance<Tile>();
            Tiles[index].sprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size, 20);
        }
    }
}