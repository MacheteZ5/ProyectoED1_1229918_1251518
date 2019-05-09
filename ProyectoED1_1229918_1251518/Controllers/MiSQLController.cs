using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoED1_1229918_1251518.Models;
using arbolesb;
namespace ProyectoED1_1229918_1251518.Controllers
{
    public class MiSQLController : Controller
    {
        // GET: MiSQL
        public ActionResult Index()
        {
            return View();
        }
        static Dictionary<string, object> dic = new Dictionary<string, object>();
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            if (dic.Count == 0)
            {
                int contador = 0;
                string filePath = string.Empty;
                if (postedFile != null)
                {
                    //dirección del archivo
                    string path = Server.MapPath("~/archivo/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    string csvData = System.IO.File.ReadAllText(filePath);

                    foreach (string row in csvData.Split('\r'))
                    {
                        if ((!string.IsNullOrEmpty(row)) && (contador != 0))
                        {
                            if (row != "\n")
                            {
                                Diccionario dicc = new Diccionario();
                                string extra = row.Split('\n')[0];
                                string clave = row.Split(';')[1];
                                string valor = row.Split(';')[1];
                                object valores = valor;
                                dic.Add(clave, valores);
                                dicc.clave = clave;
                                dicc.Valor = valor;
                            }
                        }
                        else
                        {
                            contador++;
                        }
                    }
                }
            }
            return View();
        }
        public ActionResult Menú()
        {
            return View();
        }
        /// <summary>
        // Esta función devuelve un valor tipo T que contenga el diccionario, pero solamente devuelve el valor no la llave
        private static T GetAnyValue<T>(string strKey)
        {
            object obj;
            T retType;

            dic.TryGetValue(strKey, out obj);
            try
            {
                retType = (T)obj;
            }
            catch
            {
                retType = default(T);
            }
            return retType;
        }
        /// </summary>
        //Aquí se guardará los nombres de las tablas
        static List<string> nombrestabla = new List<string>();
        //Aquí se guardarán los nombres de las columnas 
        static List<string> nombrecolumnas = new List<string>();
        //Aquí se guardaran las listas de los nombres de las columnas
        static List<List<string>> listadelistadenombrescolumnas = new List<List<string>>();
        //aquí se guardarán las listas de los tipos de datos(INT, VARCHAR, DATETIME)
        static List<List<string>> listadelistadetiposdevalores = new List<List<string>>();
        //Aquí se guardarán los tipos de valores(Int, Varchar, Datetime)
        static List<string> tiposdevalores = new List<string>();
        //Aquí se guardarán las listas de las listas que se poseen actualmente
        static List<List<Información>> listadelistas = new List<List<Información>>();
        //Aquí se guardaran las estructuras de ArbolB+
        static List<arbolesb.BPTree<Información>> listadearboles = new List<BPTree<Información>>();
        //valida si se crea una nueva tabla o no
        static bool crearnuevo = false;
        //Ambos metodos sirven para la lectura y la creación de la tabla (árbol)
        public ActionResult CreaciónTabla()
        {
            return View();
        }
        public ActionResult CreaciónTabla2()
        {
            tiposdevalores = new List<string>();
            nombrecolumnas = new List<string>();
            if (listadelistas.Count() == 0)
            {
                nombrestabla = new List<string>();
            }
            string SQL = Request.Form["SQLs"].ToString();
            char[] delimitadores = { ' ', ',', '(', ')', '\r', '"', '\n' };
            string[] separación = SQL.Split(delimitadores);
            string[] datos = new string[100];
            int i = 0;
            foreach (string linea in separación)
            {
                if (linea != "")
                {
                    datos[i] = linea;
                    i++;
                }
            }
            bool repetido = false;
            foreach (string nombre in nombrestabla)
            {
                if (datos[2] == nombre)
                {
                    repetido = true;
                    break;
                }
            }
            if (!repetido)
            {
                if (dic.ContainsValue(datos[0]))
                {
                    if (GetAnyValue<string>("CREATE") == datos[0])
                    {
                        if (datos[1] == "TABLE")
                        {
                            int m = 0;
                            string nombredelacolumna;
                            int conteostring = 0;
                            int conteoint = 0;
                            int conteoDatetime = 0;
                            bool cierto = false;
                            foreach (string linea in datos)
                            {
                                if (linea != null)
                                {
                                    if (linea == "INT" || linea == "VARCHAR" || linea == "DATETIME")
                                    {
                                        if (linea == "VARCHAR")
                                        {
                                            nombrecolumnas.Add(datos[m - 1]);
                                            tiposdevalores.Add("VARCHAR");
                                            conteostring++;
                                        }
                                        else
                                        {
                                            if (linea == "INT")
                                            {
                                                nombrecolumnas.Add(datos[m - 1]);
                                                nombredelacolumna = datos[m - 1];
                                                conteoint++;
                                                tiposdevalores.Add("INT");
                                            }
                                            else
                                            {
                                                if (linea == "DATETIME")
                                                {
                                                    nombrecolumnas.Add(datos[m - 1]);
                                                    nombredelacolumna = datos[m - 1];
                                                    conteoDatetime++;
                                                    tiposdevalores.Add("DATETIME");
                                                }
                                            }
                                        }
                                    }
                                    m++;
                                }
                                else
                                {
                                    if (linea == "GO")
                                    {
                                        cierto = true;
                                    }
                                }
                            
                            }
                            if (conteoint == 0)
                            {
                                return RedirectToAction("CreaciónTabla");
                            }
                            else
                            {
                                if (!cierto)
                                {
                                    nombrestabla.Add(datos[2]);
                                    listadelistadetiposdevalores.Add(tiposdevalores);
                                    listadelistadenombrescolumnas.Add(nombrecolumnas);
                                    crearnuevo = true;
                                    return RedirectToAction("InserciónDatos");
                                }
                                else
                                {
                                    return RedirectToAction("CreaciónTabla");
                                }
                            }
                        }
                        else
                        {
                            return RedirectToAction("CreaciónTabla");
                        }
                    }
                    else
                    {
                        return RedirectToAction("CreaciónTabla");
                    }
                }
                else
                {
                    return RedirectToAction("CreaciónTabla");
                }
            }
            else
            {
                return RedirectToAction("CreaciónTabla");
            }
        }

