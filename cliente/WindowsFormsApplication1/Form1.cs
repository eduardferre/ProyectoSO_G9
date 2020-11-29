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
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        int puerto = 50078;
        string nombreConectado;
        Socket server;
        Thread atender;
        int conectado = 0;
        int numConectados; //Número de conectados sin contar el propio cliente. 
        int numRespuestas; //Número de respuestas a la invitación que se han recibido (tanto afirmativas como negativas).
        int numRespuestasAceptar; //Número de jugadores que han aceptado la invitación de jugar.

        delegate void DelegadoParaEscribir(string mensaje);

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false; //Necesario para que los elementos de los formularios puedan ser
            //accedidos desde threads diferentes a los que los crearon
        }

        private void AtenderServidor()
        {
            while (true)
            {
                //Recibimos el mensaje del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                string[] trozos = Encoding.ASCII.GetString(msg2).Split('/');
                int codigo = Convert.ToInt32(trozos[0]);
                string mensaje = trozos[1].Split('\0')[0];

                switch (codigo)
                {

                    case 1: //registro
                        if (mensaje == "OK")
                            {
                                MessageBox.Show("Jugador registrado correctamente.");
                            }
                        else if (mensaje == "EXISTS")
                            {
                                MessageBox.Show("El usario no se ha añadido porque ya existe");
                            }
                            conectado = 1;

                        break;

                    case 2: //login
                        string Nombre = Nombre_Login_txt.Text;
                        string Contraseña = Convert.ToString(Contraseña_Login_txt.Text);
                        if (mensaje == "FOUND")
                            {
                                nombreConectado = Nombre_Login_txt.Text;
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
                                   
                        break;

                    case 3: //Consulta 1

                        MessageBox.Show("La media de la duración de las partidas es: " + mensaje + " segundos");
                        break;

                    case 4: //Consulta 2

                        MessageBox.Show("La duración de la partida más larga de " + JugadorConsulta_txt.Text + " es: " + mensaje);
                        break;

                    case 5: //Consulta 3

                        MessageBox.Show("El jugador con más victorias siendo " + RolConsultas_txt.Text + " el día " + FechaConsultas_txt.Text + " es: " + mensaje);
                        break;

                    case 6: //Consulta 4

                        MessageBox.Show("El rol de la partida más larga de " + JugadorConsulta_txt.Text + " es: " + mensaje);
                        break;

                    case 7: //Invitación para jugar
                        Respuestas_lbl.Clear();
                        string nombre_huesped = mensaje;
                        if (nombre_huesped != nombreConectado) //El propio huésped no debe ver la invitación generada por si mismo.
                        {
                            DialogResult dialogResult;
                            dialogResult = MessageBox.Show(nombre_huesped + " ha enviado una solicitud para iniciar la partida. ¿Desea iniciar la partida ahora? Todos los jugadores deben estar de acuerdo para iniciar la partida.", "Confirmación", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                enviarRespuestaInvitacion(nombreConectado, 1); //La respuesta es sí.
                            }
                            else
                            {
                                enviarRespuestaInvitacion(nombreConectado, 0);  //La respuesta es no.
                            }
                        }
                        break;

                    case 71: //Respuesta a la invitación.
                        string nombre_respuesta = trozos[1];
                        int respuesta = Convert.ToInt32 (trozos[2].Split('\0')[0]);
                        if (respuesta == 1) //Lo que significa que el jugador ha aceptado la invitación. Solo se añaden al label los que aceptan.
                        {
                            Respuestas_lbl.AppendText(nombre_respuesta); //En este caso, uno mismo sí va a poder verse en el label de personas que han aceptado.
                            Respuestas_lbl.AppendText(Environment.NewLine);
                            numRespuestasAceptar++;
                            numRespuestas++;
                        }
                        else
                        {
                            numRespuestas++;
                        }
                        if (numRespuestas == numConectados) //Significa que ya han respondido todos.
                        {
                            if (numRespuestasAceptar == numRespuestas) //Significa que todas las respuestas son afirmativas => Empezará la partida.
                            {
                                MessageBox.Show("Todos los jugadores han aceptado la invitación. La partida puede empezar.");
                            }
                            else //Sigifica que alguien ha denegado la invitación => La partida NO empezará.
                            {
                                MessageBox.Show("La partida no empezará. NO todos los jugadores han aceptado la invitación.");
                            }
                            numRespuestas = 0;
                            numRespuestasAceptar = 0;
                        }

                        break;
                        
                     
                    case 110: //Lista conectados
                        Conectados_lbl.Clear();
                        numConectados = 0;
                        string[] separadas;
                        separadas = mensaje.Split('/');

                        for (int i = 0; i < Convert.ToInt32(separadas[0]); i++)
                        {
                            if (trozos[i + 2] != nombreConectado)  //De esta forma, el jugador conectado no ve su propio nombre dentro de la lista de conectados, solo los de los demás.
                            {
                                Conectados_lbl.AppendText(trozos[i + 2]);
                                Conectados_lbl.AppendText(Environment.NewLine);
                                numConectados++;
                            }
                        }
                        break;
                     
                    case 111: //Número de servicios

                        DelegadoParaEscribir delegado = new DelegadoParaEscribir(PonContador);
                        Servicios_lbl.Invoke(delegado, new object[] {mensaje});  //Invocamos al thread que creó este objeto para que haga lo que se especifica a continuacion.
                        break;
                }
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            numConectados = 0;
        }

        public void PonContador (string mensaje)
        {
             Servicios_lbl.Text = mensaje;
        }
        private void Register_button_Click(object sender, EventArgs e)
        {
            if (conectado == 0)
            {
                //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
                //al que deseamos conectarnos
                IPAddress direc = IPAddress.Parse("147.83.117.22");
                IPEndPoint ipep = new IPEndPoint(direc, puerto);

                this.BackColor = Color.Green;
                string Nombre = Nombre_Register_txt.Text;
                string Contraseña = Convert.ToString(Contraseña_Register_txt.Text);

                //Creamos el socket 
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                if ((Nombre_Register_txt.Text != "") && (Contraseña_Register_txt.Text != ""))
                {
                    try
                    {
                        server.Connect(ipep);  //Intentamos connectar el socket.


                        //Ahora construiremos el mensaje, el cual tendrá la sigueinte estructura: "0/Nombre/Contraseña.
                        string mensaje = "1/" + Nombre + "/" + Contraseña;

                        //Enviamos al servidor el mensaje.
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                    }
                    catch
                    {
                        //Si hay excepcion imprimimos error y salimos del programa con return 
                        MessageBox.Show("No he podido conectar con el servidor");
                        return;
                    }
                    //pongo en marcha el thread que atenderá los mensajes del servidor
                    ThreadStart ts = delegate { AtenderServidor(); };
                    atender = new Thread(ts);
                    atender.Start();
                }
                else
                {
                    MessageBox.Show("Debes introducir nombe y contraseña");
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
                IPAddress direc = IPAddress.Parse("147.83.117.22");
                IPEndPoint ipep = new IPEndPoint(direc, puerto);

                string Nombre = Nombre_Login_txt.Text;
                nombreConectado = Nombre;
                string Contraseña = Convert.ToString(Contraseña_Login_txt.Text);

                //Creamos el socket 
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                if ((Nombre_Login_txt.Text != "") && (Contraseña_Login_txt.Text != ""))
                {
                    try
                    {

                        server.Connect(ipep);  //Intentamos connectar el socket.
                        //Ahora construiremos el mensaje, el cual tendrá la sigueinte estructura: "1/Nombre/Contraseña.
                        string mensaje = "2/" + Nombre + "/" + Contraseña;

                        //Enviamos al servidor el mensaje.
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                    }
                    catch
                    {
                        //Si hay excepcion imprimimos error y salimos del programa con return 
                        MessageBox.Show("No he podido conectar con el servidor");
                        return;
                    }
                    //pongo en marcha el thread que atenderá los mensajes del servidor
                    ThreadStart ts = delegate { AtenderServidor(); };
                    atender = new Thread(ts);
                    atender.Start();
                }
                else
                    {
                        MessageBox.Show("Debes introducir nombre y contraseña");
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
                        string mensaje = "3/Consulta 1/";
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);

                    }

                    else if (Consulta2.Checked)
                    {
                        if (JugadorConsulta_txt.Text=="")
                        {
                            MessageBox.Show("Tienes que introducir el nombre de un jugador");
                        }
                        else
                        {
                        string mensaje = "4/Consulta 2/" + JugadorConsulta_txt.Text;
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);

                        }
                    }

                    else if (Consulta3.Checked)
                    {
                        string mensaje = "5/Consulta 3/" + RolConsultas_txt.Text + "/" + FechaConsultas_txt.Text;
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                        
                         
                    }

                    else if (Consulta4.Checked)
                    {
                        if (JugadorConsulta_txt.Text == "")
                        {
                            MessageBox.Show("Tienes que introducir el nombre de un jugador");
                        }
                        else
                        {
                            string mensaje = "6/Consulta 4/" + JugadorConsulta_txt.Text;
                            // Enviamos al servidor el nombre tecleado
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                            server.Send(msg);

                        }

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
                atender.Abort();
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

        private void Invitar_button_Click(object sender, EventArgs e)
        {
            string mensaje = "7/" + nombreConectado + "/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        private void enviarRespuestaInvitacion(string nombreRespuesta, int respuesta)
        {
            string mensaje = "71/" + nombreRespuesta + "/" + respuesta + "/";   //Se envía con la cabezera 71, el nombre del jugador que acaba de responder, seguido por su respuesta (0: No acepta, 1:Acepta).
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        private void Nombre_Register_txt_TextChanged(object sender, EventArgs e)
        {
            Nombre_Register_txt.MaxLength = 10;
        }

        private void Contraseña_Register_txt_TextChanged(object sender, EventArgs e)
        {
            Contraseña_Register_txt.MaxLength = 10;
        }

        private void Nombre_Login_txt_TextChanged(object sender, EventArgs e)
        {
            Nombre_Login_txt.MaxLength = 10;
        }

        private void Contraseña_Login_txt_TextChanged(object sender, EventArgs e)
        {
            Contraseña_Login_txt.MaxLength = 10;
        }

    }
}
