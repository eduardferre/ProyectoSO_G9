﻿namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.EnviarConsultas_button = new System.Windows.Forms.Button();
            this.RolConsultas_txt = new System.Windows.Forms.ComboBox();
            this.FechaConsultas_txt = new System.Windows.Forms.DateTimePicker();
            this.JugadorConsulta_txt = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.Consulta4 = new System.Windows.Forms.RadioButton();
            this.Consulta2 = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.Consulta1 = new System.Windows.Forms.RadioButton();
            this.Consulta3 = new System.Windows.Forms.RadioButton();
            this.Registro = new System.Windows.Forms.GroupBox();
            this.Contraseña_Register_txt = new System.Windows.Forms.TextBox();
            this.Nombre_Register_txt = new System.Windows.Forms.TextBox();
            this.Register_button = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Login = new System.Windows.Forms.GroupBox();
            this.Desconectar_button = new System.Windows.Forms.Button();
            this.Login_button = new System.Windows.Forms.Button();
            this.Login_LBL = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Contraseña_Login_txt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Nombre_Login_txt = new System.Windows.Forms.TextBox();
            this.Conectados_lbl = new System.Windows.Forms.TextBox();
            this.Servicios_lbl = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.Registro.SuspendLayout();
            this.Login.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(267, 45);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 29);
            this.label2.TabIndex = 1;
            this.label2.Text = "Fecha:";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.groupBox1.Controls.Add(this.EnviarConsultas_button);
            this.groupBox1.Controls.Add(this.RolConsultas_txt);
            this.groupBox1.Controls.Add(this.FechaConsultas_txt);
            this.groupBox1.Controls.Add(this.JugadorConsulta_txt);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.Consulta4);
            this.groupBox1.Controls.Add(this.Consulta2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.Consulta1);
            this.groupBox1.Controls.Add(this.Consulta3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(18, 442);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(1188, 231);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Peticiones";
            // 
            // EnviarConsultas_button
            // 
            this.EnviarConsultas_button.Location = new System.Drawing.Point(519, 186);
            this.EnviarConsultas_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.EnviarConsultas_button.Name = "EnviarConsultas_button";
            this.EnviarConsultas_button.Size = new System.Drawing.Size(159, 35);
            this.EnviarConsultas_button.TabIndex = 16;
            this.EnviarConsultas_button.Text = "Enviar";
            this.EnviarConsultas_button.UseVisualStyleBackColor = true;
            this.EnviarConsultas_button.Click += new System.EventHandler(this.EnviarConsultas_button_Click);
            // 
            // RolConsultas_txt
            // 
            this.RolConsultas_txt.FormattingEnabled = true;
            this.RolConsultas_txt.Items.AddRange(new object[] {
            "INFILTRADO",
            "HACKER"});
            this.RolConsultas_txt.Location = new System.Drawing.Point(76, 42);
            this.RolConsultas_txt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RolConsultas_txt.Name = "RolConsultas_txt";
            this.RolConsultas_txt.Size = new System.Drawing.Size(180, 28);
            this.RolConsultas_txt.TabIndex = 15;
            this.RolConsultas_txt.Text = "INFILTRADO";
            // 
            // FechaConsultas_txt
            // 
            this.FechaConsultas_txt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.FechaConsultas_txt.Location = new System.Drawing.Point(363, 43);
            this.FechaConsultas_txt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FechaConsultas_txt.MaxDate = new System.DateTime(2020, 12, 31, 0, 0, 0, 0);
            this.FechaConsultas_txt.MinDate = new System.DateTime(2020, 1, 1, 0, 0, 0, 0);
            this.FechaConsultas_txt.Name = "FechaConsultas_txt";
            this.FechaConsultas_txt.Size = new System.Drawing.Size(132, 26);
            this.FechaConsultas_txt.TabIndex = 14;
            this.FechaConsultas_txt.Value = new System.DateTime(2020, 10, 25, 0, 0, 0, 0);
            // 
            // JugadorConsulta_txt
            // 
            this.JugadorConsulta_txt.Location = new System.Drawing.Point(1022, 49);
            this.JugadorConsulta_txt.Name = "JugadorConsulta_txt";
            this.JugadorConsulta_txt.Size = new System.Drawing.Size(112, 26);
            this.JugadorConsulta_txt.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label8.Location = new System.Drawing.Point(912, 45);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 29);
            this.label8.TabIndex = 11;
            this.label8.Text = "Jugador:";
            // 
            // Consulta4
            // 
            this.Consulta4.AutoSize = true;
            this.Consulta4.Location = new System.Drawing.Point(696, 128);
            this.Consulta4.Name = "Consulta4";
            this.Consulta4.Size = new System.Drawing.Size(405, 24);
            this.Consulta4.TabIndex = 13;
            this.Consulta4.TabStop = true;
            this.Consulta4.Text = "Rol en la partida más larga del jugador seleccionado.";
            this.Consulta4.UseVisualStyleBackColor = true;
            // 
            // Consulta2
            // 
            this.Consulta2.AutoSize = true;
            this.Consulta2.Location = new System.Drawing.Point(696, 92);
            this.Consulta2.Name = "Consulta2";
            this.Consulta2.Size = new System.Drawing.Size(445, 24);
            this.Consulta2.TabIndex = 9;
            this.Consulta2.TabStop = true;
            this.Consulta2.Text = "Duración de la partida más larga del jugador seleccionado.";
            this.Consulta2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label5.Location = new System.Drawing.Point(18, 45);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 29);
            this.label5.TabIndex = 10;
            this.label5.Text = "Rol:";
            // 
            // Consulta1
            // 
            this.Consulta1.AutoSize = true;
            this.Consulta1.Location = new System.Drawing.Point(58, 92);
            this.Consulta1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Consulta1.Name = "Consulta1";
            this.Consulta1.Size = new System.Drawing.Size(392, 24);
            this.Consulta1.TabIndex = 7;
            this.Consulta1.TabStop = true;
            this.Consulta1.Text = "Dime la media de la duración de todas las partidas.";
            this.Consulta1.UseVisualStyleBackColor = true;
            // 
            // Consulta3
            // 
            this.Consulta3.AutoSize = true;
            this.Consulta3.Location = new System.Drawing.Point(60, 128);
            this.Consulta3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Consulta3.Name = "Consulta3";
            this.Consulta3.Size = new System.Drawing.Size(460, 24);
            this.Consulta3.TabIndex = 8;
            this.Consulta3.TabStop = true;
            this.Consulta3.Text = "Dime el jugador con más victorias del rol y día seleccionados.";
            this.Consulta3.UseVisualStyleBackColor = true;
            // 
            // Registro
            // 
            this.Registro.BackColor = System.Drawing.Color.Silver;
            this.Registro.Controls.Add(this.Contraseña_Register_txt);
            this.Registro.Controls.Add(this.Nombre_Register_txt);
            this.Registro.Controls.Add(this.Register_button);
            this.Registro.Controls.Add(this.label4);
            this.Registro.Controls.Add(this.label3);
            this.Registro.Controls.Add(this.label1);
            this.Registro.Location = new System.Drawing.Point(18, 18);
            this.Registro.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Registro.Name = "Registro";
            this.Registro.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Registro.Size = new System.Drawing.Size(590, 414);
            this.Registro.TabIndex = 7;
            this.Registro.TabStop = false;
            this.Registro.Text = "Registro";
            // 
            // Contraseña_Register_txt
            // 
            this.Contraseña_Register_txt.Location = new System.Drawing.Point(210, 229);
            this.Contraseña_Register_txt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Contraseña_Register_txt.Name = "Contraseña_Register_txt";
            this.Contraseña_Register_txt.Size = new System.Drawing.Size(244, 26);
            this.Contraseña_Register_txt.TabIndex = 11;
            this.Contraseña_Register_txt.TextChanged += new System.EventHandler(this.Contraseña_Register_txt_TextChanged);
            // 
            // Nombre_Register_txt
            // 
            this.Nombre_Register_txt.Location = new System.Drawing.Point(210, 171);
            this.Nombre_Register_txt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Nombre_Register_txt.Name = "Nombre_Register_txt";
            this.Nombre_Register_txt.Size = new System.Drawing.Size(244, 26);
            this.Nombre_Register_txt.TabIndex = 10;
            this.Nombre_Register_txt.TextChanged += new System.EventHandler(this.Nombre_Register_txt_TextChanged);
            // 
            // Register_button
            // 
            this.Register_button.Location = new System.Drawing.Point(273, 312);
            this.Register_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Register_button.Name = "Register_button";
            this.Register_button.Size = new System.Drawing.Size(112, 35);
            this.Register_button.TabIndex = 9;
            this.Register_button.Text = "Registrarse";
            this.Register_button.UseVisualStyleBackColor = true;
            this.Register_button.Click += new System.EventHandler(this.Register_button_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(202, 68);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(189, 37);
            this.label4.TabIndex = 8;
            this.label4.Text = "REGISTRO";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 220);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(183, 37);
            this.label3.TabIndex = 7;
            this.label3.Text = "Contraseña";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(52, 162);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 37);
            this.label1.TabIndex = 5;
            this.label1.Text = "Nombre";
            // 
            // Login
            // 
            this.Login.BackColor = System.Drawing.Color.Silver;
            this.Login.Controls.Add(this.Desconectar_button);
            this.Login.Controls.Add(this.Login_button);
            this.Login.Controls.Add(this.Login_LBL);
            this.Login.Controls.Add(this.label6);
            this.Login.Controls.Add(this.Contraseña_Login_txt);
            this.Login.Controls.Add(this.label7);
            this.Login.Controls.Add(this.Nombre_Login_txt);
            this.Login.Location = new System.Drawing.Point(616, 18);
            this.Login.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Login.Name = "Login";
            this.Login.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Login.Size = new System.Drawing.Size(590, 414);
            this.Login.TabIndex = 8;
            this.Login.TabStop = false;
            this.Login.Text = "Login";
            // 
            // Desconectar_button
            // 
            this.Desconectar_button.BackColor = System.Drawing.Color.LightGray;
            this.Desconectar_button.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Desconectar_button.Location = new System.Drawing.Point(453, 369);
            this.Desconectar_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Desconectar_button.Name = "Desconectar_button";
            this.Desconectar_button.Size = new System.Drawing.Size(128, 35);
            this.Desconectar_button.TabIndex = 10;
            this.Desconectar_button.Text = "Desconectar";
            this.Desconectar_button.UseVisualStyleBackColor = false;
            this.Desconectar_button.Click += new System.EventHandler(this.Desconectar_button_Click);
            // 
            // Login_button
            // 
            this.Login_button.Location = new System.Drawing.Point(279, 312);
            this.Login_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Login_button.Name = "Login_button";
            this.Login_button.Size = new System.Drawing.Size(112, 35);
            this.Login_button.TabIndex = 9;
            this.Login_button.Text = "Login";
            this.Login_button.UseVisualStyleBackColor = true;
            this.Login_button.Click += new System.EventHandler(this.Login_button_Click);
            // 
            // Login_LBL
            // 
            this.Login_LBL.AutoSize = true;
            this.Login_LBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Login_LBL.Location = new System.Drawing.Point(246, 88);
            this.Login_LBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Login_LBL.Name = "Login_LBL";
            this.Login_LBL.Size = new System.Drawing.Size(122, 37);
            this.Login_LBL.TabIndex = 8;
            this.Login_LBL.Text = "LOGIN";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(16, 220);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(183, 37);
            this.label6.TabIndex = 7;
            this.label6.Text = "Contraseña";
            // 
            // Contraseña_Login_txt
            // 
            this.Contraseña_Login_txt.Location = new System.Drawing.Point(210, 229);
            this.Contraseña_Login_txt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Contraseña_Login_txt.Name = "Contraseña_Login_txt";
            this.Contraseña_Login_txt.Size = new System.Drawing.Size(244, 26);
            this.Contraseña_Login_txt.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(52, 162);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(132, 37);
            this.label7.TabIndex = 5;
            this.label7.Text = "Nombre";
            // 
            // Nombre_Login_txt
            // 
            this.Nombre_Login_txt.Location = new System.Drawing.Point(210, 169);
            this.Nombre_Login_txt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Nombre_Login_txt.Name = "Nombre_Login_txt";
            this.Nombre_Login_txt.Size = new System.Drawing.Size(244, 26);
            this.Nombre_Login_txt.TabIndex = 4;
            this.Nombre_Login_txt.TextChanged += new System.EventHandler(this.Nombre_Login_txt_TextChanged);
            // 
            // Conectados_lbl
            // 
            this.Conectados_lbl.BackColor = System.Drawing.SystemColors.Control;
            this.Conectados_lbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Conectados_lbl.Location = new System.Drawing.Point(1242, 86);
            this.Conectados_lbl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Conectados_lbl.Multiline = true;
            this.Conectados_lbl.Name = "Conectados_lbl";
            this.Conectados_lbl.ReadOnly = true;
            this.Conectados_lbl.Size = new System.Drawing.Size(203, 325);
            this.Conectados_lbl.TabIndex = 18;
            this.Conectados_lbl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Servicios_lbl
            // 
            this.Servicios_lbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Servicios_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F);
            this.Servicios_lbl.Location = new System.Drawing.Point(1242, 534);
            this.Servicios_lbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Servicios_lbl.Name = "Servicios_lbl";
            this.Servicios_lbl.Size = new System.Drawing.Size(203, 127);
            this.Servicios_lbl.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1274, 496);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(151, 20);
            this.label9.TabIndex = 21;
            this.label9.Text = "Número de servicios";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1265, 50);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(160, 20);
            this.label10.TabIndex = 22;
            this.label10.Text = "Lista de conecctados";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1473, 685);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.Servicios_lbl);
            this.Controls.Add(this.Conectados_lbl);
            this.Controls.Add(this.Login);
            this.Controls.Add(this.Registro);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.Registro.ResumeLayout(false);
            this.Registro.PerformLayout();
            this.Login.ResumeLayout(false);
            this.Login.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton Consulta1;
        private System.Windows.Forms.RadioButton Consulta3;
        private System.Windows.Forms.GroupBox Registro;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox Login;
        private System.Windows.Forms.Label Login_LBL;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Contraseña_Login_txt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox Nombre_Login_txt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton Consulta4;
        private System.Windows.Forms.RadioButton Consulta2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox JugadorConsulta_txt;
        private System.Windows.Forms.TextBox Contraseña_Register_txt;
        private System.Windows.Forms.TextBox Nombre_Register_txt;
        private System.Windows.Forms.Button Register_button;
        private System.Windows.Forms.Button Login_button;
        private System.Windows.Forms.TextBox Conectados_lbl;
        private System.Windows.Forms.Label Servicios_lbl;
        private System.Windows.Forms.ComboBox RolConsultas_txt;
        private System.Windows.Forms.DateTimePicker FechaConsultas_txt;
        private System.Windows.Forms.Button EnviarConsultas_button;
        private System.Windows.Forms.Button Desconectar_button;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
    }
}
