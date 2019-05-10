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
        static int conteo = 0;
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
                                //introduce los valores divididos al diccionario
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
            //recibe la cadena de caracteres que se escribió en el cuadro de texto
            string SQL = Request.Form["SQLs"].ToString();
            char[] delimitadores = { ' ', ',', '(', ')', '\r', '"', '\n' };
            string[] separación = SQL.Split(delimitadores);
            string[] datos = new string[100];
            int i = 0;
            //introduce los valores a un nuevo vector
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
            //verifica si la tabla que se desea crear ya existe
            if (!repetido)
            {
                //verifica los que los valores que contenga el vector, también los contenga el diccionario
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
                                    //verifica que tipo de variables fueron las que se introdujeron
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
                                    //si todo fue introducido de la forma correcta, crea la tabla
                                    conteo++;
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
            if (conteo == 0)
            {
                return RedirectToAction("Menú");
            }
            else
            {
                return View();
            }
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
            //verifica si la escritura que se escribió en la casilla de texto, la contiene el diccionario
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
                //verifica si existe una tabla con el nombre que se introdujo 
                if (verdad == true)
                {
                    int s = 0;
                    int contador = 0;
                    //Verifica si se desea introducir un valor nuevo a la tabla existente o si se desea crear una nueva tabla
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
                                //convierte los valores a sus respectivas variables
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
                                //muestra la tabla y agrega los datos al arbol
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
                                return View("InserciónDatos");
                            }
                        }
                        else
                        {
                            return View("InserciónDatos");
                        }
                    }
                    else
                    {
                        //
                        List<Información> lista = new List<Información>();
                        List<string> nuevalistanombrecolumnas = new List<string>();
                        List<string> nuevalistadetiposdevalores = new List<string>();
                        arbolesb.BPTree<Información> auxiliar = new BPTree<Información>(4,4,3);
                        //encuentra las estructuras que se desea utilizar
                        nuevalistanombrecolumnas = listadelistadenombrescolumnas[j];
                        nuevalistadetiposdevalores = listadelistadetiposdevalores[j];
                        auxiliar = listadearboles[j];
                        lista = listadelistas[j];
                        //elimina los valores de sus respectivas listas debido a que al final se actualizaran todos juntos
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
                                //inserta los valores al arbol y se muestra en la lista
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
                                return View("InserciónDatos");
                            }
                        }
                        else
                        {
                            return View("InserciónDatos");
                        }
                    }
                }
                else
                {
                    return View("InserciónDatos");
                }
            }
            else
            {
                return View("InserciónDatos");
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
            //recibe dos textos la clave y el valor que se desea cambiar
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
                //Comprueba que la clave que se escribió esté dentro del diccionario
                if (dic.ContainsKey(clavenueva))
                {
                    //eliminar el valor viejo y vuelve a introducirlo al diccionario con el nuevo valor
                    dic.Remove(clavenueva);
                    dic[clavenueva] =valornuevo;
                    //escribe en el nuevo diccionario los valores actualizados del diccionario (clave,valor)
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
            if (conteo == 0)
            {
                return RedirectToAction("Menú");
            }
            else
            {
                return View();
            }
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
            //valida que la el valor que se escribió esté dentro del diccionario
            if (dic.ContainsValue(datos[0]))
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
                //verifica si la variable de la clave eliminar si posea el valor que se escribió
                if (GetAnyValue<string>("DELETE") == datos[0])
                {
                    
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
                                    //método de eliminar valores deseados del árbol, por la llave primaria
                                    List<Información> inf = new List<Información>();
                                    arbolesb.BPTree<Información> nuevo = new BPTree<Información>(4,4,3);
                                    List<string> nuevalistanombrecolumnas = new List<string>();
                                    List<string> nuevalistadetiposdevalores = new List<string>();
                                    nuevalistanombrecolumnas = listadelistadenombrescolumnas[n];
                                    nuevalistadetiposdevalores = listadelistadetiposdevalores[n];
                                    inf = listadelistas[n];
                                    nuevo = listadearboles[n];
                                    //eliminar los elementos de sus respectivas listas con el fin de actualizarlas y que todos los datos se encuentren en la misma posición
                                    listadelistas.Remove(inf);
                                    listadearboles.Remove(nuevo);
                                    listadelistadetiposdevalores.Remove(nuevalistadetiposdevalores);
                                    listadelistadenombrescolumnas.Remove(nuevalistanombrecolumnas);
                                    nombrestabla.Remove(datos[2]);
                                    Información x = new Información();
                                    int repetidos = 0;
                                    foreach(Información p in inf)
                                    {
                                        if (p.num == Convert.ToInt32(datos[5]))
                                        {
                                            nuevo.delete(p);
                                            x = p;
                                            repetidos++;
                                        }
                                    }
                                    
                                    if (repetidos > 0)
                                    {
                                        for(int t = 0; t < repetidos; t++)
                                        {
                                            inf.Remove(x);
                                        }
                                    }
                                    else
                                    {
                                        inf.Remove(x);
                                    }
                                    //Volver a insertar todos los valores y estructuras a sus respectivas listas para que se encuentren de forma ordenada
                                    listadelistadetiposdevalores.Add(nuevalistadetiposdevalores);
                                    listadelistadenombrescolumnas.Add(nuevalistanombrecolumnas);
                                    nombrestabla.Add(datos[2]);
                                    listadearboles.Add(nuevo);
                                    listadelistas.Add(inf);
                                    return View("Menú");
                                }
                                else
                                {
                                    return RedirectToAction("Eliminación");
                                }
                            }
                            else
                            {
                                //método para eliminar todos los valores del árbol, sin embargo, deja las claves libres
                                List<Información> lista = new List<Información>();
                                arbolesb.BPTree<Información> arbolaux = new BPTree<Información>(4,4,3);
                                List<string> nuevalistanombrecolumnas = new List<string>();
                                List<string> nuevalistadetiposdevalores = new List<string>();

                                //encontrar los valores 
                                nuevalistanombrecolumnas = listadelistadenombrescolumnas[n];
                                nuevalistadetiposdevalores = listadelistadetiposdevalores[n];
                                lista = listadelistas[n];
                                arbolaux = listadearboles[n];
                                //eliminar 
                                listadelistadetiposdevalores.Remove(nuevalistadetiposdevalores);
                                listadelistadenombrescolumnas.Remove(nuevalistanombrecolumnas);
                                listadelistas.Remove(lista);
                                listadearboles.Remove(arbolaux);
                                nombrestabla.Remove(datos[2]);
                                int p = 0;
                                Información infos = new Información();
                                foreach(Información info in lista)
                                {
                                    arbolaux.delete(info);
                                    p++;
                                }
                                
                                for(int j = 0; j < p; j++)
                                {
                                    infos=lista[0];
                                    lista.Remove(infos);
                                }
                                //Introducir los valores nuevamente, actualizar la información
                                listadelistadetiposdevalores.Add(nuevalistadetiposdevalores);
                                listadelistadenombrescolumnas.Add(nuevalistanombrecolumnas);
                                nombrestabla.Add(datos[2]);
                                listadearboles.Add(arbolaux);
                                listadelistas.Add(lista);
                                return RedirectToAction("InserciónDatos");
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
                    if (GetAnyValue<string>("DROP") == datos[0])
                    {
                        //eliminar la estructura completa, elimina el árbol completo como si nunca hubiera existido
                        listadelistas.Remove(listadelistas[n]);
                        listadearboles.Remove(listadearboles[n]);
                        nombrestabla.Remove(nombrestabla[n]);
                        listadelistadenombrescolumnas.Remove(listadelistadenombrescolumnas[n]);
                        listadelistadetiposdevalores.Remove(listadelistadetiposdevalores[n]);
                        return RedirectToAction("CreaciónTabla");
                    }
                    else
                    {
                        return RedirectToAction("Eliminación");
                    }
                }
            }
            else
            {
                return RedirectToAction("Eliminación");
            }
        }

        //Método busqueda
        public ActionResult Busqueda()
        {
            return View();
        }
        public ActionResult Busqueda2()
        {
            string insertar = Request.Form["buscar"].ToString();
            char[] delimitadores = { ' ', ',', '(', ')', '\r', '"', '\n', '=' };
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
            if(GetAnyValue<string>("SELECT") == datos[0])
            {
                bool co = false;
                int k = 0;
                foreach (string l in datos)
                {
                    if (GetAnyValue<string>("FROM") == l)
                    {
                        co = true;
                        break;
                    }
                    else
                    {
                        k++;
                    }
                }
                if (co == true)
                {
                    int b = 0;
                    bool existetabla = false;
                    foreach(string nombre in nombrestabla)
                    {
                        if (datos[k + 1] == nombre)
                        {
                            existetabla = true;
                            break;
                        }
                        else
                        {
                            b++;
                        }
                    }
                    if (existetabla)
                    {
                        List<Información> lista = new List<Información>();
                        lista = listadelistas[b];
                        arbolesb.BPTree<Información> arbolauxiliar = new BPTree<Información>(4,4,3);
                        arbolauxiliar=listadearboles[b];
                        List<Información> aux = new List<Información>();
                        arbolesb.Node<Información> arb = new Node<Información>(4,true);
                        Información num = new Información();
                        num.num = 1;
                        arb=arbolauxiliar.search(num);
                        aux=arb.getKeys();
                        if (datos[1] == "*")
                        {
                            if (datos.Count()==4)
                            {
                                return View(lista);
                            }
                            else
                            {
                                if (GetAnyValue<string>("WHERE") == datos[k + 2])
                                {
                                    List<Información> auxi = new List<Información>();
                                    foreach(Información info in lista)
                                    {
                                        if (info.num == Convert.ToInt32(datos[k + 4]))
                                        {
                                            auxi.Add(info);
                                        }
                                    }
                                    return View(auxi);
                                }
                                else
                                {
                                    return RedirectToAction("Busqueda");
                                }
                            }
                        }
                        else
                        {
                            Información ult = new Información();
                            List<Información> auxiliar = new List<Información>();
                            List<string> tip = new List<string>();
                            List<string> tip2 = new List<string>();
                            tip = listadelistadenombrescolumnas[b];
                            tip2 = listadelistadetiposdevalores[b];
                            List<string> d = new List<string>();
                            
                            int r = 0;
                            int varchar = 0;
                            int entero = 0;
                            int tiempo = 0;
                            if (!datos.Contains("WHERE"))
                            {
                                foreach(string l in datos)
                                {
                                    if (l == tip[r])
                                    {
                                        d.Add(l);
                                        r++;
                                    }
                                }
                                int s = 0;
                                int q = 0;
                                foreach(Información t in lista)
                                {
                                    ult = new Información();
                                    foreach (string l in tip)
                                    {
                                        if (s < d.Count())
                                        {
                                            if (l == d[s])
                                            {
                                                if (tip2[q] == "INT")
                                                {
                                                    if (entero == 0)
                                                    {
                                                        ult.num = t.num;
                                                    }
                                                    else
                                                    {
                                                        if (entero == 1)
                                                        {
                                                            ult.num2 = t.num2;
                                                        }
                                                        else
                                                        {
                                                            if (entero == 2)
                                                            {
                                                                ult.num3 = t.num3;
                                                            }
                                                        }
                                                    }
                                                    entero++;
                                                }
                                                else
                                                {
                                                    if (tip2[q] == "VARCHAR")
                                                    {
                                                        if (varchar == 0)
                                                        {
                                                            ult.varchar = t.varchar;
                                                        }
                                                        else
                                                        {
                                                            if (varchar == 1)
                                                            {
                                                                ult.varchar2 = t.varchar2;
                                                            }
                                                            else
                                                            {
                                                                if (varchar == 2)
                                                                {
                                                                    ult.varchar3 = t.varchar3;
                                                                }
                                                            }
                                                        }
                                                        varchar++;
                                                    }
                                                    else
                                                    {
                                                        if (tip2[q] == "DATETIME")
                                                        {
                                                            if (tiempo == 0)
                                                            {
                                                                ult.tiempo = t.tiempo;
                                                            }
                                                            else
                                                            {
                                                                if (tiempo == 1)
                                                                {
                                                                    ult.tiempo2 = t.tiempo2;
                                                                }
                                                                else
                                                                {
                                                                    if (tiempo == 2)
                                                                    {
                                                                        ult.tiempo3 = t.tiempo3;
                                                                    }
                                                                }
                                                            }
                                                            tiempo++;
                                                        }
                                                    }
                                                }
                                                s++;
                                            }
                                        }
                                        q++;
                                    }
                                    s = 0;
                                    q = 0;
                                    auxiliar.Add(ult);
                                }
                                return View(auxiliar);
                            }
                            else
                            {
                                foreach (string l in datos)
                                {
                                    if (l == tip[r])
                                    {
                                        d.Add(l);
                                        r++;
                                    }
                                }
                                int s = 0;
                                int q = 0;
                                foreach (Información t in lista)
                                {
                                    if (t.num == Convert.ToInt32(datos[k + 4]))
                                    {
                                        ult = new Información();
                                        foreach (string l in tip)
                                        {
                                            if (s < d.Count())
                                            {
                                                if (l == d[s])
                                                {
                                                    if (tip2[q] == "INT")
                                                    {
                                                        if (entero == 0)
                                                        {
                                                            ult.num = t.num;
                                                        }
                                                        else
                                                        {
                                                            if (entero == 1)
                                                            {
                                                                ult.num2 = t.num2;
                                                            }
                                                            else
                                                            {
                                                                if (entero == 2)
                                                                {
                                                                    ult.num3 = t.num3;
                                                                }
                                                            }
                                                        }
                                                        entero++;
                                                    }
                                                    else
                                                    {
                                                        if (tip2[q] == "VARCHAR")
                                                        {
                                                            if (varchar == 0)
                                                            {
                                                                ult.varchar = t.varchar;
                                                            }
                                                            else
                                                            {
                                                                if (varchar == 1)
                                                                {
                                                                    ult.varchar2 = t.varchar2;
                                                                }
                                                                else
                                                                {
                                                                    if (varchar == 2)
                                                                    {
                                                                        ult.varchar3 = t.varchar3;
                                                                    }
                                                                }
                                                            }
                                                            varchar++;
                                                        }
                                                        else
                                                        {
                                                            if (tip2[q] == "DATETIME")
                                                            {
                                                                if (tiempo == 0)
                                                                {
                                                                    ult.tiempo = t.tiempo;
                                                                }
                                                                else
                                                                {
                                                                    if (tiempo == 1)
                                                                    {
                                                                        ult.tiempo2 = t.tiempo2;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (tiempo == 2)
                                                                        {
                                                                            ult.tiempo3 = t.tiempo3;
                                                                        }
                                                                    }
                                                                }
                                                                tiempo++;
                                                            }
                                                        }
                                                    }
                                                    s++;
                                                }
                                            }
                                            q++;
                                        }
                                        s = 0;
                                        q = 0;
                                        auxiliar.Add(ult);
                                    }
                                }
                                return View(auxiliar);
                            }
                        }
                    }
                    else
                    {
                        return RedirectToAction("Busqueda");
                    }
                }
                else
                {
                    return RedirectToAction("Busqueda");
                }
            }
            else
            {
                return RedirectToAction("Busqueda");
            }
        }
    }
}