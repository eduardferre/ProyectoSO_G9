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
        int puerto = 9060;
        string nombreConectado;
        Socket server;
        Thread atender;
        int conectado = 0;
        int numConectados; //Número de conectados sin contar el propio cliente. 
        int numRespuestas; //Número de respuestas a la invitación que se han recibido (tanto afirmativas como negativas).
        int numRespuestasAceptar; //Número de jugadores que han aceptado la invitación de jugar.
        int numInvitacionesIndividuales;
        int InvitacionAceptada = 0;

        List<string> listaConectados = new List<string>();
        List<string> listaInvitadosIndividuales = new List<string>();
        int invitacionActiva; //Parámetro de correción de errores que no permite que el jugador haga invitaciones para jugar hasta que las actuales invitaciones se hayan respondido.
        string nombres_invitados = "";

        delegate void DelegadoParaEscribir(string mensaje);
        delegate void DelegadoInvitacionRecibida(string mensaje);
        delegate void DelegadoInicioSesion();
        delegate void DelegadoListaConectados(string ListaConectados);
        delegate void DelegadoInvitacionRechazada();
        delegate void DelegadoDesconexion();
        delegate void DelegadoActivarInvitacion();

        public void DelegarInicioSesion()
        {
            // Es pot usar per el tema de desactivar botons a l'iniciar sessió i així no haver de fer correcció d'errors, ho deixo en blanc perquè de moment no ens implica res, ja que està corregit en codi.
        }
        public void DelegarDesconexion()
        {
            // Exactament el mateix que el comentat anteriorment.
        }
        public void DelegarInvitacionRecibida(string UsuarioInvitacion)
        {
            // Per si ho necessitem en un futur
        }
        public void DelegarActivarInvitacion()
        {
            // Per activar el botó d'invitació, per si ho necessitem
        }
        public void DelegarInvitacionRechazada()
        {
            // Per activar el botó d'invitació, així poder convidar si ha denegat invitar
        }
       

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false; //Necesario para que los elementos de los formularios puedan ser
            //accedidos desde threads diferentes a los que los crearon
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            numConectados = 0;
            numRespuestas = 0;
            numRespuestasAceptar = 0;
            numInvitacionesIndividuales = 0;
            invitacionActiva = 0;
        }

        private void AtenderServidor()
        {
            while (true)
            {
                //Recibimos el mensaje del servidor
                byte[] msg2 = new byte[80];
 
                try
                {
                    server.Receive(msg2);
                }
                catch (SocketException ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }

                string[] trozos = Encoding.ASCII.GetString(msg2).Split('/');

                int codigo = Convert.ToInt32(trozos[0]);
                string mensaje = trozos[1].Split('\0')[0];

                switch (codigo)
                {
                    case 1: //registro
                        if (mensaje == "OK")
                        {
                            // Si usem els delegats aquí haurai d'haver-hi un DelegadoInicioSesion
                            MessageBox.Show("Jugador registrado correctamente.");
                            conectado = 1;
                            this.BackColor = Color.Green;
                        }
                        else if (mensaje == "NO")
                        {
                            string mensaje2 = "0/" + nombreConectado;
                            byte[] msg3 = System.Text.Encoding.ASCII.GetBytes(mensaje2);
                            server.Send(msg3);
                            server.Shutdown(SocketShutdown.Both);
                            server.Close();
                            MessageBox.Show("El usario no se ha añadido porque ya existe");
                            this.BackColor = Color.Gray;
                            atender.Abort();
                        }
                        else if (mensaje == "LLENA")
                        {
                            string mensaje2 = "0/" + nombreConectado;
                            byte[] msg3 = System.Text.Encoding.ASCII.GetBytes(mensaje2);
                            server.Send(msg3);
                            server.Shutdown(SocketShutdown.Both);
                            server.Close();
                            MessageBox.Show("El usario no se ha añadido porque el servidor esta saturado.");
                            this.BackColor = Color.Gray;
                            atender.Abort();
                        }
                        else
                        {
                            string mensaje2 = "0/" + nombreConectado;
                            byte[] msg3 = System.Text.Encoding.ASCII.GetBytes(mensaje2);
                            server.Send(msg3);
                            server.Shutdown(SocketShutdown.Both);
                            server.Close();
                            MessageBox.Show("Se ha producido un error en la base de datos.");
                            this.BackColor = Color.Gray;
                            atender.Abort();
                        }
                            
                        break;

                    case 2: //login
                        string Nombre = Nombre_Login_txt.Text;
                        string Contraseña = Convert.ToString(Contraseña_Login_txt.Text);

                        if (mensaje == "FOUND")
                        {
                            // Si usem els delegats aquí haurai d'haver-hi un DelegadoInicioSesion
                            nombreConectado = Nombre_Login_txt.Text;
                            MessageBox.Show("Bienvenido de nuevo " + Nombre + ".");
                            this.BackColor = Color.Green;
                            conectado = 1;
                        }
                        else if (mensaje == "NOT_FOUND")
                        {
                            string mensaje2 = "0/" + nombreConectado;
                            byte[] msg3 = System.Text.Encoding.ASCII.GetBytes(mensaje2);
                            server.Send(msg3);
                            server.Shutdown(SocketShutdown.Both);
                            server.Close();
                            MessageBox.Show("Lo siento, no estás registrad@ todavía.");
                            this.BackColor = Color.Gray;
                            atender.Abort();
                        }
                        else if (mensaje == "NOT_PASS")
                        {
                            string mensaje2 = "0/" + nombreConectado;
                            byte[] msg3 = System.Text.Encoding.ASCII.GetBytes(mensaje2);
                            server.Send(msg3);
                            server.Shutdown(SocketShutdown.Both);
                            server.Close();
                            MessageBox.Show("Contraseña incorrecta, vuelve a intentarlo.");
                            this.BackColor = Color.Gray;
                            atender.Abort();
                        }
                        else if (mensaje == "LLENA")
                        {
                            string mensaje2 = "0/" + nombreConectado;
                            byte[] msg3 = System.Text.Encoding.ASCII.GetBytes(mensaje2);
                            server.Send(msg3);
                            server.Shutdown(SocketShutdown.Both);
                            server.Close();
                            MessageBox.Show("El servidor está saturado, intentalo más tarde.");
                            this.BackColor = Color.Gray;
                            atender.Abort();
                        }
                        else
                        {
                            string mensaje2 = "0/" + nombreConectado;
                            byte[] msg3 = System.Text.Encoding.ASCII.GetBytes(mensaje2);
                            server.Send(msg3);
                            server.Shutdown(SocketShutdown.Both);
                            server.Close();
                            MessageBox.Show("Se ha producido un error con la base de datos.");
                            this.BackColor = Color.Gray;
                            atender.Abort();
                        }
                                   
                        break;

                    case 3: //Consulta 1

                        if (mensaje == "NO")
                        {
                            MessageBox.Show("No hay aún partidas.");
                        }
                        else if (mensaje == "ERROR")
                        {
                            MessageBox.Show("Se ha producido un error con la base de datos.");
                        }
                        else
                        {
                            MessageBox.Show("La media de la duración de las partidas es: " + mensaje + " segundos");
                        }
                        
                        break;

                    case 4: //Consulta 2

                        if (mensaje == "NO")
                        {
                            MessageBox.Show("No hay ningún jugador que coincida.");
                        }
                        else if (mensaje == "ERROR")
                        {
                            MessageBox.Show("Se ha producido un error con la base de datos.");
                        }
                        else
                        {
                            MessageBox.Show("La duración de la partida más larga de " + JugadorConsulta_txt.Text + " es: " + mensaje);
                        }
                       
                        break;

                    case 5: //Consulta 3

                        if (mensaje == "NO")
                        {
                            MessageBox.Show("No hay ninguna coincidencia.");
                        }
                        else if (mensaje == "ERROR")
                        {
                            MessageBox.Show("Se ha producido un error con la base de datos.");
                        }
                        else
                        {
                            MessageBox.Show("El jugador con más victorias siendo " + RolConsultas_txt.Text + " el día " + FechaConsultas_txt.Text + " es: " + mensaje);
                        }

                        break;

                    case 6: //Consulta 4

                        if (mensaje == "NO")
                        {
                            MessageBox.Show("No hay ninguna coincidencia.");
                        }
                        else if (mensaje == "ERROR")
                        {
                            MessageBox.Show("Se ha producido un error con la base de datos.");
                        }
                        else
                        {
                            MessageBox.Show("El rol de la partida más larga de " + JugadorConsulta_txt.Text + " es: " + mensaje);
                        }
                        
                        break;

                    case 7: //Invitación para jugar

                        // Si usem els delegats aquí haurai d'haver-hi un DelegadoInvitacionRecibida

                        string nombre_huesped = mensaje;

                        if (nombre_huesped != nombreConectado) //El propio huésped no debe ver la invitación generada por si mismo.
                        {
                            DialogResult dialogResult;
                            dialogResult = MessageBox.Show(nombre_huesped + " ha enviado una solicitud para iniciar la partida. ¿Deseas iniciar la partida ahora? Todos los jugadores deben estar de acuerdo para iniciar la partida.", "Confirmación", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                InvitacionAceptada = 1;
                                enviarRespuestaInvitacion_Aceptada(nombre_huesped, nombreConectado); // Se ha aceptado la invitación.
                            }
                            else
                            {
                                InvitacionAceptada = 0;
                                enviarRespuestaInvitacion_Rechazada(nombre_huesped, nombreConectado); // Se ha denegado la invitación.
                            }
                        }
                        break;

                    case 70: // Respuesta a la invitación aceptada
                        string nombre_respuesta_aceptada = trozos[1];

                        Respuestas_lbl.AppendText(nombre_respuesta_aceptada); //En este caso, uno mismo sí va a poder verse en el label de personas que han aceptado.
                        Respuestas_lbl.AppendText(Environment.NewLine);
                        numRespuestasAceptar++;
                        numRespuestas++;

                        if (numRespuestas == numInvitacionesIndividuales) //Significa que ya han respondido todos los que habían sido invitados.
                        {
                            if (numRespuestasAceptar == numRespuestas) //Significa que todas las respuestas son afirmativas => Empezará la partida.
                            {
                                invitacionActiva = 0;
                                InvitadosIndividuales_lbl.Clear();
                                MessageBox.Show("Pantalla de: " + nombreConectado + "Todos los jugadores han aceptado la invitación. La partida puede empezar.");
                            }
                            else //Sigifica que alguien ha denegado la invitación => La partida NO empezará.
                            {
                                invitacionActiva = 0;
                                numInvitacionesIndividuales = 0;
                                InvitadosIndividuales_lbl.Clear();
                                Respuestas_lbl.Clear();
                                MessageBox.Show("Pantalla de: " + nombreConectado + " La partida no empezará. NO todos los jugadores han aceptado la invitación.");
                            }
                            numRespuestas = 0;
                            numRespuestasAceptar = 0;
                        }
                        break;

                    case 71: // Respuesta a la invitación denegada
                        string nombre_respuesta_rechazada = trozos[1];

                        numRespuestas++;

                        if (numRespuestas == numInvitacionesIndividuales) //Significa que ya han respondido todos los que habían sido invitados.
                        {
                            if (numRespuestasAceptar == numRespuestas) //Significa que todas las respuestas son afirmativas => Empezará la partida.
                            {
                                invitacionActiva = 0;
                                InvitadosIndividuales_lbl.Clear();
                                MessageBox.Show("Pantalla de: " + nombreConectado + "Todos los jugadores han aceptado la invitación. La partida puede empezar.");
                            }
                            else //Sigifica que alguien ha denegado la invitación => La partida NO empezará.
                            {
                                invitacionActiva = 0;
                                numInvitacionesIndividuales = 0;
                                InvitadosIndividuales_lbl.Clear();
                                Respuestas_lbl.Clear();
                                MessageBox.Show("Pantalla de: " + nombreConectado + " La partida no empezará. NO todos los jugadores han aceptado la invitación.");
                            }
                            numRespuestas = 0;
                            numRespuestasAceptar = 0;
                        }
                        break;
                        
                    case 80: //Enviar mensaje
                        
                        string chat = trozos[1];
                        MensajesRecibidos_lbl.AppendText(chat);
                        MensajesRecibidos_lbl.AppendText(Environment.NewLine);
                       
                           
                    break; 


                    case 110: //Lista conectados
                        Conectados_lbl.Clear();
                        listaConectados.Clear();
                        
                        numConectados = 0;
                        string[] separadas;
                        string[] nombre_prov;
                        separadas = mensaje.Split('/');

                        for (int i = 0; i < Convert.ToInt32(separadas[0]); i++)
                        {
                            nombre_prov = trozos[i + 2].Split('\0');

                            listaConectados.Add(nombre_prov[0]);

                            if (nombre_prov[0] != nombreConectado)  //De esta forma, el jugador conectado no ve su propio nombre dentro de la lista de conectados, solo los de los demás.
                            {
                                Conectados_lbl.AppendText(nombre_prov[0]);
                                Conectados_lbl.AppendText(Environment.NewLine);
                                numConectados++;
                            }
                        }
                        
                        break;

                    case 111: //Número de servicios

                        DelegadoParaEscribir delegado = new DelegadoParaEscribir(PonContador);
                        Servicios_lbl.Invoke(delegado, new object[] { mensaje });  //Invocamos al thread que creó este objeto para que haga lo que se especifica a continuacion.
                        
                        break;
                }
            }
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
                IPAddress direc = IPAddress.Parse("192.168.56.102");
                IPEndPoint ipep = new IPEndPoint(direc, puerto);

                string Nombre = Nombre_Register_txt.Text;
                nombreConectado = Nombre;
                string Contraseña = Convert.ToString(Contraseña_Register_txt.Text);

                //Creamos el socket 
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                if ((Nombre_Register_txt.Text != "") && (Contraseña_Register_txt.Text != ""))
                {
                    try
                    {
                        server.Connect(ipep);  //Intentamos connectar el socket.


                        //Ahora construiremos el mensaje, el cual tendrá la sigueinte estructura: "1/Nombre/Contraseña.
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
                    MessageBox.Show("Debes introducir nombre y contraseña");
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
                string mensaje = "0/" + nombreConectado;

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                Conectados_lbl.Clear();
                Servicios_lbl.Text = "";

                // Nos desconectamos
                atender.Abort();
                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();

                conectado = 0;
                MessageBox.Show("Te has desconectado!");
            }
            else
            {
                MessageBox.Show("Eh, que no estas conectado! Puedes volverte a conectar haciendo Login ;D");
            }
        }

        private void Invitar_todos_button_Click(object sender, EventArgs e)
        {
            if (invitacionActiva == 0)
            {
                nombres_invitados = "";
                int Seleccionados = listaConectados.Count - 1;
                int i = 0;

                if (Seleccionados != 0)
                {
                    if (Seleccionados != listaInvitadosIndividuales.Count)
                    {
                        for (i = 0; i <= Seleccionados; i++)
                        {

                            if ((listaConectados[i] != nombreConectado) && (listaInvitadosIndividuales.Count == 0))
                            {
                                InvitadosIndividuales_lbl.AppendText(listaConectados[i]);
                                InvitadosIndividuales_lbl.AppendText(Environment.NewLine);
                                listaInvitadosIndividuales.Add(listaConectados[i]);
                                nombres_invitados = nombres_invitados + listaConectados[i] + "/";
                                numInvitacionesIndividuales++;
                            }
                            else if ((listaConectados[i] != nombreConectado) && (Buscar_EnLista(listaConectados[i], listaInvitadosIndividuales) != 1))
                            {
                                InvitadosIndividuales_lbl.AppendText(listaConectados[i]);
                                InvitadosIndividuales_lbl.AppendText(Environment.NewLine);
                                listaInvitadosIndividuales.Add(listaConectados[i]);
                                nombres_invitados = nombres_invitados + listaConectados[i] + "/";
                                numInvitacionesIndividuales++;
                            }
                        }

                        string mensaje = "7/" + numInvitacionesIndividuales + "/" + nombreConectado + ":" + nombres_invitados;
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);

                        invitacionActiva = 1;
                    }
                    else
                    {
                        MessageBox.Show("Ya has invitado a todos los jugadores conectados.");
                    }
                }
                else
                {
                    MessageBox.Show("No hay nadie conectado.");
                }
            }
            else
            {
                MessageBox.Show("Ya hay una o más peticiones en curso. Podrá iniciar otra petición una vez haya sido resulta la que está activa.");
            }
        }

        private void Invitar_Individual_button_Click(object sender, EventArgs e)
        {
            if (Invitado_Individual_txt.Text != "")
            {
                if (invitacionActiva == 0)
                {
                    nombres_invitados = "";
                    int Seleccionados = listaConectados.Count - 1;

                    if (Seleccionados != 0)
                    {
                        if (Seleccionados != listaInvitadosIndividuales.Count)
                        {
                            if (Buscar_EnLista(Invitado_Individual_txt.Text, listaConectados) == 1)
                            {
                                if ((Invitado_Individual_txt.Text != nombreConectado) && (Buscar_EnLista(Invitado_Individual_txt.Text, listaInvitadosIndividuales) != 1))
                                {
                                    InvitadosIndividuales_lbl.AppendText(Invitado_Individual_txt.Text);
                                    InvitadosIndividuales_lbl.AppendText(Environment.NewLine);
                                    listaInvitadosIndividuales.Add(Invitado_Individual_txt.Text);
                                    nombres_invitados = nombres_invitados + Invitado_Individual_txt.Text + "/";
                                    numInvitacionesIndividuales++;
                                }
                                else
                                {
                                    MessageBox.Show("No te puedes invitar a ti mismo o a alguien que ya ha sido invitado.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Este jugador no esta conectado.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ya has invitado a todos los jugadores conectados.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No hay nadie conectado.");
                    }
                }
                else
                {
                    MessageBox.Show("Ya hay una o más peticiones en curso. Podrá iniciar otra petición una vez haya sido resulta la que está activa.");
                }
            }
            else
            {
                MessageBox.Show("Escribe un jugador antes de nada.");
            }
        }

        private void EnviarPeticiones_button_Click(object sender, EventArgs e)
        {
            if (InvitadosIndividuales_lbl.Text != "")
            {
                if (invitacionActiva == 0)
                {
                    if (listaInvitadosIndividuales.Count != 0)
                    {
                        int i = 0;
                        while (i < listaInvitadosIndividuales.Count)
                        {
                            string mensaje = "7/" + numInvitacionesIndividuales + "/" + nombreConectado + ":" + nombres_invitados;
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                            server.Send(msg);
                            i++;
                        }

                        invitacionActiva = 1;
                    }
                    else
                    {
                        MessageBox.Show("No hay jugadores seleccionados todavía.");
                    }
                }
                else
                {
                    MessageBox.Show("Ya hay una o más peticiones en curso. Podrá iniciar otra petición una vez haya sido resulta la que está activa.");
                }
            }
            else
            {
                MessageBox.Show("Selecciona un jugador primero.");
            }
        }

        private int Buscar_EnLista(string nombre, List<string> lista)
        {
            int i = 0;
            int encontrado = 0;

            while ((encontrado == 0) && (i < lista.Count))
            {
                if (nombre == lista[i])
                {
                    encontrado = 1;
                }

                i++;
            }

            return encontrado;
        }

        private void enviarRespuestaInvitacion_Aceptada(string nombreInvita, string nombreAcepta)
        {
            string mensaje = "70/" + nombreInvita + "/" + nombreAcepta + "/";   //Se envía con la cabezera 71, el nombre del jugador que acaba de responder, seguido por su respuesta (0: No acepta, 1:Acepta).
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        private void enviarRespuestaInvitacion_Rechazada(string nombreInvita, string nombreRechaza)
        {
            string mensaje = "71/" + nombreInvita + "/" + nombreRechaza + "/";   
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        private void EnviarMensaje_button_Click(object sender, EventArgs e)
        {
            string mensaje = "80/" + nombreConectado + ": " + Mensaje_txt.Text + "/";
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

        private void limpiar_invitaciones_button_Click(object sender, EventArgs e)
        {
            invitacionActiva = 0;
            numInvitacionesIndividuales = 0;
            InvitadosIndividuales_lbl.Clear();
            Respuestas_lbl.Clear();
            listaInvitadosIndividuales.Clear();
        }

    }
}
