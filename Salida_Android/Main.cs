using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using System.Data;

namespace Salida_Android
{
    [Activity(Label = "Main")]
    public class Main : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Button button = FindViewById<Button>(Resource.Id.button1);
            EditText entrada = FindViewById<EditText>(Resource.Id.editText1);
            TextView fechaE = FindViewById<TextView>(Resource.Id.textView2);
            TextView fechaS = FindViewById<TextView>(Resource.Id.textView3);

            button.Click += delegate
            {
                string connsqlstring = "Server=" + Intent.Extras.GetString("servidor") + ";Port=3306;database=estacionamiento;User Id=" + Intent.Extras.GetString("user") + ";Password=" + Intent.Extras.GetString("pass") + ";charset=utf8";

                // aqui checa en la tabla pago
                try
                {
                    MySqlConnection sqlconn = new MySqlConnection(connsqlstring);
                    sqlconn.Open();

                    string Query = "SELECT * FROM pago WHERE idfolio = " + entrada.Text;
                    DataTable t = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(Query, sqlconn);
                    da.Fill(t);
                    if (t.Rows.Count != 0)
                    {
                        DataRow row = t.Rows[0];
                        string folios = row["idfolio"].ToString();
                        if (folios == entrada.Text)
                        {
                            DateTime date1 = new DateTime(Convert.ToInt32(row["año_entrada"].ToString()), Convert.ToInt32(row["mes_entrada"].ToString()), Convert.ToInt32(row["dia_entrada"].ToString()), Convert.ToInt32(row["hora_entrada"].ToString()), Convert.ToInt32(row["minutos_entrada"].ToString()), Convert.ToInt32(row["segundos_entrada"].ToString()));
                            DateTime date2 = DateTime.Now;
                            fechaE.Text = "Fecha Entrada: " + date1.Day + "/" + date1.Month + "/" + date1.Year + " " + date1.Hour + ":" + date1.Minute + ":" + date1.Second;
                            fechaS.Text = "Fecha Salida: " + date2.Day + "/" + date2.Month + "/" + date2.Year + " " + date2.Hour + ":" + date2.Minute + ":" + date2.Second;
                            //TimeSpan ts = date2 - date1;
                            //int diferencia = ts.Minutes;
                            //if (diferencia <= 15)
                            //{
                                try
                                {
                                    DateTime thisDay = DateTime.Now;

                                    MySqlConnection db = new MySqlConnection(connsqlstring);
                                    db.Open();

                                    string consulta = "INSERT INTO salida(idfolio,hora_entrada,minutos_entrada,segundos_entrada,dia_entrada,mes_entrada,año_entrada,hora_salida,minuto_salida,segundos_salida,dia_salida,mes_salida,año_salida,pagado)" +
                                    " VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{1},{12},'{13}');";
                                    consulta = String.Format(consulta, folios, Convert.ToInt32(row["hora_entrada"].ToString()), Convert.ToInt32(row["minutos_entrada"].ToString()), Convert.ToInt32(row["segundos_entrada"].ToString()), Convert.ToInt32(row["dia_entrada"].ToString()), Convert.ToInt32(row["mes_entrada"].ToString()), Convert.ToInt32(row["año_entrada"].ToString()),
                                        thisDay.Hour, thisDay.Minute, thisDay.Second, thisDay.Day, thisDay.Month, thisDay.Year, row["pago"].ToString());



                                    MySqlCommand instruccion = new MySqlCommand(consulta, db);
                                    instruccion.ExecuteNonQuery();
                                    db.Close();
                                    Toast.MakeText(this, "Salida Registrada Con Exito", ToastLength.Long).Show();
                                }
                                catch (Exception ex)
                                {
                                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                                }
                            }
                            try
                            {
                                MySqlConnection db2 = new MySqlConnection(connsqlstring);
                                db2.Open();


                                string eliminar = "DELETE FROM pago WHERE idfolio = {0};";
                                eliminar = string.Format(eliminar, folios);
                                MySqlCommand instruccionEL = new MySqlCommand(eliminar, db2);
                                instruccionEL.ExecuteNonQuery();
                                db2.Close();
                                Toast.MakeText(this, "Modificado con exito", ToastLength.Long).Show();

                            }
                            catch (MySqlException ex)
                            {

                            }
                        }
                    //}
                }
                catch (MySqlException ex)
                {

                }

                // aqui checa en la tabla android
                try
                {
                    MySqlConnection sqlconn2 = new MySqlConnection(connsqlstring);
                    sqlconn2.Open();

                    string Query2 = "SELECT * FROM android WHERE idfolio = " + entrada.Text;
                    DataTable t2 = new DataTable();
                    MySqlDataAdapter da2 = new MySqlDataAdapter(Query2, sqlconn2);
                    da2.Fill(t2);
                    if (t2.Rows.Count != 0)
                    {
                        DataRow row2 = t2.Rows[0];
                        string folios2 = row2["idfolio"].ToString();
                        if (folios2 == entrada.Text)
                        {

                            DateTime date1 = new DateTime(Convert.ToInt32(row2["año_entrada"].ToString()), Convert.ToInt32(row2["mes_entrada"].ToString()), Convert.ToInt32(row2["dia_entrada"].ToString()), Convert.ToInt32(row2["hora_entrada"].ToString()), Convert.ToInt32(row2["minutos_entrada"].ToString()), Convert.ToInt32(row2["segundos_entrada"].ToString()));
                            DateTime date2 = DateTime.Now;
                            fechaE.Text = "Fecha Entrada: " + date1.Day + "/" + date1.Month + "/" + date1.Year + " " + date1.Hour + ":" + date1.Minute + ":" + date1.Second;
                            fechaS.Text = "Fecha Salida: " + date2.Day + "/" + date2.Month + "/" + date2.Year + " " + date2.Hour + ":" + date2.Minute + ":" + date2.Second;
                            TimeSpan ts = date2 - date1;
                            int diferencia = ts.Minutes;
                            if (diferencia <= 15)
                            {
                                try
                                {
                                    DateTime thisDay = DateTime.Now;

                                    MySqlConnection db = new MySqlConnection(connsqlstring);
                                    db.Open();

                                    string consulta = "INSERT INTO salida(idfolio,hora_entrada,minutos_entrada,segundos_entrada,dia_entrada,mes_entrada,año_entrada,hora_salida,minuto_salida,segundos_salida,dia_salida,mes_salida,año_salida,pagado)" +
                                    " VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{1},{12},Pagado);";
                                    consulta = String.Format(consulta, folios2, Convert.ToInt32(row2["hora_entrada"].ToString()), Convert.ToInt32(row2["minutos_entrada"].ToString()), Convert.ToInt32(row2["segundos_entrada"].ToString()), Convert.ToInt32(row2["dia_entrada"].ToString()), Convert.ToInt32(row2["mes_entrada"].ToString()), Convert.ToInt32(row2["año_entrada"].ToString()),
                                        thisDay.Hour, thisDay.Minute, thisDay.Second, thisDay.Day, thisDay.Month, thisDay.Year);

                                    MySqlCommand instruccion = new MySqlCommand(consulta, db);
                                    instruccion.ExecuteNonQuery();
                                    db.Close();
                                    Toast.MakeText(this, "Salida Registrada Con Exito", ToastLength.Long).Show();
                                }
                                catch (Exception ex)
                                {
                                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                                }

                                try
                                {
                                    MySqlConnection db2 = new MySqlConnection(connsqlstring);
                                    db2.Open();


                                    string eliminar = "DELETE FROM android WHERE idfolio = {0};";
                                    eliminar = string.Format(eliminar, folios2);

                                    MySqlCommand instruccionEL = new MySqlCommand(eliminar, db2);
                                    instruccionEL.ExecuteNonQuery();
                                    db2.Close();
                                    Toast.MakeText(this, "Modificado con exito", ToastLength.Long).Show();

                                }
                                catch (MySqlException ex)
                                {

                                }
                            }
                            else
                            {
                                Toast.MakeText(this, "No Puede Salir, Valla y PAGUE", ToastLength.Long).Show();
                            }

                        }
                    }
                }
                catch (Exception ex)
                {

                }
            };
        }
    }
}