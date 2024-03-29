﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace PixelBoard
{
    internal class DisplayHelper
    {
        internal sbyte height = 20;
        internal sbyte width = 10;

        internal sbyte framerate = 10;

        internal static System.Timers.Timer displayRefreshTimer;

        internal IPixel[,] lastBoard;
        internal IPixel[,] currentBoard;
        
        internal string lastLCDNumber = "";
        internal string currentLCDNumber = "";

        internal void SetFramerate(sbyte framerate)
        {
            if (framerate > 0 && framerate <= 60)
            {
                this.framerate = framerate;
            }
            else if (framerate > 60)
            {
                this.framerate = 60;
            }
            else
            {
                this.framerate = 1;
            }
        }

        internal void SetSize(sbyte height, sbyte width)
        {
            this.height = height;
            this.width = width;
        }

        internal void makeTimer(ElapsedEventHandler handler)
        {
            displayRefreshTimer = new Timer(1000 / this.framerate);
            displayRefreshTimer.Elapsed += handler;
            displayRefreshTimer.AutoReset = true;
            displayRefreshTimer.Enabled = true;
        }

        internal void ValidateLCDValue(int value, int max, string argumentName)
        {
            if (value > max)
            {
                throw new ArgumentOutOfRangeException(argumentName, "Int to display was over 6 digits, which is not valid");
            }
            else if (value < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName, "Int to display was negative, which is not valid");
            }
        }

        internal void Draw(IPixel[,] pixels)
        {
            if (pixels.GetLength(0) == this.height)
            {
                if (pixels.GetLength(1) == this.width)
                {
                    this.currentBoard = pixels;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("pixels", "pixels width was not the defined display width");
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("pixels", "pixels height was not the defined display height");
            }
        }

        internal void Draw(ILocatedPixel pixel)
        {
            if (pixel.Column < this.width)
            {
                if (pixel.Row < this.height)
                {
                    this.currentBoard[pixel.Row, pixel.Column] = (IPixel)pixel;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("pixel", "pixel row was not within the defined display height");
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("pixel", "pixel column was not within the defined display width");
            }
        }

        internal void DisplayInt(int value)
        {
            this.ValidateLCDValue(value, 999999, "value");
            this.currentLCDNumber = value.ToString(); ;
        }

        internal void DisplayInt(int value, bool? leftAligned)
        {
            this.ValidateLCDValue(value, 999999, "value");
            string paddedValue = value.ToString();
            if (leftAligned != null || leftAligned == true)
            {
                while (value < 99999 && value != 0)
                {
                    value *= 10;
                    paddedValue += " ";
                }
            }
            this.currentLCDNumber = paddedValue;
        }

        internal void DisplayInts(int leftValue, int rightValue)
        {
            this.ValidateLCDValue(leftValue, 999, "leftValue");
            this.ValidateLCDValue(rightValue, 999, "rightValue");

            string paddedLeft = leftValue.ToString();
            string paddedRight = rightValue.ToString();

            while (leftValue < 99 && leftValue != 0)
            {
                leftValue *= 10;
                paddedLeft += " ";
            }
            while (rightValue < 99 && rightValue != 0)
            {
                rightValue *= 10;
                paddedRight = " " + paddedRight;
            }
            if(leftValue == 0)
            {
                paddedLeft = "0  ";
            }
            if (rightValue == 0)
            {
                paddedRight = "  0";
            }
            leftValue += rightValue;

            this.currentLCDNumber = paddedLeft + paddedRight;
        }
    }
}