        //Aquí se incertarán los valores a las tablas
        public ActionResult InserciónDatos()
        {
            return View();
        }
        public ActionResult InserciónDatos2()
        {
            string insertar = Request.Form["valores"].ToString();
            char[] delimitadores = { ' ', ',', '(', ')', '\r', '"', '\n' };
            string[] separación = insertar.Split(delimitadores);
            List<string> datos = new List<string>();
            //string[] datos = new string[100];
            int i = 0;
            foreach (string k in separación)
            {
                if (k != "")
                {
                    datos.Add(k);
                    i++;
                }
            }
            string cadena = datos[0] + " " + datos[1];
            if (GetAnyValue<string>("INSERT INTO") == cadena)
            {
                bool verdad = false;
                int j = 0;
                foreach (string nombre in nombrestabla)
                {
                    if (nombre == datos[2])
                    {
                        verdad = true;
                        break;
                    }
                    j++;
                }
                if (verdad == true)
                {
                    int s = 0;
                    int contador = 0;
                    if (crearnuevo == true)
                    {
                        foreach (string d in datos)
                        {
                            if (s != nombrecolumnas.Count())
                            {
                                if (nombrecolumnas[s] == d)
                                {
                                    s++;
                                    contador++;
                                }
                            }
                        }
                        if (contador == nombrecolumnas.Count())
                        {
                            Models.Información info = new Models.Información();
                            int cantidad = 0;
                            int cantidad2 = 0;
                            int cantidad3 = 0;
                            foreach (string lineas in tiposdevalores)
                            {
                                if (lineas == "INT")
                                {
                                    cantidad++;
                                }
                                else
                                {
                                    if (lineas == "DATETIME")
                                    {
                                        cantidad2++;
                                    }
                                    else
                                    {
                                        if (lineas == "VARCHAR")
                                        {
                                            cantidad3++;
                                        }
                                    }
                                }
                            }

                            string[] valor = new string[10];
                            int q = 0;
                            bool x = false;
                            while (q != datos.Count())
                            {
                                if (q != (datos.Count() - 1))
                                {
                                    if (GetAnyValue<string>("VALUES") == datos[q])
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        q++;
                                    }
                                }
                                else
                                {
                                    x = true;
                                }
                            }
                            if (x != true)
                            {
                                int y = q + 1;
                                for (int ñ = 0; ñ < datos.Count(); ñ++)
                                {
                                    if (y != datos.Count())
                                    {
                                        if (datos[y] != null)
                                        {
                                            valor[ñ] = datos[y];
                                            y++;
                                        }
                                    }
                                }
                                int r = 0;
                                int rastreoint = 0;
                                int rastreovarchar = 0;
                                int rastreodatetime = 0;
                                foreach (string l in tiposdevalores)
                                {
                                    if (l == "INT")
                                    {
                                        if (cantidad == 1)
                                        {
                                            info.num = Convert.ToInt32(valor[r]);
                                            r++;
                                        }
                                        else
                                        {
                                            if (cantidad == 2)
                                            {
                                                if (rastreoint == 0)
                                                {
                                                    info.num = Convert.ToInt32(valor[r]);
                                                    r++;
                                                    rastreoint++;
                                                }
                                                else
                                                {
                                                    if (rastreoint == 1)
                                                    {
                                                        info.num2 = Convert.ToInt32(valor[r]);
                                                        r++;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (cantidad == 3)
                                                {
                                                    if (rastreoint == 0)
                                                    {
                                                        info.num = Convert.ToInt32(valor[r]);
                                                        rastreoint++;
                                                        r++;
                                                    }
                                                    else
                                                    {
                                                        if (rastreoint == 1)
                                                        {
                                                            try
                                                            {
                                                                info.num2 = Convert.ToInt32(valor[r]);
                                                                rastreoint++;
                                                                r++;
                                                            }
                                                            catch
                                                            {
                                                                return RedirectToAction("Conección2");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (rastreoint == 2)
                                                            {
                                                                info.num3 = Convert.ToInt32(valor[r]);
                                                                r++;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (l == "DATETIME")
                                        {
                                            if (cantidad2 == 1)
                                            {
                                                info.tiempo = Convert.ToDateTime(valor[r]);
                                                r++;
                                            }
                                            else
                                            {
                                                if (cantidad2 == 2)
                                                {
                                                    if (rastreodatetime == 0)
                                                    {
                                                        info.tiempo = Convert.ToDateTime(valor[r]);
                                                        rastreodatetime++;
                                                        r++;
                                                    }
                                                    else
                                                    {
                                                        if (rastreodatetime == 1)
                                                        {
                                                            info.tiempo2 = Convert.ToDateTime(valor[r]);
                                                            r++;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (cantidad2 == 3)
                                                    {
                                                        if (rastreodatetime == 0)
                                                        {
                                                            info.tiempo = Convert.ToDateTime(valor[r]);
                                                            rastreodatetime++;
                                                            r++;
                                                        }
                                                        else
                                                        {
                                                            if (rastreodatetime == 1)
                                                            {
                                                                info.tiempo2 = Convert.ToDateTime(valor[r]);
                                                                rastreodatetime++;
                                                                r++;
                                                            }
                                                            else
                                                            {
                                                                if (rastreodatetime == 2)
                                                                {
                                                                    info.tiempo3 = Convert.ToDateTime(valor[r]);
                                                                    r++;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (l == "VARCHAR")
                                            {
                                                if (cantidad3 == 1)
                                                {
                                                    info.varchar = Convert.ToString(valor[r]);
                                                    r++;
                                                }
                                                else
                                                {
                                                    if (cantidad3 == 2)
                                                    {
                                                        if (rastreovarchar == 0)
                                                        {
                                                            info.varchar = Convert.ToString(valor[r]);
                                                            rastreovarchar++;
                                                            r++;
                                                        }
                                                        else
                                                        {
                                                            if (rastreovarchar == 1)
                                                            {
                                                                info.varchar2 = Convert.ToString(valor[r]);
                                                                r++;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (cantidad3 == 3)
                                                        {
                                                            if (rastreovarchar == 0)
                                                            {
                                                                info.varchar = Convert.ToString(valor[r]);
                                                                rastreovarchar++;
                                                                r++;
                                                            }
                                                            else
                                                            {
                                                                if (rastreovarchar == 1)
                                                                {
                                                                    info.varchar2 = Convert.ToString(valor[r]);
                                                                    rastreovarchar++;
                                                                    r++;
                                                                }
                                                                else
                                                                {
                                                                    if (rastreovarchar == 2)
                                                                    {
                                                                        info.varchar3 = Convert.ToString(valor[r]);
                                                                        r++;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                                nombrecolumnas = null;
                                tiposdevalores = null;
                                crearnuevo = false;
                                arbolesb.BPTree<Información> arbol = new BPTree<Información>(4, 4, 3);
                                arbol.insetNode(info, 0);
                                List<Información> aux = new List<Información>();
                                aux.Add(info);
                                listadearboles.Add(arbol);
                                listadelistas.Add(aux);
                                return View(aux);
                            }
                            else
                            {
                                return View("InserciónDatos2");
                            }
                        }
                        else
                        {
                            return View("InserciónDatos2");
                        }
                    }
                    else
                    {
                        //corregir esto
                        List<Información> lista = new List<Información>();
                        List<string> nuevalistanombrecolumnas = new List<string>();
                        List<string> nuevalistadetiposdevalores = new List<string>();
                        arbolesb.BPTree<Información> auxiliar = new BPTree<Información>(4,4,3);
                        nuevalistanombrecolumnas = listadelistadenombrescolumnas[j];
                        nuevalistadetiposdevalores = listadelistadetiposdevalores[j];
                        auxiliar = listadearboles[j];
                        lista = listadelistas[j];
                        listadearboles.Remove(auxiliar);
                        listadelistas.Remove(lista);
                        listadelistadenombrescolumnas.Remove(nuevalistanombrecolumnas);
                        listadelistadetiposdevalores.Remove(nuevalistadetiposdevalores);
                        nombrestabla.Remove(datos[2]);
                        foreach (string d in datos)
                        {
                            if (s != nuevalistanombrecolumnas.Count())
                            {
                                if (nuevalistanombrecolumnas[s] == d)
                                {
                                    s++;
                                    contador++;
                                }
                            }
                        }
                        if (contador == nuevalistanombrecolumnas.Count())
                        {
                            Models.Información info = new Models.Información();
                            int cantidad = 0;
                            int cantidad2 = 0;
                            int cantidad3 = 0;
                            foreach (string lineas in nuevalistadetiposdevalores)
                            {
                                if (lineas == "INT")
                                {
                                    cantidad++;
                                }
                                else
                                {
                                    if (lineas == "DATETIME")
                                    {
                                        cantidad2++;
                                    }
                                    else
                                    {
                                        if (lineas == "VARCHAR")
                                        {
                                            cantidad3++;
                                        }
                                    }
                                }
                            }
                            string[] valor = new string[10];
                            int q = 0;
                            bool x = false;
                            while (q != datos.Count())
                            {
                                if (q != (datos.Count() - 1))
                                {
                                    if (GetAnyValue<string>("VALUES") == datos[q])
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        q++;
                                    }
                                }
                                else
                                {
                                    x = true;
                                }
                            }
                            if (x != true)
                            {
                                int y = q + 1;
                                for (int ñ = 0; ñ < datos.Count(); ñ++)
                                {
                                    if (y != datos.Count())
                                    {
                                        if (datos[y] != null)
                                        {
                                            valor[ñ] = datos[y];
                                            y++;
                                        }
                                    }
                                }
                                int r = 0;
                                int rastreoint = 0;
                                int rastreovarchar = 0;
                                int rastreodatetime = 0;
                                foreach (string l in nuevalistadetiposdevalores)
                                {
                                    if (l == "INT")
                                    {
                                        if (cantidad == 1)
                                        {
                                            info.num = Convert.ToInt32(valor[r]);
                                            r++;
                                        }
                                        else
                                        {
                                            if (cantidad == 2)
                                            {
                                                if (rastreoint == 0)
                                                {
                                                    info.num = Convert.ToInt32(valor[r]);
                                                    r++;
                                                    rastreoint++;
                                                }
                                                else
                                                {
                                                    if (rastreoint == 1)
                                                    {
                                                        info.num2 = Convert.ToInt32(valor[r]);
                                                        r++;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (cantidad == 3)
                                                {
                                                    if (rastreoint == 0)
                                                    {
                                                        info.num = Convert.ToInt32(valor[r]);
                                                        rastreoint++;
                                                        r++;
                                                    }
                                                    else
                                                    {
                                                        if (rastreoint == 1)
                                                        {
                                                            info.num2 = Convert.ToInt32(valor[r]);
                                                            rastreoint++;
                                                            r++;
                                                        }
                                                        else
                                                        {
                                                            if (rastreoint == 2)
                                                            {
                                                                info.num3 = Convert.ToInt32(valor[r]);
                                                                r++;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (l == "DATETIME")
                                        {
                                            if (cantidad2 == 1)
                                            {
                                                info.tiempo = Convert.ToDateTime(valor[r]);
                                                r++;
                                            }
                                            else
                                            {
                                                if (cantidad2 == 2)
                                                {
                                                    if (rastreodatetime == 0)
                                                    {
                                                        info.tiempo = Convert.ToDateTime(valor[r]);
                                                        rastreodatetime++;
                                                        r++;
                                                    }
                                                    else
                                                    {
                                                        if (rastreodatetime == 1)
                                                        {
                                                            info.tiempo2 = Convert.ToDateTime(valor[r]);
                                                            r++;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (cantidad2 == 3)
                                                    {
                                                        if (rastreodatetime == 0)
                                                        {
                                                            info.tiempo = Convert.ToDateTime(valor[r]);
                                                            rastreodatetime++;
                                                            r++;
                                                        }
                                                        else
                                                        {
                                                            if (rastreodatetime == 1)
                                                            {
                                                                info.tiempo2 = Convert.ToDateTime(valor[r]);
                                                                rastreodatetime++;
                                                                r++;
                                                            }
                                                            else
                                                            {
                                                                if (rastreodatetime == 2)
                                                                {
                                                                    info.tiempo3 = Convert.ToDateTime(valor[r]);
                                                                    r++;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (l == "VARCHAR")
                                            {
                                                if (cantidad3 == 1)
                                                {
                                                    info.varchar = Convert.ToString(valor[r]);
                                                    r++;
                                                }
                                                else
                                                {
                                                    if (cantidad3 == 2)
                                                    {
                                                        if (rastreovarchar == 0)
                                                        {
                                                            info.varchar = Convert.ToString(valor[r]);
                                                            rastreovarchar++;
                                                            r++;
                                                        }
                                                        else
                                                        {
                                                            if (rastreovarchar == 1)
                                                            {
                                                                info.varchar2 = Convert.ToString(valor[r]);
                                                                r++;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (cantidad3 == 3)
                                                        {
                                                            if (rastreovarchar == 0)
                                                            {
                                                                info.varchar = Convert.ToString(valor[r]);
                                                                rastreovarchar++;
                                                                r++;
                                                            }
                                                            else
                                                            {
                                                                if (rastreovarchar == 1)
                                                                {
                                                                    info.varchar2 = Convert.ToString(valor[r]);
                                                                    rastreovarchar++;
                                                                    r++;
                                                                }
                                                                else
                                                                {
                                                                    if (rastreovarchar == 2)
                                                                    {
                                                                        info.varchar3 = Convert.ToString(valor[r]);
                                                                        r++;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                                //arbol
                                auxiliar.insetNode(info, 0);
                                listadearboles.Add(auxiliar);
                                //tabla nombres
                                nombrestabla.Add(datos[2]);
                                //listas de columnas y tipo de valor
                                listadelistadenombrescolumnas.Add(nuevalistanombrecolumnas);
                                listadelistadetiposdevalores.Add(nuevalistadetiposdevalores);
                                //lista de listas
                                lista.Add(info);
                                listadelistas.Add(lista);
                                return View(lista);
                            }
                            else
                            {
                                return View("InserciónDatos2");
                            }
                        }
                        else
                        {
                            return View("InserciónDatos2");
                        }
                    }
                }
                else
                {
                    return View("InserciónDatos2");
                }
            }
            else
            {
                return View("InserciónDatos2");
            }
        }

        //Aquí el usuario podrá realizar un cambio de valores al diccionario, solamente los valores no las claves.
        public ActionResult CambioValor()
        {
            return View();
        }
        private string ruta = AppDomain.CurrentDomain.BaseDirectory + "NuevoDiccionario.csv";
        public ActionResult CambioValor2()
        {
            string clave = Request.Form["clave"].ToString();
            string valor = Request.Form["valor"].ToString();
            char[] delimitadores = { ' ', '"' };
            string clavenueva="";
            string valornuevo="";
            string[] trozo = valor.Split(delimitadores);
            string[] trozo2 = clave.Split(delimitadores);
            int m = 0;
            int q = 0;
            foreach(string l in trozo2)
            {
                if (l != "")
                {
                    if (clavenueva == "")
                    {
                        clavenueva = l;
                    }
                    m++;
                }
            }
            foreach(string l in trozo)
            {
                if (l != "")
                {
                    if (valornuevo=="")
                    {
                        valornuevo = l;
                    }
                    q++;
                }
                
            }
            if (q == 1 && m == 1)
            {
                if (dic.ContainsKey(clavenueva))
                {
                    dic.Remove(clavenueva);
                    dic[clavenueva] =valornuevo;

                    StreamWriter writer = new StreamWriter(ruta);
                    string contenido = null;
                    foreach (KeyValuePair<string, object> k in dic)
                    {
                        if (k.Key != null)
                        {
                            contenido = string.Format("{0},{1}", k.Key, k.Value);
                            writer.WriteLine(contenido);
                        }
                    }
                    writer.Close();
                    return RedirectToAction("Menú");
                }
                else
                {
                    return RedirectToAction("CambioValor");
                }
            }
            else
            {
                return RedirectToAction("CambioValor");
            }
        }
        //Metodo de eliminación del árbol
        public ActionResult Eliminación()
        {
            return View();
        }
        public ActionResult Eliminación2()
        {
            string insertar = Request.Form["eliminación"].ToString();
            char[] delimitadores = { ' ', ',', '(', ')', '\r', '"', '\n' ,'='};
            string[] separación = insertar.Split(delimitadores);
            List<string> datos = new List<string>();
            int i = 0;
            foreach (string k in separación)
            {
                if (k != "")
                {
                    datos.Add(k);
                    i++;
                }
            }
            if (dic.ContainsValue(datos[0]))
            {
                if (GetAnyValue<string>("DELETE") == datos[0])
                {
                    int n = 0;
                    bool nombreencontrado = false;
                    foreach (string l in nombrestabla)
                    {
                        if (datos[2] == l)
                        {
                            nombreencontrado = true;
                            break;
                        }
                        else
                        {
                            n++;
                        }
                    }
                    if (nombreencontrado)
                    {
                        int c = 0;
                        foreach(string l in datos)
                        {
                            c++;
                        }
                        if (c>2)
                        {
                            if (c > 3)
                            {
                                if (GetAnyValue<string>("WHERE") == datos[3])
                                {
                                    List<Información> inf = new List<Información>();
                                    arbolesb.BPTree<Información> nuevo = new BPTree<Información>(4,4,3);
                                    int k = 0;
                                    foreach (List<Información> info in listadelistas)
                                    {
                                        if (k == n)
                                        {
                                            inf = info;
                                            break;
                                        }
                                        else
                                        {
                                            k++;
                                        }
                                    }
                                    k = 0;
                                    foreach (arbolesb.BPTree<Información> a in listadearboles)
                                    {
                                        if (k == n)
                                        {
                                           nuevo = a;
                                           break;
                                        }
                                        else
                                        {
                                            k++;
                                        }
                                    }
                                    
                                    Información inv = new Información();
                                    arbolesb.Node<Información> p = new Node<Información>(4,true);
                                    inv.num = Convert.ToInt32(datos[5]);
                                    p=nuevo.search(inv);
                                    inv = p.getKeys()[0];
                                    nuevo.delete(inv);
                                }
                                else
                                {
                                    return RedirectToAction("Eliminación");
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            return RedirectToAction("Eliminación");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Eliminación");
                    }
                }
                else
                {
                    return RedirectToAction("Eliminación");
                }
            }
            else
            {
                return RedirectToAction("Eliminación");
            }

           return View();
        }
    }
}