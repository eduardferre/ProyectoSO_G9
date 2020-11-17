using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        int puerto = 9080;
        Socket server;
        int conectado;

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            conectado = 0;
        }


        private void Register_button_Click(object sender, EventArgs e)
        {
            if (conectado == 0)
            {
                //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
                //al que deseamos conectarnos
                IPAddress direc = IPAddress.Parse("192.168.56.102");
                IPEndPoint ipep = new IPEndPoint(direc, puerto);

                //Creamos el socket 
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    server.Connect(ipep);  //Intentamos connectar el socket.
                    this.BackColor = Color.Green;
                    string Nombre = Nombre_Register_txt.Text;
                    string Contraseña = Convert.ToString(Contraseña_Register_txt.Text);

                    //Ahora construiremos el mensaje, el cual tendrá la sigueinte estructura: "0/Nombre/Contraseña.
                    string mensaje = "1/" + Nombre + "/" + Contraseña;

                    //Enviamos al servidor el mensaje.
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor.
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

                    if (mensaje == "OK")
                    {
                        MessageBox.Show("Jugador registrado correctamente.");
                    }
                    else if (mensaje == "EXISTS")
                    {
                        MessageBox.Show("El usario no se ha añadido porque ya existe");
                    }


                    //Y ahora cerramos la conexión con el servidor.
                    conectado = 1;
                }
                catch
                {
                    //Si hay excepcion imprimimos error y salimos del programa con return 
                    MessageBox.Show("No he podido conectar con el servidor");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Eh, me estas buscando las cosquillas eh jajaja, ya estas logeado, and Desconectate si quieres registrarte con otro usuario mastodonte");
            }
        }


        private void Login_button_Click(object sender, EventArgs e)
        {
            if (conectado == 0)
            {
                //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
                //al que deseamos conectarnos
                IPAddress direc = IPAddress.Parse("192.168.56.102");
                IPEndPoint ipep = new IPEndPoint(direc, puerto);

                //Creamos el socket 
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    server.Connect(ipep);  //Intentamos connectar el socket.
                    string Nombre = Nombre_Login_txt.Text;
                    string Contraseña = Convert.ToString(Contraseña_Login_txt.Text);

                    //Ahora construiremos el mensaje, el cual tendrá la sigueinte estructura: "1/Nombre/Contraseña.
                    string mensaje = "2/" + Nombre + "/" + Contraseña;

                    //Enviamos al servidor el mensaje.
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor.
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                    if (mensaje == "FOUND")
                    {
                        MessageBox.Show("Bienvenido de nuevo " + Nombre + ".");
                        this.BackColor = Color.Green;
                        conectado = 1;
                    }
                    else
                    {
                        MessageBox.Show("Lo siento, no estás registrad@ todavía.");
                        this.BackColor = Color.Gray;
                        server.Shutdown(SocketShutdown.Both);
                        server.Close();
                    }
                }
                catch
                {
                    //Si hay excepcion imprimimos error y salimos del programa con return 
                    MessageBox.Show("No he podido conectar con el servidor");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Eh, que ya estas conectado! Si te quieres perder tiempo volviendo a conectarte puedes pulsar Desconectar y volver a hacer Login xD");
            }
        }
        

        private void EnviarConsultas_button_Click(object sender, EventArgs e)
        {
            if (conectado == 1)
            {
                try
                {
                    this.BackColor = Color.Green;


                    if (Consulta1.Checked)
                    {
                        // Quiere saber la longitud
                        string mensaje = "3/Consulta 1/";
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);

                        //Recibimos la respuesta del servidor
                        byte[] msg2 = new byte[80];
                        server.Receive(msg2);
                        mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                        MessageBox.Show("La media de la duración de las partidas es: " + mensaje + " segundos");
                    }

                    else if (Consulta2.Checked)
                    {
                        // Quiere saber si el nombre es bonito
                        string mensaje = "4/Consulta 2/" + JugadorConsulta_txt.Text;
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);

                        //Recibimos la respuesta del servidor
                        byte[] msg2 = new byte[80];
                        server.Receive(msg2);
                        mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                        MessageBox.Show("La duración de la partida más larga de " + JugadorConsulta_txt.Text + " es: " + mensaje);
                    }

                    else if (Consulta3.Checked)
                    {
                        // Quiere saber si el nombre es bonito
                        string mensaje = "5/Consulta 3/" + RolConsultas_txt.Text + "/" + FechaConsultas_txt.Text;
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);

                        //Recibimos la respuesta del servidor
                        byte[] msg2 = new byte[80];
                        server.Receive(msg2);
                        mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                        MessageBox.Show("El jugador con más victorias siendo " + RolConsultas_txt.Text + " el día " + FechaConsultas_txt.Text + " es: " + mensaje);
                    }

                    else if (Consulta4.Checked)
                    {
                        // Quiere saber si el nombre es bonito
                        string mensaje = "6/Consulta 4/" + JugadorConsulta_txt.Text;
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);

                        //Recibimos la respuesta del servidor
                        byte[] msg2 = new byte[80];
                        server.Receive(msg2);
                        mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                        MessageBox.Show("El rol de la partida más larga de " + JugadorConsulta_txt.Text + " es: " + mensaje);
                    }

                    else
                    {
                        // Informamos al servidor que no se realiza ninguna consulta, usaremos el codigo 112 (emergencia)
                        string mensaje = "112/";
                        // Enviamos al servidor
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                        MessageBox.Show("Selecciona alguna consulta antes de nada");
                    }

                    // Se terminó el servicio. 
                }
                catch (SocketException)
                {
                    //Si hay excepcion imprimimos error y salimos del programa con return 
                    MessageBox.Show("No he podido conectar con el servidor");
                    return;
                }
            }
            else
            {
                MessageBox.Show("No puedes realizar una consulta sin iniciar sesión primero con un usario ya registrado.");
            }
        }


        private void Desconectar_button_Click(object sender, EventArgs e)
        {
            if (conectado == 1)
            {
                // Mensaje de desconexión
                string mensaje = "0/";

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Nos desconectamos
                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();

                conectado = 0;
            }
            else
            {
                MessageBox.Show("Eh, que no estas conectado! Puedes volverte a conectar haciendo Login ;D");
            }
        }


        private void Servicios_button_Click(object sender, EventArgs e)
        {
            if (conectado == 1)
            {
                // Mensaje de petición de servicios
                string mensaje = "111/";

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                Servicios_lbl.Text = mensaje;
            }
            else
            {
                MessageBox.Show("Eh, que no estás conectado chaval, no estás autorizad@ <3");
            }

        }


        private void Conectados_button_Click(object sender, EventArgs e)
        {
            if (conectado == 1)
            {
                Conectados_lbl.Clear();
                // Mensaje de petición de lista
                string mensaje = "110/";

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

                string[] separadas;
                separadas = mensaje.Split('/');

                for (int i = 0; i < Convert.ToInt32(separadas[0]); i++)
                {
                    Conectados_lbl.AppendText(separadas[i + 1]);
                    Conectados_lbl.AppendText(Environment.NewLine);
                }
            }
            else
            {
                MessageBox.Show("Eh, que no estás conectado chaval, no estás autorizad@ <3");
            }
        }
    }
}
