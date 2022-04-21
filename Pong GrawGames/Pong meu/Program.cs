using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Pong
{
    class Program : GameWindow
    {
        public bool possoaumentar = true;

        public int Player1Points = 0;
        public int Player2Points = 0;
        
        protected bool P1UpColide = false;
        protected bool P1DonwColide =  false;
        protected bool P2UpColide = false;
        protected bool P2DonwColide = false;

        public float RBoll = 0.9f;
        public float GBoll = 1f;
        public float BBoll = 0.5f;

        public float XBoll = 0;
        public float YBoll = 0;

        public int BollTam = 20;

        public static float BInitialS = 3;
        public static float PInitialS = 5;

        public float BollSpeedX = BInitialS;
        public float BollSpeedY = BInitialS;

        public float PlayerSpeed = PInitialS;

        public float YPlayer1 = 0;
        public float YPlayer2 = 0;

        public float XPlayer1()
        {
            return -ClientSize.Width / 2 + LargPlayers() / 2;
        }

        public float XPlayer2()
        {
            return ClientSize.Width / 2 - LargPlayers() / 2;
        }

        public float AltPlayers()
        {
            return 4 * BollTam;
        }

        public float LargPlayers()
        {
            return BollTam;
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            XBoll += BollSpeedX;
            YBoll += BollSpeedY;
            CollideTests();

            Console.WriteLine("Blue: " + Player1Points + " Red: " + Player2Points + " BSpeed: " + BollSpeedX + " " + BollSpeedY);

            if(BollSpeedX >+ 6|| BollSpeedY >= 6)
            {
                possoaumentar = false;
                BollSpeedX = 6f;
                BollSpeedY = 6f;
            }

            if (XBoll + BollTam / 2 > XPlayer2() - LargPlayers() / 2 && YBoll - BollTam / 2 < YPlayer2 + AltPlayers() / 2 && YBoll + BollTam / 2 > YPlayer2 - AltPlayers() / 2)//Detector contato parede da Direita
            {
                BollSpeedX *= -1;
                Acelerar();
            }
            if (XBoll - BollTam / 2 < XPlayer1() + LargPlayers() / 2 && YBoll - BollTam / 2 < YPlayer1 + AltPlayers() / 2 && YBoll + BollTam / 2 > YPlayer1 - AltPlayers() / 2)//Detector contato parede da Esquerda
            {
                BollSpeedX *= -1;
                Acelerar();
            }

            if (XBoll < -ClientSize.Width / 2)//saiu fora Player 1
            {
                XBoll = 0;
                YBoll = 0;
                BollSpeedX = BInitialS;
                BollSpeedY = BInitialS;
                PlayerSpeed = PInitialS;

                possoaumentar = true;

                Player2Points++;
            }
            if(XBoll > ClientSize.Width / 2)
            {
                XBoll = 0;
                YBoll = 0;
                BollSpeedX = BInitialS;
                BollSpeedY = BInitialS;
                PlayerSpeed = PInitialS;

                Player1Points++;
            }

            if (YBoll + BollTam / 2 > ClientSize.Height / 2)//Detector contato parede de cima
            {
                BollSpeedY *= -1;
            }
            if (YBoll - BollTam / 2 < -ClientSize.Height / 2)//Detector contato parede de baixo
            {
                BollSpeedY *= -1;
            }
                                
            if(!P1UpColide)
            {
                if (Keyboard.GetState().IsKeyDown(Key.W))//Player 1 controle cima
                {
                    YPlayer1 += PlayerSpeed;
                }
            }
            if(!P1DonwColide)
            {
                if (Keyboard.GetState().IsKeyDown(Key.S))//Player 1 controle baixo
                {
                    YPlayer1 -= PlayerSpeed;
                }
            }
            if (!P2UpColide)
            {
                if (Keyboard.GetState().IsKeyDown(Key.Up))//Player 2 controle cima
                {
                    YPlayer2 += PlayerSpeed;
                }
            }
            if (!P2DonwColide)
            {
                if (Keyboard.GetState().IsKeyDown(Key.Down))//Player 2 controle baixo
                {
                    YPlayer2 -= PlayerSpeed;
                }
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {

            GL.Viewport(0, 0, ClientSize.Width, ClientSize.Height);

            Matrix4 projectation = Matrix4.CreateOrthographic(ClientSize.Width, ClientSize.Height, 0.0f, 1.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projectation);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            DQuadrilatero(XBoll, YBoll, BollTam, BollTam, RBoll, GBoll, BBoll);//Bola
            DQuadrilatero(XPlayer1(), YPlayer1, LargPlayers(), AltPlayers(), 0, 0, 1);//Jogador 1
            DQuadrilatero(XPlayer2(), YPlayer2, LargPlayers(), AltPlayers(), 1, 0, 0.1f);//Jogador 2

            SwapBuffers();
        }

        static void Main()
        {
            new Program().Run();
        }

        void DQuadrilatero(float x, float y, float alt, float larg, float R, float G, float B)
        {
            GL.Color3(R, G, B);

            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(-0.5f * alt + x, -0.5f * larg + y);
            GL.Vertex2(0.5f * alt + x, -0.5f * larg + y);
            GL.Vertex2(0.5f * alt + x, 0.5f * larg + y);
            GL.Vertex2(-0.5f * alt + x, 0.5f * larg + y);
            GL.End();
        }

        protected void Acelerar()
        {
            if(possoaumentar)
            {
                BollSpeedX *= 1.1f;
                BollSpeedY *= 1.1f;

                PlayerSpeed *= 1.1f;
            }
        }

        public void CollideTests()
        {
            if (YPlayer1 + AltPlayers() / 2 > ClientSize.Height / 2)//Detector contato parede de cima P1
            {
                P1UpColide = true;
                P1DonwColide = false;
            }
            else if (YPlayer1 - AltPlayers() / 2 < -ClientSize.Height / 2)//Detector contato parede de baixo P1
            {
                P1DonwColide = true;
                P1UpColide = false;
            }
            else
            {
                P1UpColide = false;
                P1DonwColide = false;
            }

            if (YPlayer2 + AltPlayers() / 2 > ClientSize.Height / 2)//Detector contato parede de cima P2
            {
                P2UpColide = true;
                P2DonwColide = false;
            }
            else if (YPlayer2 - AltPlayers() / 2 < -ClientSize.Height / 2)//Detector contato parede de baixo P2
            {
                P2DonwColide = true;
                P2UpColide = false;
            }
            else
            {
                P2UpColide = false;
                P2DonwColide = false;
            }
        }
    }
}