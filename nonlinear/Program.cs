﻿using System;
using System.Numerics;

namespace nonlinear
{
    class Program
    {
        /*private static float PenaltyG(float A, float B, float C)
        {
            float val = GVal(A, B, C);
            if (val != 0) return 0;
            return val * val;
        }
        private static float PenaltyH(float A, float B, float C)
        {
            float val = HVal(A, B, C);
            if (val <= 0) return 0;
            return val * val;
        }*/


        //g(X) = 0
        //
        private static float VolumeSq(float A, float B, float C)
        {
            return 0.125f* A * B * C; // V^2
        }

        private static float GVal(float A, float B, float C)
        {
            //g(X) = A - 0.2 => A - 0.2 = 0
            return A + B + C - 1f;//A - 0.2f;
        }
      
        private static Vector3 PenaltyGradG(float A, float B, float C)
        {
            float val = GVal(A, B, C); // (A + B + C - 1f)^2 => 2(A + B + C - 1f)
            //if (Math.Abs(val) < epsilon) return new Vector3(0, 0, 0);
            return new Vector3(2f*val, 2f*val, 2f*val);
        }

        private static float HVal(float x)
        {
            //x >= 0 => A >0 B> 0 C>0
            //h(X) = B => B - 0.5 <= 0
            return -x; // -x <= 0 -A <= 0 // -B<=0
        }
      
        private static Vector3 PenaltyGradH(float x, int i)
        {
            float val = HVal(x); // -x <= 0
            if (val <= 0) return new Vector3(0, 0, 0); // nesuveiks jei x < 0

            if (i == 0) return new Vector3(2f * (x), 0, 0); // A <= 0 => A^2 => 2A
            if (i == 1) return new Vector3(0, 2f * (x), 0);
            return new Vector3(0, 0, 2f * (x));
        }

        private static Vector3 PenaltiesGradH(float A, float B, float C)
        {
            Vector3 h = PenaltyGradH(A, 0) + PenaltyGradH(B, 1) + PenaltyGradH(C, 2);
            return h;
        }



        private static double FVal(float A, float B, float C, float r)
        { // r -> 0
            return VolumeSq(A, B, C);// + 1 / r * (PenaltyG(A, B, C) + PenaltyH(A, B, C));
        }


        private static Vector3 Gradient(float A, float B, float C, float r)
        {
            Vector3 h = PenaltiesGradH(A, B, C);
            Vector3 g = PenaltyGradG(A, B, C);
            Vector3 grad = -(new Vector3(0.125f * B * C, 0.125f * A * C, 0.125f * A * B)); // V
            return grad + (1 / r) * (g + h);
        }

        private static float epsilon = 0.00001f;
        private static float epsilon2 = 0.0005f;
        private static float gamma = 0.8f;
        private static float rInit = 3f;
        private static void GradientDescent(Vector3 startPoint)
        {
            float k = 0.5f;
            int n = 0;
            float r = rInit;

            Vector3 pBefore = new Vector3(-100, -100, 100);
            float V = 0;
            Vector3 pos = startPoint;

            Console.WriteLine(pos);
            while (true)
            {
                V = VolumeSq(pos.X, pos.Y, pos.Z);
                pos = pos - gamma * Gradient(pos.X, pos.Y, pos.Z, r);
                
                r *= k; 
                gamma *= k;

                Console.WriteLine(pos + "   -->   " + V + " r -> " + r);

                if (Vector3.Distance(pos, pBefore) < epsilon2 || n > 100000 || float.IsNaN(pos.X)) break;
                pBefore = pos;
                n++;
            }
            Console.WriteLine(n);
        }


        //  0.9 0.5 0.3
        private static Vector3 p0 = new Vector3(0.4f, 0.4f, 0.4f);
        private static Vector3 p4 = new Vector3(-1f, 0, 0);

        private static Vector3 p1 = new Vector3(0, 0, 0);
        private static Vector3 p2 = new Vector3(1, 1, 1);
        private static Vector3 p3 = new Vector3(0.9f, 0.5f, 0.3f);
        static void Main(string[] args)
        {
            GradientDescent(p2);
        }
    }
}